import { BaseModal } from "./BaseModal";

export function ConfirmationModal({
    children,
    show = false,
    title = "",
    message = "",
    onOk = () => { },
    onClose = () => { },
}) {
    return (
        <BaseModal
            show={show}
            onOk={onOk}
            onClose={onClose}
            title={title}
            okLegend={'Delete'}
            cancelLegend={'Cancel'}
        >
            <div className="flex flex-col gap-2">
                <p>{message}</p>
                {children}
            </div>
        </BaseModal>
    );
}