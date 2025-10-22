// Inventory Updates Handler
const InventoryUpdates = {
    connection: null,

    init: async function() {
        // Initialize inventory hub connection
        this.connection = SignalRManager.initConnection('inventoryHub', '/hubs/inventory');

        // Set up event handlers
        this.connection.on('ReceiveInventoryUpdate', (notification) => {
            this.handleInventoryUpdate(notification);
        });

        this.connection.on('ReceiveCarStockUpdate', (notification) => {
            this.handleCarStockUpdate(notification);
        });

        // Start the connection
        await SignalRManager.startConnection('inventoryHub');
    },

    handleInventoryUpdate: function(notification) {
        console.log('Inventory update received:', notification);

        // Update stock in car list if on cars page
        if (window.location.pathname.includes('/Cars')) {
            this.updateCarStock(notification);
        }

        // Show notification for significant stock changes
        if (notification.currentStock === 0) {
            this.showNotification(
                'Out of Stock',
                `${notification.brand} ${notification.model} is now out of stock!`,
                'warning'
            );
        } else if (notification.updateType === 'Restock') {
            this.showNotification(
                'Stock Updated',
                `${notification.brand} ${notification.model} has been restocked! Available: ${notification.currentStock}`,
                'success'
            );
        }
    },

    handleCarStockUpdate: function(notification) {
        console.log('Car stock update received:', notification);
        this.updateCarStock(notification);
    },

    updateCarStock: function(notification) {
        // Update stock display for specific car
        const stockElement = document.querySelector(`[data-car-id="${notification.carId}"] .car-stock`);
        if (stockElement) {
            const previousValue = parseInt(stockElement.textContent);
            stockElement.textContent = notification.currentStock;

            // Add animation class
            stockElement.classList.add('stock-updated');
            setTimeout(() => {
                stockElement.classList.remove('stock-updated');
            }, 2000);

            // Update stock status class
            if (notification.currentStock === 0) {
                stockElement.classList.remove('text-success', 'text-warning');
                stockElement.classList.add('text-danger');
            } else if (notification.currentStock < 5) {
                stockElement.classList.remove('text-success', 'text-danger');
                stockElement.classList.add('text-warning');
            } else {
                stockElement.classList.remove('text-warning', 'text-danger');
                stockElement.classList.add('text-success');
            }
        }

        // Update order button availability
        const orderButton = document.querySelector(`[data-car-id="${notification.carId}"] .order-button`);
        if (orderButton) {
            if (notification.currentStock === 0) {
                orderButton.disabled = true;
                orderButton.textContent = 'Out of Stock';
                orderButton.classList.add('btn-secondary');
                orderButton.classList.remove('btn-primary');
            } else {
                orderButton.disabled = false;
                orderButton.textContent = 'Order Now';
                orderButton.classList.add('btn-primary');
                orderButton.classList.remove('btn-secondary');
            }
        }
    },

    subscribeToCarInventory: async function(carId) {
        if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) {
            try {
                await this.connection.invoke('SubscribeToCarInventory', carId);
                console.log(`Subscribed to inventory updates for car ${carId}`);
            } catch (error) {
                console.error('Error subscribing to car inventory:', error);
            }
        }
    },

    unsubscribeFromCarInventory: async function(carId) {
        if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) {
            try {
                await this.connection.invoke('UnsubscribeFromCarInventory', carId);
                console.log(`Unsubscribed from inventory updates for car ${carId}`);
            } catch (error) {
                console.error('Error unsubscribing from car inventory:', error);
            }
        }
    },

    showNotification: function(title, message, type) {
        const toastHtml = `
            <div class="toast align-items-center text-white bg-${type === 'success' ? 'success' : type === 'warning' ? 'warning' : 'info'} border-0" role="alert" aria-live="assertive" aria-atomic="true">
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
    }
};

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    InventoryUpdates.init();
});

// Make it globally available for page-specific usage
window.InventoryUpdates = InventoryUpdates;
