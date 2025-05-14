import { useId } from "react";

/**
 * Input Component
 * @param {Object} props - Component props
 * @param {React.ReactNode} [props.children=null] - Child elements (e.g., icons)
 * @param {string} [props.type='text'] - Input type (e.g., text, password)
 * @param {string} [props.name=null] - Input name
 * @param {string} [props.placeholder=null] - Placeholder text
 * @param {string} [props.value=null] - Input value
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
    childrenPosition = 'prepend',
    onChange = () => { }
}) {
    const uniqueId = useId();
    const componentId = `input-${uniqueId}`;

    return (
        <div id={componentId} className="border border-gray-300 rounded-lg flex items-center">
            { childrenPosition === 'prepend' && (children) }

            <input
                type={type}
                name={name}
                placeholder={placeholder}
                value={value}
                onChange={onChange}
                className="p-2 focus:outline-none w-full"
            />

            { childrenPosition === 'append' && (children) }            
        </div>
    )
}