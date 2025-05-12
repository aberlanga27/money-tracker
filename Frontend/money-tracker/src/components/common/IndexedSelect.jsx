import { api } from "../../boot/axios";
import { notifyError } from '../../utils/notify';
import { useCallback, useEffect, useId, useState } from "react";

/**
 * @component
 * Custom select component that fetches options from an API endpoint.
 * 
 * @param {string} endpoint - The endpoint to fetch the options from.
 * @param {string} label - The label to display for the select input.
 * @param {string} optionValue - The key to use as the value for the options.
 * @param {string} optionLabel - The key to use as the label for the options.
 * @param {string} customOptionLabel - A function to customize the label of the options.
 * @param {string} defaultValue - The default value to set for the select input.
 * @param {number} debounce - The debounce time in milliseconds for the search input.
 * @param {string} width - The width of the select input.
 * @param {string} minWidth - The minimum width of the select input.
 * @param {function} onChange - A callback function to call when the selected option changes.
 * @param {function} onClear - A callback function to call when the clear button is clicked.
 * 
 * @example
    <IndexedSelect
        endpoint={'Bank'} label={'Banks'} optionLabel={'bankName'} optionValue={'bankId'}
        defaultValue={1}
        onChange={({ value, label, option }) => {}}
        onClear={() => {}}
    />
*/
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
    onClear = () => { }
}) {
    const uniqueId = useId();
    const componentId = `indexed-select-${uniqueId}`;

    const [selectedOption, setSelectedOption] = useState(null);

    const [options, setOptions] = useState([]);
    const [fallbackOptions, setFallbackOptions] = useState([]);
    const [loading, setLoading] = useState(false);

    const [search, setSearch] = useState("");
    const [displaySearch, setDisplaySearch] = useState("");

    // ...

    const handleSelectedOption = (option) => {
        setDisplaySearch(option[optionLabel]);
        setSelectedOption(option[optionValue]);

        if (defaultValue === option[optionValue]) return;

        onChange({
            value: option[optionValue],
            label: option[optionLabel],
            option
        });
    };

    const handelClearSelected = () => {
        setSearch("");
        setDisplaySearch("");
        setSelectedOption(null);

        setOptions(fallbackOptions);
        onClear();
    };

    // ...

    const fetchOptions = () => {
        setLoading(true);

        api.get(`/${endpoint}`)
            .then(({ data }) => {
                if (!data.status) {
                    notifyError({ message: data.message });
                    return;
                }

                setOptions(data.response);
                setFallbackOptions(data.response);
            })
            .catch((error) => notifyError({ message: error }))
            .finally(() => setLoading(false));
    }

    const fetchDefaultValue = () => {
        if (!defaultValue) {
            setDisplaySearch("");
            setSearch("");
            setSelectedOption(null);
            return;
        };

        if (fallbackOptions.some(option => option[optionValue] === defaultValue)) return;

        api.get(`/${endpoint}/${defaultValue}`)
            .then(({ data }) => {
                if (!data.status) {
                    notifyError({ message: data.message });
                    return;
                }

                setOptions([data.response, ...options]);
                setFallbackOptions([data.response, ...fallbackOptions]);
                handleSelectedOption(data.response);
            })
            .catch((error) => notifyError({ message: error }))
    }

    const searchOptionsOnDatabase = useCallback(() => {
        const needle = search?.trim().toLowerCase();

        if (!needle || needle.length < 3) {
            setOptions(fallbackOptions);
            return;
        }

        setLoading(true);

        api.get(`/${endpoint}/Search`, { params: { search: needle } })
            .then(({ data }) => {
                setOptions(data);
            })
            .catch((error) => notifyError({ message: error }))
            .finally(() => setLoading(false));
    }, [endpoint, search, fallbackOptions]);

    const focusOnDropdown = () => {
        const optionsContainer = document.querySelector(`#${componentId} .selectable-options`);
        optionsContainer.classList.remove("hidden");

        const optionsArrow = document.querySelector(`#${componentId} .selectable-options-arrow`);
        optionsArrow.classList.add("rotate-180");
    }

    const blurOnDropdown = () => {
        const optionsContainer = document.querySelector(`#${componentId} .selectable-options`);
        optionsContainer.classList.add("hidden");

        const optionsArrow = document.querySelector(`#${componentId} .selectable-options-arrow`);
        optionsArrow.classList.remove("rotate-180");
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

        return () => {
            setOptions([]);
            setFallbackOptions([]);
            setSelectedOption(null);
            setLoading(false);
            setSearch("");
            setDisplaySearch("");
        }
    }, [])

    return (
        <div
            id={componentId}
            className="selectable-options-container flex flex-col gap-2 relative"
            style={{ width, minWidth }}
            onFocus={focusOnDropdown}
            onBlur={blurOnDropdown}
        >
            <input
                type="text"
                placeholder={`Select ${label}`}
                className="p-2 border border-gray-300 rounded-lg"
                value={displaySearch}
                disabled={loading}
                onClick={focusOnDropdown}
                onChange={(e) => {
                    setSearch(e.target.value);
                    setDisplaySearch(e.target.value);
                }}
            />

            <div className="cursor-pointer absolute text-gray-500 right-2 top-2">
                {
                    selectedOption
                        ? (<span className="material-icons selectable-options-close text-gray-300 hover:text-negative transition-all duration-300" onClick={handelClearSelected}>close</span>)
                        : null
                }
                <span className="material-icons selectable-options-arrow transition-all duration-300 transform">arrow_drop_down</span>
            </div>

            <div className="selectable-options hidden bg-white w-[100%] max-h-[200px] mt-1 border-1 border-gray-300 rounded-md z-1000 overflow-y-auto absolute top-full left-0" onMouseDown={(e) => e.preventDefault()}>
                {
                    options?.length > 0
                        ? options.map((option) => (
                            <div
                                key={`${componentId}-option-${option[optionValue]}`}
                                id={`option-${option[optionValue]}`}
                                className={`option p-3 cursor-pointer transition-all duration-300 ${selectedOption === option[optionValue] ? "bg-primary text-white" : "hover:bg-primary/20"}`}
                                onClick={() => {
                                    handleSelectedOption(option);
                                    blurOnDropdown();
                                }}
                            >
                                {customOptionLabel ? customOptionLabel(option) : option[optionLabel]}
                            </div>
                        ))
                        : <div className="no-options p-3 text-center text-secondary">No options found</div>
                }
            </div>
        </div>
    )
}