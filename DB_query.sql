-- =======================================================
-- 1. TẠO VÀ SỬ DỤNG DATABASE
-- =======================================================
CREATE DATABASE CarStoreDB;
GO

USE CarStoreDB;
GO

-- =======================================================
-- 2. TẠO BẢNG NGƯỜI DÙNG (USERS)
-- =======================================================
-- LƯU Ý QUAN TRỌNG: Trong ứng dụng thực tế, PasswordHash phải lưu trữ
-- kết quả BĂM (hash) của mật khẩu, không phải mật khẩu thô.
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL, 
    Phone NVARCHAR(20),
    Role NVARCHAR(20) DEFAULT 'Customer', -- Customer / Admin
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- =======================================================
-- 3. TẠO BẢNG XE Ô TÔ (CARS)
-- =======================================================
CREATE TABLE Cars (
    CarId INT IDENTITY(1,1) PRIMARY KEY,
    Brand NVARCHAR(50) NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL, -- Giá tiền (ví dụ: VNĐ)
    Year INT NOT NULL,
    ImageUrl NVARCHAR(255),
    Description NVARCHAR(MAX),
    Stock INT DEFAULT 1 CHECK (Stock >= 0), -- Đảm bảo tồn kho không âm
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- =======================================================
-- 4. TẠO BẢNG ĐƠN HÀNG (ORDERS - Flow Mua Xe)
-- =======================================================
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    CarId INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    Quantity INT DEFAULT 1 CHECK (Quantity >= 1),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending', -- Pending / Completed / Canceled
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (CarId) REFERENCES Cars(CarId)
);
GO

-- =======================================================
-- 5. TẠO BẢNG LỊCH LÁI THỬ (TESTDRIVES - Flow Lái Thử)
-- =======================================================
CREATE TABLE TestDrives (
    TestDriveId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    CarId INT NOT NULL,
    ScheduleDate DATETIME NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Scheduled', -- Scheduled / Done / Canceled
    Note NVARCHAR(255),
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (CarId) REFERENCES Cars(CarId)
);
GO

-- =======================================================
-- 6. TRIGGER: Tự động trừ tồn kho khi tạo đơn hàng
-- =======================================================
-- Trigger này chạy mỗi khi có INSERT vào bảng Orders.
CREATE TRIGGER trg_UpdateCarStock
ON Orders
AFTER INSERT
AS
BEGIN
    -- Chỉ trừ tồn kho nếu đơn hàng được tạo (Quantity > 0)
    UPDATE C
    SET C.Stock = C.Stock - I.Quantity
    FROM Cars C
    INNER JOIN inserted I ON C.CarId = I.CarId
    WHERE C.Stock >= I.Quantity; -- Tránh tồn kho âm (đã có CHECK, nhưng thêm điều kiện an toàn hơn)
END
GO

-- =======================================================
-- 7. DỮ LIỆU MẪU (SAMPLE DATA)
-- =======================================================

-- Người dùng (3 bản ghi)
-- LƯU Ý: Mật khẩu dưới đây chỉ dùng cho môi trường phát triển/kiểm thử.
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role)
VALUES 
(N'Nguyễn Văn A', 'a@example.com', '123456', '0909123456', 'Customer'),
(N'Trần Thị B', 'b@example.com', '123456', '0909988776', 'Customer'),
(N'Admin Cửa Hàng', 'admin@carstore.com', 'admin123', '0987654321', 'Admin');
GO
-- =======================================================
-- 7. DỮ LIỆU MẪU: XE Ô TÔ (CARS) CẬP NHẬT VỚI MÔ TẢ CHI TIẾT
-- =======================================================

