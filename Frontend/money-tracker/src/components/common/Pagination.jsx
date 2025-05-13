import { useId, useState } from "react";
import { Button } from "./Button";

/**
 * Pagination Component
 * @param {Object} props - Component props
 * @param {number} props.noRecords - Total number of records
 * @param {number} [props.defaultItemsPerPage=10] - Default number of items per page
 * @param {Array<number>} [props.pageSizes=[5, 10, 20, 50]] - Array of page sizes
 * @param {boolean} [props.disabled=false] - Disable pagination controls
 * @param {function} [props.onNext] - Callback for next page
 * @param {function} [props.onPrevious] - Callback for previous page
 * @returns {JSX.Element} Pagination component
 */
export function Pagination({
    noRecords,
    defaultItemsPerPage = 10,
    pageSizes = [5, 10, 20, 50],
    disabled = false,
    onNext = () => { },
    onPrevious = () => { }
}) {
    const uniqueId = useId();
    const componentId = `pagination-${uniqueId}`;

    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(defaultItemsPerPage);

    const totalPages = Math.ceil(noRecords / itemsPerPage);

    const handlePrevious = () => {
        if (currentPage > 1) {
            setCurrentPage(currentPage - 1);
            onPrevious({
                page: currentPage - 1,
                itemsPerPage
            });
        }
    };

    const handleNext = () => {
        if (currentPage < totalPages) {
            setCurrentPage(currentPage + 1);
            onNext({
                page: currentPage + 1,
                itemsPerPage
            });
        }
    };

    const handleFirst = () => {
        setCurrentPage(1);
        onPrevious({
            page: 1,
            itemsPerPage
        });
    }

    const handleLast = () => {
        setCurrentPage(totalPages);
        onNext({
            page: totalPages,
            itemsPerPage
        });
    }

    return (
        <div id={componentId} className="pagination-wrapper flex justify-end items-center gap-2">
            <div className="pagination-records-per-page flex items-center gap-1">
                <div className="ecords-per-page-title text-xs font-bold">
                    Records per page
                </div>

                <div className="records-per-page-selector">
                    {
                        pageSizes.length > 0 && (
                            <select
                                className="px-1 py-0.5 border border-gray-300 rounded-lg"
                                value={itemsPerPage}
                                onChange={(e) => {
                                    if (disabled) return;
                                    setItemsPerPage(parseInt(e.target.value));
                                    setCurrentPage(1);
                                    onNext({
                                        page: 1,
                                        itemsPerPage: parseInt(e.target.value)
                                    });
                                }}
                            >
                                {
                                    pageSizes.map((size) => (
                                        <option key={size} value={size}>
                                            {size}
                                        </option>
                                    ))
                                }
                            </select>
                        )
                    }
                </div>
            </div>

            <div className="pagination-controls flex items-center gap-1">
                <Button onClick={handleFirst} disabled={disabled || currentPage === 1}>
                    <span className="material-icons" style={{ fontSize: "0.8rem" }}>keyboard_double_arrow_left</span>
                </Button>
                <Button onClick={handlePrevious} disabled={disabled || currentPage === 1}>
                    <span className="material-icons" style={{ fontSize: "0.8rem" }}>chevron_left</span>
                </Button>

                <span className="text-sm">
                    {currentPage} / {totalPages}
                </span>

                <Button onClick={handleNext} disabled={disabled || currentPage === totalPages}>
                    <span className="material-icons" style={{ fontSize: "0.8rem" }}>chevron_right</span>
                </Button>
                <Button onClick={handleLast} disabled={disabled || currentPage === totalPages}>
                    <span className="material-icons" style={{ fontSize: "0.8rem" }}>keyboard_double_arrow_right</span>
                </Button>
            </div>
        </div>
    );
}