// Admin Dashboard Notifications Handler
const AdminDashboard = {
    connection: null,

    init: async function() {
        // Only initialize if user is admin
        const isAdminPage = window.location.pathname.includes('/Admin');
        if (!isAdminPage) {
            console.log('Not on admin page, skipping admin hub initialization');
            return;
        }

        // Initialize admin hub connection
        this.connection = SignalRManager.initConnection('adminHub', '/hubs/admin');

        // Set up event handlers for all admin notifications
        this.connection.on('ReceiveOrderNotification', (notification) => {
            this.handleAdminOrderNotification(notification);
        });

        this.connection.on('ReceiveOrderUpdate', (notification) => {
            this.handleAdminOrderUpdate(notification);
        });

        this.connection.on('ReceiveTestDriveNotification', (notification) => {
            this.handleAdminTestDriveNotification(notification);
        });

        this.connection.on('ReceiveTestDriveUpdate', (notification) => {
            this.handleAdminTestDriveUpdate(notification);
        });

        // Start the connection
        await SignalRManager.startConnection('adminHub');
    },

    handleAdminOrderNotification: function(notification) {
        console.log('Admin: New order notification:', notification);

        this.showAdminNotification(
            'New Order',
            `${notification.userName} placed an order for ${notification.carBrand} ${notification.carModel}`,
            'primary'
        );

        this.updateAdminBadge('orders');

        // Play notification sound (optional)
        this.playNotificationSound();
    },

    handleAdminOrderUpdate: function(notification) {
        console.log('Admin: Order update:', notification);

        this.showAdminNotification(
            'Order Updated',
            notification.message,
            'info'
        );
    },

    handleAdminTestDriveNotification: function(notification) {
        console.log('Admin: New test drive notification:', notification);

        this.showAdminNotification(
            'New Test Drive',
            `${notification.userName} scheduled a test drive for ${notification.carBrand} ${notification.carModel}`,
            'primary'
        );

        this.updateAdminBadge('testdrives');

        // Play notification sound (optional)
        this.playNotificationSound();
    },

    handleAdminTestDriveUpdate: function(notification) {
        console.log('Admin: Test drive update:', notification);

        this.showAdminNotification(
            'Test Drive Updated',
            notification.message,
            'info'
        );
    },

    showAdminNotification: function(title, message, type) {
        // Create prominent admin notification
        const toastHtml = `
            <div class="toast align-items-center text-white bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        <strong><i class="bi bi-bell-fill"></i> ${title}</strong><br>${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        `;

        const toastContainer = document.getElementById('toast-container');
        if (toastContainer) {
            toastContainer.insertAdjacentHTML('beforeend', toastHtml);
            const toastElement = toastContainer.lastElementChild;
            const toast = new bootstrap.Toast(toastElement, { autohide: true, delay: 7000 });
            toast.show();

            toastElement.addEventListener('hidden.bs.toast', () => {
                toastElement.remove();
            });
        }

        // Also add to admin notification panel if it exists
        this.addToNotificationPanel(title, message, type);
    },

    addToNotificationPanel: function(title, message, type) {
        const panel = document.getElementById('admin-notification-panel');
        if (panel) {
            const timestamp = new Date().toLocaleTimeString();
            const notificationHtml = `
                <div class="notification-item alert alert-${type} alert-dismissible fade show" role="alert">
                    <strong>${title}</strong> - ${timestamp}<br>
                    <small>${message}</small>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            `;
            panel.insertAdjacentHTML('afterbegin', notificationHtml);

            // Keep only last 10 notifications
            const notifications = panel.querySelectorAll('.notification-item');
            if (notifications.length > 10) {
                notifications[notifications.length - 1].remove();
            }
        }
    },

    updateAdminBadge: function(type) {
        const badge = document.getElementById(`admin-${type}-badge`);
        if (badge) {
            const currentCount = parseInt(badge.textContent) || 0;
            badge.textContent = currentCount + 1;
            badge.style.display = 'inline-block';
        }

        // Also update main admin notification badge
        const mainBadge = document.getElementById('admin-notification-badge');
        if (mainBadge) {
            const currentCount = parseInt(mainBadge.textContent) || 0;
            mainBadge.textContent = currentCount + 1;
            mainBadge.style.display = 'inline-block';
        }
    },

    playNotificationSound: function() {
        // Optional: Play a subtle notification sound
        // You can add an audio element or use Web Audio API
        try {
            const audio = new Audio('/sounds/notification.mp3');
            audio.volume = 0.3;
            audio.play().catch(e => console.log('Could not play notification sound:', e));
        } catch (error) {
            console.log('Notification sound not available');
        }
    }
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    AdminDashboard.init();
});

// Make it globally available
window.AdminDashboard = AdminDashboard;
