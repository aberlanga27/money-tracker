import { Link, useLocation } from "react-router-dom";
import { useState, useEffect } from "react";

export default function Menu({ options = [] }) {
    const location = useLocation();
    const [currentRoute, setCurrentRoute] = useState(location.pathname);

    useEffect(() => {
        setCurrentRoute(location.pathname);
    }, [location.pathname]);

    const getActiveStyles = (path) => {
        const baseStyles = "flex justify-center items-center rounded-2xl transition-all duration-300 ease-in-out";

        return currentRoute === path
            ? `${baseStyles} bg-white text-primary`
            : baseStyles;
    };

    return (
        <nav>
            <ul className="flex justify-around bg-primary text-white p-1">
                {
                    options.map((option) => (
                        <li key={option.path} className="flex-1 text-center">
                            <Link to={option.path} className="text-white">
                                <div className={getActiveStyles(option.path)}>
                                    <span className="material-icons">{option.icon}</span>
                                </div>
                            </Link>
                        </li>
                    ))
                }
            </ul>
        </nav>
    );
}