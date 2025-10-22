// Order Notifications Handler
const OrderNotifications = {
    connection: null,

    init: async function() {
        // Initialize order hub connection
        this.connection = SignalRManager.initConnection('orderHub', '/hubs/orders');

        // Set up event handlers
        this.connection.on('ReceiveOrderNotification', (notification) => {
            this.handleNewOrder(notification);
        });

        this.connection.on('ReceiveOrderConfirmation', (notification) => {
            this.handleOrderConfirmation(notification);
        });

        this.connection.on('ReceiveOrderUpdate', (notification) => {
            this.handleOrderUpdate(notification);
        });

        // Start the connection
        await SignalRManager.startConnection('orderHub');
    },

    handleNewOrder: function(notification) {
        console.log('New order received:', notification);

        // Show notification
        this.showNotification(
            'New Order',
            notification.message,
            'info'
        );

        // Update admin dashboard if on admin page
        if (window.location.pathname.includes('/Admin/Orders')) {
            this.addOrderToList(notification);
        }

        // Update notification badge
        this.updateNotificationBadge();
    },

    handleOrderConfirmation: function(notification) {
        console.log('Order confirmation received:', notification);

        this.showNotification(
            'Order Confirmed',
            `Your order for ${notification.carBrand} ${notification.carModel} has been placed successfully!`,
            'success'
        );
    },

    handleOrderUpdate: function(notification) {
        console.log('Order update received:', notification);

        this.showNotification(
            'Order Update',
            notification.message,
            'info'
        );

        // Update order in the list if on orders page
        if (window.location.pathname.includes('/Orders')) {
            this.updateOrderInList(notification);
        }
    },

    addOrderToList: function(notification) {
        // This will be implemented in the specific page script
        if (typeof window.addOrderToTable === 'function') {
            window.addOrderToTable(notification);
        }
    },

    updateOrderInList: function(notification) {
        // Update existing order row
        const orderRow = document.querySelector(`tr[data-order-id="${notification.orderId}"]`);
        if (orderRow) {
            const statusCell = orderRow.querySelector('.order-status');
            if (statusCell) {
                statusCell.textContent = notification.status;
                statusCell.className = `order-status badge bg-${this.getStatusColor(notification.status)}`;
            }
        }
    },

    getStatusColor: function(status) {
        const statusColors = {
            'Pending': 'warning',
            'Confirmed': 'info',
            'Processing': 'primary',
            'Shipped': 'success',
            'Delivered': 'success',
            'Cancelled': 'danger'
        };
        return statusColors[status] || 'secondary';
    },

    showNotification: function(title, message, type) {
        // Create toast notification
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

            // Remove toast element after it's hidden
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
    OrderNotifications.init();
});
