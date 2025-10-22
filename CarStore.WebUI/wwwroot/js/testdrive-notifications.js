// Test Drive Notifications Handler
const TestDriveNotifications = {
    connection: null,

    init: async function() {
        // Initialize test drive hub connection
        this.connection = SignalRManager.initConnection('testDriveHub', '/hubs/testdrives');

        // Set up event handlers
        this.connection.on('ReceiveTestDriveNotification', (notification) => {
            this.handleNewTestDrive(notification);
        });

        this.connection.on('ReceiveTestDriveConfirmation', (notification) => {
            this.handleTestDriveConfirmation(notification);
        });

        this.connection.on('ReceiveTestDriveUpdate', (notification) => {
            this.handleTestDriveUpdate(notification);
        });

        // Start the connection
        await SignalRManager.startConnection('testDriveHub');
    },

    handleNewTestDrive: function(notification) {
        console.log('New test drive received:', notification);

        this.showNotification(
            'New Test Drive',
            notification.message,
            'info'
        );

        // Update admin dashboard if on admin page
        if (window.location.pathname.includes('/Admin/TestDrives')) {
            this.addTestDriveToList(notification);
        }

        this.updateNotificationBadge();
    },

    handleTestDriveConfirmation: function(notification) {
        console.log('Test drive confirmation received:', notification);

        this.showNotification(
            'Test Drive Scheduled',
            `Your test drive for ${notification.carBrand} ${notification.carModel} has been scheduled!`,
            'success'
        );
    },

    handleTestDriveUpdate: function(notification) {
        console.log('Test drive update received:', notification);

        this.showNotification(
            'Test Drive Update',
            notification.message,
            'info'
        );

        // Update test drive in the list if on test drives page
        if (window.location.pathname.includes('/TestDrives')) {
            this.updateTestDriveInList(notification);
        }
    },

    addTestDriveToList: function(notification) {
        // This will be implemented in the specific page script
        if (typeof window.addTestDriveToTable === 'function') {
            window.addTestDriveToTable(notification);
        }
    },

    updateTestDriveInList: function(notification) {
        const testDriveRow = document.querySelector(`tr[data-testdrive-id="${notification.testDriveId}"]`);
        if (testDriveRow) {
            const statusCell = testDriveRow.querySelector('.testdrive-status');
            if (statusCell) {
                statusCell.textContent = notification.status;
                statusCell.className = `testdrive-status badge bg-${this.getStatusColor(notification.status)}`;
            }
        }
    },

    getStatusColor: function(status) {
        const statusColors = {
            'Scheduled': 'info',
            'Confirmed': 'primary',
            'Completed': 'success',
            'Cancelled': 'danger',
            'No-show': 'warning'
        };
        return statusColors[status] || 'secondary';
    },

    showNotification: function(title, message, type) {
        const toastHtml = `
            <div class="toast align-items-center text-white bg-${type === 'success' ? 'success' : type === 'info' ? 'info' : 'warning'} border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        <strong>${title}</strong><br>${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        `;

        const toastContainer = document.getElementById('toast-container');
        if (toastContainer) {
            toastContainer.insertAdjacentHTML('beforeend', toastHtml);
            const toastElement = toastContainer.lastElementChild;
            const toast = new bootstrap.Toast(toastElement, { autohide: true, delay: 5000 });
            toast.show();

            toastElement.addEventListener('hidden.bs.toast', () => {
                toastElement.remove();
            });
        }
    },

    updateNotificationBadge: function() {
        const badge = document.getElementById('notification-badge');
        if (badge) {
            const currentCount = parseInt(badge.textContent) || 0;
            badge.textContent = currentCount + 1;
            badge.style.display = 'inline-block';
        }
    }
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    TestDriveNotifications.init();
});
