import { useId, useState } from "react";
import { IconButton } from "./Button";

/**
 * Pagination Component
 * @param {Object} props - Component props
 * @param {number} props.noRecords - Total number of records
 * @param {number} [props.defaultItemsPerPage=10] - Default number of items per page
 * @param {Array<number>} [props.pageSizes=[5, 10, 20, 50]] - Array of page sizes
 * @param {boolean} [props.disabled=false] - Disable pagination controls
 * @param {function} [props.onNext] - Callback for next page
 * @param {function} [props.onPrevious] - Callback for previous page
 * 
 * * @example
 *    <Pagination
 *        noRecords={noRecords}
 *        disabled={disablePagination}
 *        defaultItemsPerPage={defaultItemsPerPage}
 *        onPrevious={getRecordsPerPage} onNext={getRecordsPerPage}
 *    />
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
        if (disabled || currentPage === 1) return;

        setCurrentPage(currentPage - 1);
        onPrevious({
            page: currentPage - 1,
            itemsPerPage
        });
    };

    const handleNext = () => {
        if (disabled || currentPage === totalPages) return;

        setCurrentPage(currentPage + 1);
        onNext({
            page: currentPage + 1,
            itemsPerPage
        });
    };

    const handleFirst = () => {
        if (disabled || currentPage === 1) return;

        setCurrentPage(1);
        onPrevious({
            page: 1,
            itemsPerPage
        });
    }

    const handleLast = () => {
        if (disabled || currentPage === totalPages) return;

        setCurrentPage(totalPages);
        onNext({
            page: totalPages,
            itemsPerPage
        });
    }

    const handlePageSizeChange = (e) => {
        if (disabled) return;

        const newSize = parseInt(e.target.value);
        setItemsPerPage(newSize);
        setCurrentPage(1);

        onNext({
            page: 1,
            itemsPerPage: newSize
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
                                onChange={handlePageSizeChange}
                                disabled={disabled}
                            >
                                {
                                    pageSizes.map((size, index) => (
                                        <option key={`${componentId}-option-${index}`} value={size}>
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
                <IconButton icon="keyboard_double_arrow_left" fontSize="0.8rem" onClick={handleFirst} disabled={disabled || currentPage === 1} />
                <IconButton icon="chevron_left" fontSize="0.8rem" onClick={handlePrevious} disabled={disabled || currentPage === 1} />

                <span className="text-sm">
                    {currentPage} / {totalPages}
                </span>

                <IconButton icon="chevron_right" fontSize="0.8rem" onClick={handleNext} disabled={disabled || currentPage === totalPages} />
                <IconButton icon="keyboard_double_arrow_right" fontSize="0.8rem" onClick={handleLast} disabled={disabled || currentPage === totalPages} />
            </div>
        </div>
    );
}