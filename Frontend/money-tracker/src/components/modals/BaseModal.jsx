import { Button, PlainButton } from '../common/Button';
import './BaseModal.css';

export function BaseModal({ show, onOk, onClose, title = 'Title', children = 'Content', okLegend = 'Ok', closeLegend = 'Close' }) {
    if (!show) return null;

    const handleOk = () => {
        if (onOk) onOk()
    }

    const handleClose = () => {
        if (onClose) onClose()
    }

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <div className="modal-title">
                    <h3 className='text-lg text-primary font-bold'>{title}</h3>
                </div>

                <div className="modal-body">
                    {children}
                </div>

                <div className="modal-actions">
                    <PlainButton onClick={handleClose}>{closeLegend}</PlainButton>
                    <Button onClick={handleOk}>{okLegend}</Button>
                </div>
            </div>
        </div>
    );
};