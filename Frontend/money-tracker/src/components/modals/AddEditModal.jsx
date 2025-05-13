import { useEffect, useId, useState } from "react";
import { api } from "../../boot/axios";
import { BaseModal } from "./BaseModal";
import { IndexedSelect } from "../common/IndexedSelect";

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

    const handleSubmit = async () => {
        const method = modalMode === "add" ? "post" : "put";

        try {
            const { data } = await api[method](`/${endpoint}`, formData);
            if (data.status) {
                onOk({ ...formData, [indexKey]: data.response[indexKey] });
                setFormData({});
            }
        } catch (error) {
            console.error("Error submitting form:", error);
        }
    };

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
                                        <input
                                            className="p-2 border border-gray-300 rounded-lg"
                                            type={property.type}
                                            name={property.name}
                                            placeholder={property.display}
                                            value={formData[property.name] || ""}
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