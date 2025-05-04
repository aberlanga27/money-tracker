import { useCallback, useEffect, useId, useState } from "react";
import { api } from "../../boot/axios";
import './IndexedSelect.css';

// TODOS:
//  - Add loading state
//  - Add Notify utility on errors
//  - Add color prop and use colors definided on the main palette
//  - Add documentation to component
//  - Add support to esc key to close the dropdown
//  - Add support arrows and enter keys to navigate the dropdown

export function IndexedSelect({
    endpoint,
    label,
    optionValue,
    optionLabel,
    customOptionLabel = null,
    defaultValue,
    debounce = 500,
    width = "100%",
    minWidth = "auto",
    onChange = () => { },
    onClear = () => { },
}) {
    const uniqueId = useId();

    const [selectedOption, setSelectedOption] = useState(null);

    const [options, setOptions] = useState([]);
    const [fallbackOptions, setFallbackOptions] = useState([]);

    const [search, setSearch] = useState("");
    const [displaySearch, setDisplaySearch] = useState("");

    // ...

    const fetchOptions = async () => {
        await api.get(`/${endpoint}`)
            .then(({ data }) => {
                if (!data.status) {
                    console.error("Error fetching options:", data.message);
                    return;
                }

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
                if (!data.status) {
                    console.error("Error fetching default value:", data.message);
                    return;
                }

                setOptions([data.response, ...options]);
                setFallbackOptions([data.response, ...fallbackOptions]);
                handleSelectedOption(data.response);
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

    const focusOnDropdown = () => {
        const optionsContainer = document.querySelector(`#${uniqueId} .selectable-options`);
        optionsContainer.classList.add("visible");

        const optionsArrow = document.querySelector(`#${uniqueId} .selectable-options-arrow`);
        optionsArrow.classList.add("rotate");
    }

    const blurOnDropdown = () => {
        const optionsContainer = document.querySelector(`#${uniqueId} .selectable-options`);
        optionsContainer.classList.remove("visible");

        const optionsArrow = document.querySelector(`#${uniqueId} .selectable-options-arrow`);
        optionsArrow.classList.remove("rotate");
    }

    const handelClearSelected = () => {
        setDisplaySearch("");
        setSelectedOption(null);

        setOptions(fallbackOptions);
        onClear();
    }

    const handleSelectedOption = (option) => {
        setDisplaySearch(option[optionLabel]);
        setSelectedOption(option[optionValue]);

        onChange({
            value: option[optionValue],
            label: option[optionLabel],
            option
        });
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
            id={uniqueId}
            className="flex flex-col gap-2 selectable-options-container"
            style={{ width, minWidth }}
            onFocus={focusOnDropdown}
            onBlur={blurOnDropdown}
        >
            <input
                type="text"
                placeholder={`Search ${label}`}
                className="p-2 border border-gray-300 rounded-lg"
                value={displaySearch}
                onClick={focusOnDropdown}
                onChange={(e) => {
                    setSearch(e.target.value);
                    setDisplaySearch(e.target.value);
                }}
            />

            <div className="cursor-pointer absolute text-gray-500 right-2 top-2">
                {
                    selectedOption
                        ? (<span className="material-icons selectable-options-close text-gray-300" onClick={handelClearSelected}>close</span>)
                        : ""
                }
                <span className="material-icons selectable-options-arrow">arrow_drop_down</span>
            </div>

            <div className="selectable-options" onMouseDown={(e) => e.preventDefault()}>
                {
                    options?.length > 0
                        ? options.map((option) => (
                            <div
                                key={option[optionValue]}
                                id={`option-${option[optionValue]}`}
                                className={`option ${selectedOption === option[optionValue] ? "selected" : ""}`}
                                onClick={() => {
                                    handleSelectedOption(option);
                                    blurOnDropdown();
                                }}
                            >
                                {customOptionLabel ? customOptionLabel(option) : option[optionLabel]}
                            </div>
                        ))
                        : <div className="no-options">No options found</div>
                }
            </div>
        </div>
    )
}