-- ==========================================
-- OPTIONAL: Xóa DB cũ (nếu muốn tạo sạch)
-- ==========================================
use master
IF DB_ID('infertilityTreatment') IS NOT NULL
 BEGIN
    ALTER DATABASE infertilityTreatment SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
   DROP DATABASE infertilityTreatment;
END
GO

-- ==========================================
-- Tạo DB & chọn DB
-- ==========================================
IF DB_ID('infertilityTreatment') IS NULL
    CREATE DATABASE infertilityTreatment;
GO

USE infertilityTreatment;
GO

-- ====================
-- Bảng RoleType
-- ====================
IF OBJECT_ID('dbo.RoleType', 'U') IS NOT NULL DROP TABLE dbo.RoleType;
GO
CREATE TABLE dbo.RoleType (
    Role INT PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL
);
GO

-- ====================
-- Bảng Users (đã cập nhật)
-- ====================
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
GO
CREATE TABLE dbo.Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    RoleId INT NOT NULL,
    FullName NVARCHAR(100) NULL,
    UserName NVARCHAR(100) NOT NULL,
    [Password] NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Age INT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
    StartActiveDate DATETIME NULL,
    CONSTRAINT FK_Users_RoleType FOREIGN KEY (RoleId) REFERENCES dbo.RoleType(Role)
);
GO

-- ====================
-- Bảng TreatmentService (đã cập nhật)
-- ====================
IF OBJECT_ID('dbo.TreatmentService', 'U') IS NOT NULL DROP TABLE dbo.TreatmentService;
GO
CREATE TABLE dbo.TreatmentService (
    ServiceId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,               -- Manager/Owner (User)
    ServiceName NVARCHAR(255) NULL,
    [Description] NVARCHAR(MAX) NULL,  -- mới thêm
    Price DECIMAL(18,2) NULL,
    CONSTRAINT FK_TreatmentService_User FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId)
);
GO

-- ====================
-- Bảng Appointment (tên SINGULAR để khớp EF)
-- ====================
IF OBJECT_ID('dbo.Appointment', 'U') IS NOT NULL DROP TABLE dbo.Appointment;
GO
CREATE TABLE dbo.Appointment (
    AppointmentId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    DoctorId INT NOT NULL,
    ServiceId INT NOT NULL,
    AppointmentDate DATETIME,
    [Status] NVARCHAR(50) NULL,
	 RejectReason NVARCHAR(MAX) NULL,
	  CancelReason NVARCHAR(MAX) NULL
    CONSTRAINT FK_Appointment_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Appointment_Doctor FOREIGN KEY (DoctorId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Appointment_Service FOREIGN KEY (ServiceId) REFERENCES dbo.TreatmentService(ServiceId)
);

-- ====================
-- Bảng BLog (đúng tên như trong OnModelCreating)
-- ====================
IF OBJECT_ID('dbo.BLog', 'U') IS NOT NULL DROP TABLE dbo.BLog;
GO
CREATE TABLE dbo.BLog (
    BlogId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(255) NULL,
    [Content] NVARCHAR(MAX) NULL,
    CreatedDate DATETIME NULL,
    CONSTRAINT FK_Blog_User FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId)
);
GO

-- ====================
-- Bảng Feedback
-- ====================
IF OBJECT_ID('dbo.Feedback', 'U') IS NOT NULL DROP TABLE dbo.Feedback;
GO
CREATE TABLE dbo.Feedback (
    FeedbackId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    CreatedDate DATETIME NOT NULL,
	Rating INT NOT NULL,
	Comment NVARCHAR(MAX),
    CONSTRAINT FK_Feedback_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Users(UserId)
);
GO

