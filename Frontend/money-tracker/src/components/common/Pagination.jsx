import { useState } from "react";
import { Button } from "./Button";

export function Pagination({
    noRecords,
    itemsPerPage = 10,
    disabled = false,
    onNext = () => { },
    onPrevious = () => { }
}) {
    const [currentPage, setCurrentPage] = useState(1);

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
        <div className="pagination-controls flex justify-end items-center gap-2">
            <Button onClick={handleFirst} disabled={disabled || currentPage === 1}>
                <span className="material-icons" style={{ fontSize: "0.8rem" }}>keyboard_double_arrow_left</span>
            </Button>
            <Button onClick={handlePrevious} disabled={disabled || currentPage === 1}>
                <span className="material-icons" style={{ fontSize: "0.8rem" }}>chevron_left</span>
            </Button>

            <span className="text-sm">
                Page {currentPage} of {totalPages}
            </span>

            <Button onClick={handleNext} disabled={disabled || currentPage === totalPages}>
                <span className="material-icons" style={{ fontSize: "0.8rem" }}>chevron_right</span>
            </Button>
            <Button onClick={handleLast} disabled={disabled || currentPage === totalPages}>
                <span className="material-icons" style={{ fontSize: "0.8rem" }}>keyboard_double_arrow_right</span>
            </Button>
        </div>
    );
}