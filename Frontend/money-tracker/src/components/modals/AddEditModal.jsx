import { api } from "../../boot/axios";
import { BaseModal } from "./BaseModal";
import { IndexedSelect } from "../common/IndexedSelect";
import { Input } from "../common/Input";
import { notifyError, notifySuccess } from "../../utils/notify";
import { useCallback, useEffect, useId, useState } from "react";

export function AddEditModal({
    endpoint,
    displayName,
    indexKey,
    modalMode = "add",
    record = {},
    properties = [],
    show = false,
    onOk = () => { },
    onClose = () => { },
}) {
    const uniqueId = useId();
    const componentId = `add-edit-modal-${uniqueId}`;

    const [isValid, setIsValid] = useState(false);
    const [formData, setFormData] = useState({});

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({ ...prevData, [name]: value }));
    };

    const handleSelectChange = ({ optionValue, optionLabel, option }) => {
        setFormData((prevData) => ({
            ...prevData,
            [optionValue]: option[optionValue],
            [optionLabel]: option[optionLabel],
        }));
    };

    const validateForm = useCallback(() => {
        const isValid = properties.every((property) => {
            if (property.required && !formData[property.name])
                return false;

            if (property.type === "number") {
                const value = parseFloat(formData[property.name]);
                return !isNaN(value) && (!property.min || value >= property.min) && (!property.max || value <= property.max);
            }

            if (property.type === "text") {
                const value = formData[property.name];
                return (!property.min || value.length >= property.min) && (!property.max || value.length <= property.max);
            }
            return true;
        });
    
        setIsValid(isValid);
    }, [formData, properties]);

    const handleSubmit = async () => {
        const method = modalMode === "add" ? "post" : "put";

        try {
            const { data } = await api[method](`/${endpoint}`, formData);
            if (data.status) {
                onOk({ ...formData, [indexKey]: data.response[indexKey] });
                notifySuccess({ message: `Record ${modalMode === "add" ? "added" : "updated"} successfully!`});
                setFormData({});
            }
        } catch (error) {
            notifyError({ message: error});
        }
    };

    useEffect(() => {
        setIsValid(false);
        return () => setIsValid(false);
    }, [modalMode]);

    useEffect(() => {
        setIsValid(false);
        validateForm();
    }, [formData, validateForm]);

    useEffect(() => {
        if (modalMode === "edit")
            setFormData(record);
        else
            setFormData({});

        return () => setFormData({});
    }, [modalMode, record]);

    return (
        <BaseModal
            show={show}
            onOk={handleSubmit}
            onClose={onClose}
            title={`${modalMode === "add" ? "Add" : "Edit"} ${displayName}`}
            okLegend={modalMode === "add" ? "Add" : "Save"}
            disableOk={!isValid}
        >
            <div className="flex flex-col gap-2">
                {
                    properties.map((property, index) => (
                        <div key={`${componentId}-modal-property-${index}`} className="flex flex-col">
                            {
                                property.type === "select"
                                    ? (
                                        <IndexedSelect
                                            endpoint={property.option.name} label={property.display} optionLabel={property.option.label} optionValue={property.option.value}
                                            defaultValue={formData[property.option.value]}
                                            onChange={({ option }) => handleSelectChange({ optionLabel: property.option.label, optionValue: property.option.value, option })}
                                        />
                                    )
                                    : (
                                        <Input
                                            type={property.type}
                                            name={property.name}
                                            placeholder={property.display}
                                            value={formData[property.name] || ""}
                                            min={property.min}
                                            max={property.max}
                                            required={property.required}
                                            onChange={handleInputChange}
                                        />
                                    )
                            }
                        </div>
                    ))
                }
            </div>
        </BaseModal>
    );

}