import { useEffect, useState } from "react";
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
    const [formData, setFormData] = useState({});

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({ ...prevData, [name]: value }));
    };

    const handleSelectChange = (name, value) => {
        setFormData((prevData) => ({ ...prevData, [name]: value }));
    };

    const handleSubmit = async () => {
        try {
            const response = await api.post(`/${endpoint}`, formData);
            if (response.status === 200) {
                onOk(response.data);
            }
        } catch (error) {
            console.error("Error submitting form:", error);
        }
    };

    useEffect(() => {
        if (modalMode === "edit") {
            setFormData(record);
        }

        return () => {
            setFormData({});
        }
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
                        <div key={index} className="flex flex-col">
                            {
                                property.type === "select"
                                ? (
                                    <IndexedSelect
                                        endpoint={property.option.name} label={property.display} optionLabel={property.option.label} optionValue={property.option.value}
                                        defaultValue={formData[property.name] || null}
                                        onChange={({ value }) => handleSelectChange(property.name, value)}
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