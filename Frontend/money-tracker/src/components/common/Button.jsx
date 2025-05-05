
export function Button({ children, onClick, disabled = false, className = "", type = "button" }) {
    const disabledClasses = 'disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:bg-primary';
    return (
        <button
            type={type}
            disabled={disabled}
            onClick={onClick}
            className={`bg-primary text-white p-2 shadow-lg rounded-lg hover:bg-primary/80 transition duration-300 flex items-center justify-center cursor-pointer ${className} ${disabledClasses}`}
        >
            {children}
        </button>
    );
}


export function NegativeButton({ children, onClick, disabled = false, className = "", type = "button" }) {
    const disabledClasses = 'disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:bg-negative';
    return (
        <button
            type={type}
            disabled={disabled}
            onClick={onClick}
            className={`bg-negative text-white p-2 shadow-lg rounded-lg hover:bg-negative/80 transition duration-300 flex items-center justify-center cursor-pointer ${className} ${disabledClasses}`}
        >
            {children}
        </button>
    );
}

export function PlainButton({ children, onClick, disabled = false, className = "", type = "button" }) {
    return (
        <button
            type={type}
            disabled={disabled}
            onClick={onClick}
            className={`border-primary text-primary p-2 shadow-lg rounded-lg hover:bg-primary/20 transition duration-300 flex items-center justify-center cursor-pointer ${className}`}
        >
            {children}
        </button>
    );
}