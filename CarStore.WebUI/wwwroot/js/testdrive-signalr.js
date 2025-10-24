// TestDrive SignalR Client
class TestDriveSignalRClient {
    constructor() {
        this.connection = null;
        this.isConnected = false;
    }

    async start() {
        try {
            // Tạo kết nối SignalR
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/testdrivehub")
                .withAutomaticReconnect()
                .build();

            // Đăng ký các event handlers
            this.registerEventHandlers();

            // Bắt đầu kết nối
            await this.connection.start();
            this.isConnected = true;
            console.log("SignalR TestDrive Hub connected successfully");

            // Hiển thị thông báo kết nối thành công
            this.showNotification("Kết nối thành công", "Đã kết nối với hệ thống thông báo", "success");

        } catch (error) {
            console.error("Error starting SignalR connection:", error);
            this.showNotification("Lỗi kết nối", "Không thể kết nối với hệ thống thông báo", "error");
        }
    }

    registerEventHandlers() {
        // Xử lý khi có lịch lái thử mới được đặt
        this.connection.on("TestDriveScheduled", (data) => {
            console.log("TestDrive scheduled:", data);
            this.showNotification(
                "Lịch lái thử mới", 
                data.message, 
                "info",
                {
                    testDriveId: data.testDriveId,
                    scheduleDate: data.scheduleDate,
                    carId: data.carId
                }
            );
        });

        // Xử lý khi lịch lái thử được cập nhật
        this.connection.on("TestDriveUpdated", (data) => {
            console.log("TestDrive updated:", data);
            this.showNotification(
                "Lịch lái thử đã cập nhật", 
                data.message, 
                "info",
                {
                    testDriveId: data.testDriveId,
                    scheduleDate: data.scheduleDate,
                    carId: data.carId
                }
            );
        });

        // Xử lý khi lịch lái thử bị hủy
        this.connection.on("TestDriveCancelled", (data) => {
            console.log("TestDrive cancelled:", data);
            this.showNotification(
                "Lịch lái thử bị hủy", 
                data.message, 
                "warning",
                {
                    testDriveId: data.testDriveId
                }
            );
        });

        // Xử lý khi kết nối bị mất
        this.connection.onclose((error) => {
            this.isConnected = false;
            console.log("SignalR connection closed:", error);
            this.showNotification("Mất kết nối", "Đang thử kết nối lại...", "warning");
        });

        // Xử lý khi kết nối lại
        this.connection.onreconnecting((error) => {
            console.log("SignalR reconnecting:", error);
            this.showNotification("Đang kết nối lại", "Đang thử kết nối lại...", "info");
        });

        // Xử lý khi kết nối lại thành công
        this.connection.onreconnected((connectionId) => {
            this.isConnected = true;
            console.log("SignalR reconnected:", connectionId);
            this.showNotification("Đã kết nối lại", "Kết nối đã được khôi phục", "success");
        });
    }

    showNotification(title, message, type = "info", data = null) {
        // Tạo notification element
        const notification = document.createElement('div');
        notification.className = `fixed top-4 right-4 z-50 max-w-sm w-full bg-white rounded-lg shadow-lg border-l-4 p-4 transform transition-all duration-300 ease-in-out ${
            type === 'success' ? 'border-green-500' : 
            type === 'error' ? 'border-red-500' : 
            type === 'warning' ? 'border-yellow-500' : 
            'border-blue-500'
        }`;

        const iconClass = type === 'success' ? 'fa-check-circle text-green-500' :
                         type === 'error' ? 'fa-exclamation-circle text-red-500' :
                         type === 'warning' ? 'fa-exclamation-triangle text-yellow-500' :
                         'fa-info-circle text-blue-500';

        notification.innerHTML = `
            <div class="flex items-start">
                <div class="flex-shrink-0">
                    <i class="fas ${iconClass}"></i>
                </div>
                <div class="ml-3 w-0 flex-1">
                    <p class="text-sm font-medium text-gray-900">${title}</p>
                    <p class="text-sm text-gray-500">${message}</p>
                    ${data ? `
                        <div class="mt-2 text-xs text-gray-400">
                            ${data.testDriveId ? `ID: ${data.testDriveId}` : ''}
                            ${data.scheduleDate ? `Ngày: ${new Date(data.scheduleDate).toLocaleDateString('vi-VN')}` : ''}
                        </div>
                    ` : ''}
                </div>
                <div class="ml-4 flex-shrink-0 flex">
                    <button class="bg-white rounded-md inline-flex text-gray-400 hover:text-gray-500 focus:outline-none" onclick="this.parentElement.parentElement.parentElement.remove()">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
        `;

        // Thêm vào DOM
        document.body.appendChild(notification);

        // Tự động xóa sau 5 giây
        setTimeout(() => {
            if (notification.parentElement) {
                notification.style.transform = 'translateX(100%)';
                setTimeout(() => {
                    if (notification.parentElement) {
                        notification.remove();
                    }
                }, 300);
            }
        }, 5000);
    }

    async stop() {
        if (this.connection) {
            await this.connection.stop();
            this.isConnected = false;
            console.log("SignalR connection stopped");
        }
    }

    // Kiểm tra trạng thái kết nối
    getConnectionState() {
        return this.connection ? this.connection.state : 'Disconnected';
    }
}

// Khởi tạo client khi trang được load
let testDriveClient = null;

document.addEventListener('DOMContentLoaded', function() {
    // Kiểm tra xem SignalR có sẵn không
    if (typeof signalR !== 'undefined') {
        testDriveClient = new TestDriveSignalRClient();
        testDriveClient.start();
    } else {
        console.error('SignalR library not loaded');
    }
});

// Cleanup khi trang bị unload
window.addEventListener('beforeunload', function() {
    if (testDriveClient) {
        testDriveClient.stop();
    }
});