-- ====================
-- Bảng Schedule (tên SINGULAR, giữ nguyên cột SerivceName như model)
-- ====================
IF OBJECT_ID('dbo.Schedule', 'U') IS NOT NULL DROP TABLE dbo.Schedule;
GO
CREATE TABLE dbo.Schedule (
    ScheduleId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    DoctorId INT NOT NULL,
    SerivceName NVARCHAR(255) NULL,  -- (nguyên bản bị typo, giữ nguyên để khớp EF)
    ScheduleDate DATETIME NULL,
	Note NVARCHAR(MAX) NULL,
    CONSTRAINT FK_Schedule_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Schedule_Doctor FOREIGN KEY (DoctorId) REFERENCES dbo.Users(UserId)
);
GO

SELECT * FROM Users
-- ====================
-- Bảng MedicalRecord
-- ====================
IF OBJECT_ID('dbo.MedicalRecord', 'U') IS NOT NULL DROP TABLE dbo.MedicalRecord;
GO
CREATE TABLE dbo.MedicalRecord (
    RecordId INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentId INT NOT NULL,
    CustomerId INT NOT NULL,
    DoctorId INT NOT NULL,
    Notes NVARCHAR(MAX) NULL,             -- dùng 'Notes' để đồng nhất
    CreatedDate DATETIME NULL,
    CONSTRAINT FK_MedicalRecord_Appointment FOREIGN KEY (AppointmentId) REFERENCES dbo.Appointment(AppointmentId),
    CONSTRAINT FK_MedicalRecord_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_MedicalRecord_Doctor FOREIGN KEY (DoctorId) REFERENCES dbo.Users(UserId)
);
GO

