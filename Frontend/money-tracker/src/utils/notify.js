const baseClasses = 'fixed z-1000 pl-4 pr-2 py-2 rounded shadow-lg transition-transform transform duration-300 ease-in-out text-right';
const positionClasses = {
    'top-right': 'top-4 right-4',
    'top-left': 'top-4 left-4',
    'bottom-right': 'bottom-4 right-4',
    'bottom-left': 'bottom-4 left-4',
}
const typeClasses = {
    info: 'bg-primary',
    success: 'bg-success',
    error: 'bg-negative',
    warning: 'bg-warning',
};

export function notify({ title = '', message = '', position = 'top-right', type = 'info', textColor = 'white', timeout = 5000 }) {
    const notification = document.createElement('div');
    notification.className = `notification ${baseClasses} ${positionClasses[position]} ${typeClasses[type]} text-${textColor}`;

    const notificationContent = document.createElement('div');
    notificationContent.className = 'notification-content';
    notificationContent.innerHTML = `
        <strong>${title}</strong>
        <p>${message}</p>
    `;

    const closeButton = document.createElement('button');
    closeButton.className = 'notification-close grid place-items-center';
    closeButton.innerHTML = '<span class="material-icons cursor-pointer opacity-60 hover:opacity-100 ease-in-out transition-all duration-300" style="font-size:1.2rem;">close</span>';

    closeButton.onclick = () => {
        notification.remove();
    }

    const notificationContainer = document.createElement('div');
    notificationContainer.className = 'notification-container';
    notificationContainer.classList.add('flex', 'justify-between', 'items-center', 'gap-2');
    notificationContainer.appendChild(notificationContent);
    notificationContainer.appendChild(closeButton);

    notification.appendChild(notificationContainer);
    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, timeout);
}

export function notifyError({ title = 'Error', message = 'Error', position = 'top-right', timeout = 5000 }) {
    notify({
        title,
        message,
        position,
        type: 'error',
        textColor: 'white',
        timeout
    });
}

export function notifyWarning({ title = 'Warning', message = 'Warning', position = 'top-right', timeout = 5000 }) {
    notify({
        title,
        message,
        position,
        type: 'warning',
        textColor: 'white',
        timeout
    });
}