INSERT INTO Cars (Brand, Model, Price, Year, ImageUrl, Description, Stock)
VALUES
(
    'Toyota', 
    'Camry 2.5Q', 
    1200000000, 
    2024, 
    'https://toyotamydinh.com.vn/Upload/tin/toyota-camry-25q-hong-xe.jpg', 
    N'Phiên bản cao cấp nhất, trang bị động cơ 2.5L, nội thất da cao cấp, hệ thống an toàn Toyota Safety Sense 3.0. Thiết kế ngoại thất sang trọng, lịch lãm, phù hợp với doanh nhân.', 
    5
),
(
    'VinFast', 
    'Lux A2.0', 
    950000000, 
    2023, 
    'https://storage.googleapis.com/vinfast-data-01/Noi-that-xe-VinFast-Lux-A2.0-hang-sang-duoc-yeu%20thich.jpg', 
    N'Sedan hạng D mang thiết kế Ý của Pininfarina. Xe sử dụng động cơ BMW N20 mạnh mẽ, nội thất bọc da Nappa, màn hình giải trí lớn 10.4 inch, mang lại trải nghiệm lái thể thao và đẳng cấp.', 
    3
),
(
    'Mazda', 
    'CX-5', 
    870000000, 
    2024, 
    'https://th.bing.com/th/id/R.2e8a90a77fb0b56fa841b9174f35bb8a?rik=AYUy9wxK5G%2fUDA&pid=ImgRaw&r=0', 
    N'Mẫu SUV bán chạy với ngôn ngữ thiết kế KODO thế hệ mới. Trang bị công nghệ G-Vectoring Control Plus (GVC Plus), nội thất tinh tế và hệ thống đèn LED thích ứng, tối ưu cho cả đô thị và đường trường.', 
    7
),
(
    'Mercedes-Benz', 
    'GLC 300', 
    2500000000, 
    2024, 
    'https://tse3.mm.bing.net/th/id/OIP.Ij40uMLuCCOs86QxCzmbOwHaEK?cb=12&w=1920&h=1080&rs=1&pid=ImgDetMain&o=7&rm=3', 
    N'Phiên bản SUV sang trọng với gói ngoại thất AMG Line. Động cơ 2.0L tăng áp, hộp số 9 cấp 9G-TRONIC, hệ thống MBUX với trí tuệ nhân tạo và hệ thống treo Agility Control, mang lại sự êm ái tuyệt đối.', 
    2
),
(
    'Honda', 
    'Civic RS', 
    870000000, 
    2024, 
    'https://media.auto5.vn/files/quanganh/2024/08/03/2025-honda-civic-rs-jdm%20(6)-184359.jpg', 
    N'Phiên bản thể thao với động cơ VTEC Turbo 1.5L mạnh mẽ. Thiết kế cánh gió lớn, nội thất đen đỏ cá tính. Tích hợp gói công nghệ an toàn Honda Sensing, lý tưởng cho những khách hàng yêu thích tốc độ và phong cách năng động.', 
    4
);
GO
-- =======================================================
-- 7. DỮ LIỆU MẪU: ĐƠN HÀNG (ORDERS) CẬP NHẬT
-- =======================================================
-- Giả sử UserId = 1 (Nguyễn Văn A) và UserId = 2 (Trần Thị B) đã tồn tại.
-- Các đơn hàng này sẽ tự động trừ vào tồn kho của bảng Cars.

INSERT INTO Orders (UserId, CarId, Quantity, TotalAmount, Status)
VALUES 
(
    1, -- Nguyễn Văn A
    3, -- Mua một chiếc Mazda CX-5
    1, 
    870000000, 
    'Completed' -- Đơn hàng đã hoàn thành
),
(
    2, -- Trần Thị B
    5, -- Mua một chiếc Honda Civic RS
    1, 
    870000000, 
    'Pending' -- Đơn hàng đang chờ xử lý
),
(
    1, -- Nguyễn Văn A
    4, -- Mua thêm một chiếc Mercedes-Benz GLC 300
    1, 
    2500000000, 
    'Pending' -- Đơn hàng đang chờ xử lý
),
(
    2, -- Trần Thị B
    1, -- Mua một chiếc Toyota Camry 2.5Q
    1,
    1200000000,
    'Canceled' -- Đơn hàng đã bị hủy
);
GO
-- =======================================================
-- 7. DỮ LIỆU MẪU: LỊCH LÁI THỬ (TESTDRIVES) CẬP NHẬT
-- =======================================================
-- Các lịch lái thử được đặt vào các thời điểm khác nhau.

INSERT INTO TestDrives (UserId, CarId, ScheduleDate, Status, Note)
VALUES
(
    1, -- Nguyễn Văn A
    2, -- Đăng ký lái thử chiếc VinFast Lux A2.0
    '2025-10-22 09:30:00', -- Lịch hẹn trong tương lai
    'Scheduled', 
    N'Muốn trải nghiệm cảm giác lái và kiểm tra hệ thống giải trí.'
),
(
    2, -- Trần Thị B
    4, -- Đăng ký lái thử chiếc Mercedes-Benz GLC 300
    '2025-10-14 15:00:00', -- Lịch hẹn đã qua
    'Done', 
    N'Khách hàng rất hài lòng với độ êm ái của xe. Cần tư vấn thêm về gói tài chính.'
),
(
    1, -- Nguyễn Văn A
    5, -- Đăng ký lái thử Honda Civic RS
    '2025-10-25 10:00:00', -- Lịch hẹn trong tương lai
    'Scheduled',
    N'Quan tâm đến khả năng tăng tốc và các tính năng an toàn của Honda Sensing.'
),
(
    2, -- Trần Thị B
    3, -- Đăng ký lái thử Mazda CX-5
    '2025-11-01 14:00:00', -- Lịch hẹn đã bị hủy
    'Canceled',
    N'Khách hàng báo bận việc đột xuất, sẽ sắp xếp lại sau.'
);
GO