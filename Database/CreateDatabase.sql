-- ============================================================
-- CO SO DU LIEU: QuanLyPhimDB
-- Ung dung Quan Ly Phim & Lich Chieu (Offline)
-- SQL Server
-- ============================================================

-- Tao database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'QuanLyPhimDB')
    CREATE DATABASE QuanLyPhimDB;
GO

USE QuanLyPhimDB;
GO

-- ============================================================
-- BANG: TheLoai
-- ============================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TheLoai')
CREATE TABLE TheLoai (
    MaTheLoai   INT IDENTITY(1,1) PRIMARY KEY,
    TenTheLoai  NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- ============================================================
-- BANG: Phim
-- ============================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Phim')
CREATE TABLE Phim (
    MaPhim      INT IDENTITY(1,1) PRIMARY KEY,
    TenPhim     NVARCHAR(255) NOT NULL,
    MaTheLoai   INT NOT NULL,
    ThoiLuong   INT NOT NULL,              -- phut
    QuocGia     NVARCHAR(100),
    MoTa        NVARCHAR(MAX),
    TrangThai   NVARCHAR(50) DEFAULT N'Dang chieu',  -- Dang chieu / Sap chieu / Ngung chieu
    NgayTao     DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Phim_TheLoai FOREIGN KEY (MaTheLoai) REFERENCES TheLoai(MaTheLoai)
);
GO

-- ============================================================
-- BANG: PhongChieu
-- ============================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PhongChieu')
CREATE TABLE PhongChieu (
    MaPhong     INT IDENTITY(1,1) PRIMARY KEY,
    TenPhong    NVARCHAR(100) NOT NULL,
    SucChua     INT NOT NULL DEFAULT 50,
    LoaiPhong   NVARCHAR(20) DEFAULT '2D', -- 2D / 3D / IMAX
    TrangThai   NVARCHAR(50) DEFAULT N'Hoat dong' -- Hoat dong / Bao tri
);
GO

-- ============================================================
-- BANG: SuatChieu
-- ============================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SuatChieu')
CREATE TABLE SuatChieu (
    MaSuat      INT IDENTITY(1,1) PRIMARY KEY,
    MaPhim      INT NOT NULL,
    MaPhong     INT NOT NULL,
    NgayChieu   DATE NOT NULL,
    GioChieu    TIME NOT NULL,
    GiaVe       INT NOT NULL DEFAULT 75000, -- VND
    CONSTRAINT FK_Suat_Phim FOREIGN KEY (MaPhim) REFERENCES Phim(MaPhim) ON DELETE CASCADE,
    CONSTRAINT FK_Suat_Phong FOREIGN KEY (MaPhong) REFERENCES PhongChieu(MaPhong)
);
GO

CREATE INDEX IX_SuatChieu_NgayChieu ON SuatChieu(NgayChieu);
CREATE INDEX IX_SuatChieu_Phim ON SuatChieu(MaPhim);
GO

-- ============================================================
-- BANG: Ve
-- ============================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Ve')
CREATE TABLE Ve (
    MaVe            INT IDENTITY(1,1) PRIMARY KEY,
    MaSuat          INT NOT NULL,
    TenKhachHang    NVARCHAR(255) NOT NULL,
    SoDienThoai     NVARCHAR(20),
    MaGhe           NVARCHAR(10) NOT NULL,    -- A1, B3, ...
    GiaVe           INT NOT NULL,
    NgayBan         DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Ve_Suat FOREIGN KEY (MaSuat) REFERENCES SuatChieu(MaSuat) ON DELETE CASCADE,
    CONSTRAINT UQ_Ve_Ghe UNIQUE (MaSuat, MaGhe)  -- khong ban trung ghe
);
GO

-- ============================================================
-- STORED PROCEDURE: Thong ke ve ban theo phim
-- ============================================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_ThongKeVeTheoPhim')
    DROP PROCEDURE sp_ThongKeVeTheoPhim;
GO

CREATE PROCEDURE sp_ThongKeVeTheoPhim
AS
BEGIN
    SELECT 
        p.MaPhim,
        p.TenPhim,
        tl.TenTheLoai,
        COUNT(v.MaVe) AS SoVeBan,
        ISNULL(SUM(v.GiaVe), 0) AS DoanhThu
    FROM Phim p
    INNER JOIN TheLoai tl ON p.MaTheLoai = tl.MaTheLoai
    LEFT JOIN SuatChieu sc ON p.MaPhim = sc.MaPhim
    LEFT JOIN Ve v ON sc.MaSuat = v.MaSuat
    GROUP BY p.MaPhim, p.TenPhim, tl.TenTheLoai
    ORDER BY SoVeBan DESC;
END
GO

-- ============================================================
-- STORED PROCEDURE: Tong quan dashboard
-- ============================================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_TongQuan')
    DROP PROCEDURE sp_TongQuan;
GO

CREATE PROCEDURE sp_TongQuan
AS
BEGIN
    SELECT
        (SELECT COUNT(*) FROM Phim) AS TongPhim,
        (SELECT COUNT(*) FROM PhongChieu) AS TongPhong,
        (SELECT COUNT(*) FROM SuatChieu) AS TongSuatChieu,
        (SELECT COUNT(*) FROM Ve) AS TongVe,
        (SELECT COUNT(*) FROM Ve WHERE CAST(NgayBan AS DATE) = CAST(GETDATE() AS DATE)) AS VeHomNay,
        (SELECT ISNULL(SUM(GiaVe), 0) FROM Ve) AS TongDoanhThu
END
GO

-- ============================================================
-- DU LIEU MAU
-- ============================================================

-- The loai
INSERT INTO TheLoai (TenTheLoai) VALUES 
(N'Hanh dong'), (N'Tinh cam'), (N'Kinh di'), (N'Hai huoc'),
(N'Vien tuong'), (N'Hoat hinh'), (N'Phieu luu'), (N'Tam ly');
GO

-- Phim
INSERT INTO Phim (TenPhim, MaTheLoai, ThoiLuong, QuocGia, MoTa, TrangThai) VALUES
(N'Avengers: Endgame',       5, 181, N'My',       N'Bieu tuong sieu anh hung Marvel tro lai', N'Dang chieu'),
(N'John Wick 4',             1, 169, N'My',       N'Sat thu huyen thoai John Wick', N'Dang chieu'),
(N'Mai',                     2, 130, N'Viet Nam', N'Cau chuyen tinh cam dong dat Sai Gon', N'Dang chieu'),
(N'Conjuring 4',             3, 112, N'My',       N'Am anh kinh hoang tiep tuc', N'Sap chieu'),
(N'Co May Chien Tranh',      1, 128, N'Han Quoc', N'Phim hanh dong chien tranh', N'Dang chieu'),
(N'Inside Out 2',            6, 100, N'My',       N'Cuoc phieu luu cam xuc moi', N'Dang chieu');
GO

-- Phong chieu
INSERT INTO PhongChieu (TenPhong, SucChua, LoaiPhong, TrangThai) VALUES
(N'Phong 1', 60, '2D', N'Hoat dong'),
(N'Phong 2', 80, '3D', N'Hoat dong'),
(N'Phong 3', 100, 'IMAX', N'Hoat dong'),
(N'Phong 4', 50, '2D', N'Bao tri');
GO

-- Suat chieu (ngay hien tai va ngay mai)
DECLARE @today DATE = CAST(GETDATE() AS DATE);
DECLARE @tomorrow DATE = DATEADD(DAY, 1, @today);

INSERT INTO SuatChieu (MaPhim, MaPhong, NgayChieu, GioChieu, GiaVe) VALUES
(1, 3, @today,    '10:00', 120000),
(1, 3, @today,    '14:00', 120000),
(2, 1, @today,    '09:30', 75000),
(2, 1, @today,    '18:00', 90000),
(3, 2, @today,    '11:00', 85000),
(5, 1, @tomorrow, '10:00', 75000),
(6, 2, @tomorrow, '14:30', 95000),
(1, 3, @tomorrow, '19:00', 130000);
GO

-- Ve mau
INSERT INTO Ve (MaSuat, TenKhachHang, SoDienThoai, MaGhe, GiaVe) VALUES
(1, N'Nguyen Van An',  '0901234567', 'A1', 120000),
(1, N'Nguyen Van An',  '0901234567', 'A2', 120000),
(2, N'Tran Thi Bich',  '0912345678', 'B5', 120000),
(3, N'Le Van Chau',    '0923456789', 'C3', 75000),
(5, N'Pham Thi Dung',  '0934567890', 'A1', 85000);
GO

PRINT N'=== Tao co so du lieu QuanLyPhimDB thanh cong! ===';
GO
