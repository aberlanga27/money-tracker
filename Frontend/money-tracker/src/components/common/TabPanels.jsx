import { useId, useState } from "react";

function TabPanel({ children, value, index }) {
    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`tab-panel-${index}`}
            aria-labelledby={`tab-${index}`}
        >
            {value === index && (
                <div className="p-2">
                    {children}
                </div>
            )}
        </div>
    );
}

function TabPanelOption({ children, index, active = false, handleChange = () => { } }) {
    return (
        <div
            role="tab"
            id={`tab-${index}`}
            aria-controls={`tab-panel-${index}`}
            className={`tab-option flex-1 text-center text-primary text-sm p-1 rounded-lg cursor-pointer hover:bg-primary/20 transition duration-300 ${active ? 'bg-primary/20' : ''}`}
            onClick={(event) => handleChange(event, index)}
        >
            {children}
        </div>
    )
}

export function TabPanels({ children, sections = [] }) {
    const uniqueId = useId();
    const componentId = `tab-panels-${uniqueId}`;

    const [value, setValue] = useState(0);
    const handleChange = (_, newValue) => {
        setValue(newValue);
    };

    return (
        <div className="tab-panels">
            <div className="sections-selector flex gap-1">
                {
                    sections.map((section, index) => (
                        <TabPanelOption
                            key={`${componentId}-option-${index}`}
                            index={index}
                            active={value === index}
                            handleChange={handleChange}
                        >
                            {section}
                        </TabPanelOption>
                    ))
                }
            </div>

            <div className="tabs-contnet">
                {
                    children.map((child, index) => (
                        <TabPanel key={index} value={value} index={index}>
                            {child}
                        </TabPanel>
                    ))
                }
            </div>
        </div>
    );
}