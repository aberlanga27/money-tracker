import { useCallback, useEffect, useState } from "react";
import { api } from "../../boot/axios";
import './IndexedSelect.css';

export function IndexedSelect({
    endpoint,
    label,
    optionValue,
    optionLabel,
    customOptionLabel,
    defaultValue,
    debounce = 500,
    width = "100%",
    minWidth = "auto",
    onChange = () => { },
    onClear = () => { },
}) {
    const [selectedOption, setSelectedOption] = useState(null);

    const [options, setOptions] = useState([]);
    const [fallbackOptions, setFallbackOptions] = useState([]);

    const [search, setSearch] = useState("");

    // ...

    const fetchOptions = async () => {
        await api.get(`/${endpoint}`)
            .then(({ data }) => {
                setOptions(data.response);
                setFallbackOptions(data.response);
            })
            .catch((error) => console.error("Error fetching options:", error));
    }

    const fetchDefaultValue = async () => {
        if (!defaultValue) return;
        if (fallbackOptions.some(option => option[optionValue] === defaultValue)) return;

        await api.get(`/${endpoint}/${defaultValue}`)
            .then(({ data }) => {
                setOptions([data.response, ...options]);
                setFallbackOptions([data.response, ...fallbackOptions]);
                setSelectedOption(data.response[optionValue]);
                setSearch(data.response[optionLabel]);
            })
            .catch((error) => console.error("Error fetching default value:", error));
    }

    const searchOptionsOnDatabase = useCallback(async () => {
        const needle = search?.trim().toLowerCase();

        if (!needle || needle.length < 3) {
            setOptions(fallbackOptions);
            return;
        }

        await api.get(`/${endpoint}/Search`, { params: { search: needle } })
            .then(({ data }) => {
                setOptions(data);
            })
            .catch((error) => console.error("Error searching options:", error));
    }, [endpoint, search, fallbackOptions]);

    const handleFocusOnInput = () => {
        const optionsContainer = document.querySelector(".selectable-options");
        optionsContainer.classList.add("visible");

        const optionsArrow = document.querySelector(".selectable-options-arrow");
        optionsArrow.classList.add("rotate");
    }

    const handleBlurOnInput = () => {
        const optionsContainer = document.querySelector(".selectable-options");
        optionsContainer.classList.remove("visible");

        const optionsArrow = document.querySelector(".selectable-options-arrow");
        optionsArrow.classList.remove("rotate");
    }

    const handelClearSelected = () => {
        setSearch("");
        setSelectedOption(null);
        setOptions(fallbackOptions);

        onClear();
    }

    const handleSelectedOption = (option) => {
        setSearch(option[optionLabel]);
        setSelectedOption(option[optionValue]);

        onChange(option[optionValue]);
    }

    // ...

    useEffect(() => {
        fetchDefaultValue();
    }, [defaultValue]);

    useEffect(() => {
        const handler = setTimeout(() => {
            searchOptionsOnDatabase();
        }, debounce);

        return () => clearTimeout(handler);
    }, [search, debounce, searchOptionsOnDatabase]);

    useEffect(() => {
        fetchOptions();
        fetchDefaultValue();
    }, [])

    return (
        <div
            className="flex flex-col gap-2 selectable-options-container"
            style={{ width, minWidth }}
            onFocus={handleFocusOnInput}
        // onBlur={handleBlurOnInput}
        >
            <input
                type="text"
                placeholder={`Search ${label}`}
                className="p-2 border border-gray-300 rounded-lg"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
            />

            <div className="cursor-pointer absolute text-gray-500 right-2 top-2">
                {
                    selectedOption
                        ? (<span className="material-icons selectable-options-close text-gray-300" onClick={handelClearSelected}>close</span>)
                        : ""
                }
                <span className="material-icons selectable-options-arrow">arrow_drop_down</span>
            </div>

            <div className="selectable-options">
                {
                    options?.map((option) => (
                        <div
                            key={option[optionValue]}
                            id={`option-${option[optionValue]}`}
                            className={`option ${selectedOption === option[optionValue] ? "selected" : ""}`}
                            onClick={() => handleSelectedOption(option)}
                        >
                            {customOptionLabel ? customOptionLabel(option) : option[optionLabel]}
                        </div>
                    ))
                }
            </div>
        </div>
    )
}