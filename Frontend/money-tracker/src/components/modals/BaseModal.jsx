import { useCallback, useEffect } from 'react';
import { Button, PlainButton } from '../common/Button';

/**
 * BaseModal Component
 * @param {Object} props - Component props
 * @param {boolean} props.show - Controls the visibility of the modal
 * @param {function} props.onOk - Callback for the Ok button
 * @param {function} props.onClose - Callback for the Close button
 * @param {string} props.title - Title of the modal
 * @param {React.ReactNode} props.children - Content of the modal
 * @param {string} props.okLegend - Label for the Ok button
 * @param {string} props.closeLegend - Label for the Close button
 * @returns {JSX.Element|null} Modal component or null if not visible
 */
export function BaseModal({
    show,
    onOk,
    onClose,
    title = 'Title',
    children = 'Content',
    okLegend = 'Ok',
    closeLegend = 'Close'
}) {
    const handleOk = useCallback(() => {
        if (onOk) onOk();
    }, [onOk]);

    const handleClose = useCallback(() => {
        if (onClose) onClose();
    }, [onClose]);

    useEffect(() => {
        const handleKeyDown = (event) => {
            if (event.key === 'Escape') {
                event.preventDefault();
                handleClose();
            }
        };
        document.addEventListener('keydown', handleKeyDown);

        return () => document.removeEventListener('keydown', handleKeyDown);
    }, [handleClose]);

    if (!show) return null;

    return (
        <div
            className="modal-overlay flex items-center justify-center fixed inset-0 bg-black/50 z-2000"
            onDoubleClick={handleClose}
            role="dialog"
            aria-labelledby={title}
            aria-hidden={!show}
        >
            <div
                className="modal-content bg-white rounded-b-lg rounded-t-2xl relative w-[70vw]"
                onClick={(e) => e.stopPropagation()}
            >
                <div className="modal-title bg-primary px-6 py-4 mb-4 rounded-t-lg">
                    <h3 id="modal-title" className="text-lg text-white font-bold">
                        {title}
                    </h3>
                </div>

                <div className="modal-body px-6 mb-4">
                    {children}
                </div>

                <div className="modal-actions flex justify-end px-6 pb-6 space-x-2">
                    <PlainButton onClick={handleClose} className='w-full'>
                        {closeLegend}
                    </PlainButton>
                    <Button onClick={handleOk} className='w-full'>
                        {okLegend}
                    </Button>
                </div>
            </div>
        </div>
    );
}