-- ====================
-- Bảng PatientRequest
-- ====================
IF OBJECT_ID('dbo.PatientRequest', 'U') IS NOT NULL DROP TABLE dbo.PatientRequest;
GO
CREATE TABLE dbo.PatientRequest (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    DoctorId INT NOT NULL,
    ServiceId INT NOT NULL,
    RequestedDate DATETIME NULL,
    CreatedDate DATETIME NULL,
    Notes NVARCHAR(MAX) NULL,
    CONSTRAINT FK_PatientRequest_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_PatientRequest_Doctor FOREIGN KEY (DoctorId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_PatientRequest_Service FOREIGN KEY (ServiceId) REFERENCES dbo.TreatmentService(ServiceId)
);
GO

-- ==========================================
-- SEED DATA
-- ==========================================

-- Role types
INSERT INTO dbo.RoleType (Role, RoleName)
VALUES (1, N'Admin'),
       (2, N'Doctor'),
       (3, N'Manager'),
       (4, N'Customer'),
       (5, N'Guest');
GO

SELECT * FROM Users
-- Tạo Users (1 admin, 3 doctor, 5 customer)
INSERT INTO dbo.Users (RoleId, FullName, UserName, [Password], PhoneNumber, Age , IsActive)
VALUES


-- Admin
(1, N'Admin User', N'admin',  N'admin123', N'0900000001', '21',1),

-- Manager
(2, N'John Smith', N'drjohn',  N'pass123',  N'0900000002','21', 1),
(2, N'Jane Doe',   N'drjane',  N'pass123',  N'0900000003','21', 1),
(2, N'David Lee',  N'drdavid', N'pass123',  N'0900000004','21', 1),
-- Doctors
(3, N'Dr. Peter Parker', N'PParker',  N'pass123',  N'0900000003','21', 1),
(3, N'Dr. Nam Nguyen',   N'namnguyen',  N'pass123',  N'0900000004','21', 1),
(3, N'Dr. John Pie',  N'johnPie', N'pass123',  N'0900000005','21', 1),
-- Customers
(4, N'Alice Nguyen',   N'alice',   N'pass123',  N'0900000005','22', 1),
(4, N'Bob Tran',       N'bob',     N'pass123',  N'0900000006','20', 1),
(4, N'Charlie Pham',   N'charlie', N'pass123',  N'0900000007','19', 1),
(4, N'Daisy Le',       N'daisy',   N'pass123',  N'0900000008','20', 1),
(4, N'Ethan Vu',       N'ethan',   N'pass123',  N'0900000009','22', 1);
GO

-- Treatment Services
INSERT INTO dbo.TreatmentService (UserId, ServiceName, [Description], Price)
VALUES
(2, N'IVF Consultation',  N'Initial IVF consultation',  500.00),
(3, N'IUI Procedure',      N'Intrauterine insemination', 700.00),
(4, N'Egg Freezing',       N'Oocyte cryopreservation',  1200.00),
(2, N'Fertility Testing',  N'Baseline fertility tests',  300.00),
(3, N'Embryo Transfer',    N'Embryo transfer procedure', 1500.00);
GO

-- Appointments
INSERT INTO dbo.Appointment (CustomerId, DoctorId, ServiceId, AppointmentDate, [Status])
VALUES
(5, 2, 1, '2025-08-15 09:00', N'Scheduled'),
(6, 3, 2, '2025-08-16 14:00', N'Scheduled'),
(7, 4, 3, '2025-08-17 10:00', N'Completed'),
(8, 2, 4, '2025-08-18 11:00', N'Cancelled'),
(9, 3, 5, '2025-08-19 15:00', N'Scheduled');
GO

-- Blogs
INSERT INTO dbo.BLog (UserId, Title, [Content], CreatedDate)
VALUES
(2, N'Understanding IVF',           N'Content about IVF...', GETDATE()),
(3, N'Fertility Tips',               N'Some useful tips...', GETDATE()),
(4, N'Benefits of Egg Freezing',     N'Egg freezing helps preserve fertility...', GETDATE()),
(2, N'Diet for Fertility',           N'Healthy diet can improve fertility...', GETDATE()),
(3, N'Embryo Transfer Process',      N'Step-by-step guide...', GETDATE());
GO

-- Feedback (sửa để khớp với bảng: Rating + Comment, không có [Content])
INSERT INTO dbo.Feedback (CustomerId, CreatedDate, Rating, Comment)
VALUES
(5, GETDATE(), 5, N'Great service!'),
(6, GETDATE(), 4, N'Very professional staff.'),
(7, GETDATE(), 5, N'Doctor was kind and helpful.'),
(8, GETDATE(), 4, N'Facility was clean and modern.'),
(9, GETDATE(), 5, N'Highly recommend!');
GO

-- Schedules
INSERT INTO dbo.Schedule (CustomerId, DoctorId, SerivceName, ScheduleDate, Note)
VALUES
(5, 2, N'IVF Consultation', '2025-08-15 09:00', NULL),
(6, 3, N'IUI Procedure',    '2025-08-16 14:00', NULL),
(7, 4, N'Egg Freezing',     '2025-08-17 10:00', NULL),
(8, 2, N'Fertility Testing','2025-08-18 11:00', NULL),
(9, 3, N'Embryo Transfer',  '2025-08-19 15:00', NULL);
GO

-- Medical Records
INSERT INTO dbo.MedicalRecord (AppointmentId, CustomerId, DoctorId, Notes, CreatedDate)
VALUES
(1, 5, 2, N'Medical record details for Alice',   GETDATE()),
(2, 6, 3, N'Medical record details for Bob',     GETDATE()),
(3, 7, 4, N'Medical record details for Charlie', GETDATE()),
(4, 8, 2, N'Medical record details for Daisy',   GETDATE()),
(5, 9, 3, N'Medical record details for Ethan',   GETDATE());
GO

-- Patient Requests
INSERT INTO dbo.PatientRequest (CustomerId, DoctorId, ServiceId, RequestedDate, CreatedDate, Notes)
VALUES
(5, 2, 1, GETDATE(), GETDATE(), N'Request for IVF consultation'),
(6, 3, 2, GETDATE(), GETDATE(), N'Request for IUI procedure'),
(7, 4, 3, GETDATE(), GETDATE(), N'Request for egg freezing'),
(8, 2, 4, GETDATE(), GETDATE(), N'Request for fertility testing'),
(9, 3, 5, GETDATE(), GETDATE(), N'Request for embryo transfer');
GO
SELECT * FROM Users