import { useId, useState } from "react";

/**
 * Input Component
 * @param {Object} props - Component props
 * @param {React.ReactNode} [props.children=null] - Child elements (e.g., icons)
 * @param {string} [props.type='text'] - Input type (e.g., text, password)
 * @param {string} [props.name=null] - Input name
 * @param {string} [props.placeholder=null] - Placeholder text
 * @param {string} [props.value=null] - Input value
 * @param {number} [props.min=null] - Minimum value (for number inputs)
 * @param {number} [props.max=null] - Maximum value (for number inputs)
 * @param {boolean} [props.required=false] - Whether the input is required
 * @param {string} [props.childrenPosition='prepend'] - Position of child elements (prepend or append)
 * @param {function} [props.onChange=() => {}] - Callback for input change
 * 
 * @example
 *    <Input
 *        type="text"
 *        placeholder="Search"
 *        value={search}
 *        onChange={(e) => setSearch(e.target.value)}
 *    >
 *        <span className="material-icons pl-2 pr-0 text-gray-500" style={{ fontSize: '1.2rem' }}>
 *            search
 *        </span>
 *    </Input>
 */
export function Input({
    children,
    type = 'text',
    name = null,
    placeholder = null,
    value = null,
    min = null,
    max = null,
    required = false,
    childrenPosition = 'prepend',
    onChange = () => { }
}) {
    const uniqueId = useId();
    const componentId = `input-${uniqueId}`;

    const [valid, setValid] = useState(true);
    const [validations, setValidations] = useState([]);

    const handleInputChange = (e) => {
        const { value } = e.target;
        
        switch (type) {
            case 'number':
                setValid(!isNaN(value) && (min ? value >= min : true) && (max ? value <= max : true));
                setValidations(() => {
                    const newValidations = []
                    if (isNaN(value)) newValidations.push('This field must be a number');
                    if (min && value < min) newValidations.push(`The minimum value is ${min}`);
                    if (max && value > max) newValidations.push(`The maximum value is ${max}`);
                    return newValidations;
                })
                break;
            case 'text':
                setValid((min ? value.length >= min : true) && (max ? value.length <= max : true));
                setValidations(() => {
                    const newValidations = []
                    if (min && value.length < min) newValidations.push(`At least ${min} character${value > 1 ? 's are' : ' is'} required`);
                    if (max && value.length > max) newValidations.push(`Maximum ${max} character${value > 1 ? 's are' : ' is'} allowed`);
                    return newValidations;
                })
                break;
            default:
                setValid(true);
        }

        onChange(e);
    }

    return (
        <div id={componentId}>
            <div className={`border rounded-lg flex items-center ${valid ? 'border-gray-300' : 'border-negative'}`}>
                {childrenPosition === 'prepend' && (children)}

                <input
                    type={type}
                    name={name}
                    placeholder={placeholder}
                    value={value}
                    min={min}
                    max={max}
                    required={required}
                    onChange={handleInputChange}
                    className={`p-2 focus:outline-none w-full placeholder:text-gray-400 ${valid ? '' : 'text-negative'}`}
                />

                {childrenPosition === 'append' && (children)}
            </div>

            <ul className="validations text-xs pl-1">
                {
                    validations.map((validation, index) => (
                        <li key={`${componentId}-validation-${index}`} className="text-negative">
                            {validation}
                        </li>
                    ))
                }
            </ul>
        </div>
    )
}