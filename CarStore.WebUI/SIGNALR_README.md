# SignalR TestDrive Hub - Hướng Dẫn Sử Dụng

## Tổng Quan
Dự án đã được tích hợp SignalR để cung cấp thông báo real-time cho hệ thống đặt lịch lái thử. Khi khách hàng đặt lịch lái thử, admin sẽ nhận được thông báo ngay lập tức.

## Các Tính Năng Đã Triển Khai

### 1. TestDriveHub
- **Vị trí**: `CarStore.WebUI/Hubs/TestDriveHub.cs`
- **Chức năng**: 
  - Tự động join vào group dựa trên role (User/Admin)
  - Quản lý kết nối và ngắt kết nối
  - Hỗ trợ group messaging

### 2. TestDriveNotificationService
- **Vị trí**: `CarStore.WebUI/Services/TestDriveNotificationService.cs`
- **Chức năng**:
  - Gửi thông báo khi có lịch lái thử mới
  - Gửi thông báo khi cập nhật trạng thái
  - Gửi thông báo khi hủy lịch lái thử

### 3. JavaScript Client
- **Vị trí**: `CarStore.WebUI/wwwroot/js/testdrive-signalr.js`
- **Chức năng**:
  - Kết nối tự động với SignalR hub
  - Hiển thị thông báo real-time với UI đẹp
  - Tự động reconnect khi mất kết nối
  - Quản lý trạng thái kết nối

## Cách Sử Dụng

### 1. Đặt Lịch Lái Thử (Customer)
1. Truy cập trang chi tiết xe
2. Nhấn nút "Đặt Lái Thử"
3. Điền thông tin và submit
4. Admin sẽ nhận được thông báo real-time

### 2. Quản Lý Lịch Lái Thử (Admin)
1. Truy cập `/Admin/TestDrives`
2. Xem danh sách tất cả lịch lái thử
3. Cập nhật trạng thái bằng dropdown
4. Customer sẽ nhận được thông báo real-time

### 3. Xem Lịch Của Tôi (Customer)
1. Truy cập `/TestDrives/MyTestDrives`
2. Xem tất cả lịch lái thử của mình
3. Nhận thông báo khi admin cập nhật trạng thái

## Các Loại Thông Báo

### 1. TestDriveScheduled
- **Khi nào**: Khi customer đặt lịch lái thử mới
- **Ai nhận**: Admin và Customer
- **Nội dung**: Thông tin lịch lái thử mới

### 2. TestDriveUpdated
- **Khi nào**: Khi admin cập nhật trạng thái
- **Ai nhận**: Admin và Customer
- **Nội dung**: Thông tin cập nhật

### 3. TestDriveCancelled
- **Khi nào**: Khi hủy lịch lái thử
- **Ai nhận**: Admin và Customer
- **Nội dung**: Thông tin hủy lịch

## Cấu Hình SignalR

### Program.cs
```csharp
// Thêm SignalR service
builder.Services.AddSignalR();

// Map hub
app.MapHub<TestDriveHub>("/testdrivehub");
```

### Layout.cshtml
```html
<!-- SignalR Library -->
<script src="https://unpkg.com/@microsoft/signalr@latest/dist/browser/signalr.min.js"></script>
<!-- TestDrive SignalR Client -->
<script src="~/js/testdrive-signalr.js" asp-append-version="true"></script>
```

## Trạng Thái Kết Nối

- 🟢 **Đã kết nối**: SignalR hoạt động bình thường
- 🟡 **Đang kết nối**: Đang thử kết nối lại
- 🔴 **Mất kết nối**: Không thể kết nối với server

## Troubleshooting

### 1. Không nhận được thông báo
- Kiểm tra console browser có lỗi không
- Kiểm tra trạng thái kết nối SignalR
- Kiểm tra user đã đăng nhập chưa

### 2. Lỗi kết nối SignalR
- Kiểm tra URL hub có đúng không
- Kiểm tra authentication
- Kiểm tra CORS settings (nếu có)

### 3. Thông báo không hiển thị
- Kiểm tra JavaScript console
- Kiểm tra CSS có bị conflict không
- Kiểm tra SignalR client có load không

## Testing

### 1. Test Đặt Lịch
1. Mở 2 browser: 1 làm customer, 1 làm admin
2. Customer đặt lịch lái thử
3. Kiểm tra admin có nhận thông báo không

### 2. Test Cập Nhật Trạng Thái
1. Admin cập nhật trạng thái lịch lái thử
2. Kiểm tra customer có nhận thông báo không

### 3. Test Reconnect
1. Tắt mạng internet
2. Bật lại mạng
3. Kiểm tra SignalR có tự động kết nối lại không

## Lưu Ý Kỹ Thuật

1. **Authentication**: SignalR sử dụng cookie authentication
2. **Groups**: User được tự động join vào group dựa trên role
3. **Reconnection**: Tự động reconnect khi mất kết nối
4. **UI**: Thông báo có animation và tự động ẩn sau 5 giây
5. **Performance**: Sử dụng connection pooling và efficient messaging

## Mở Rộng

Để thêm tính năng mới:
1. Thêm method trong `TestDriveHub`
2. Thêm notification trong `TestDriveNotificationService`
3. Thêm event handler trong JavaScript client
4. Cập nhật UI để hiển thị thông báo mới
