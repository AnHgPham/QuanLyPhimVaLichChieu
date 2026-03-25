-- ============================================================
-- CO SO DU LIEU: QuanLyPhimDB
-- Ung dung Quan Ly Phim & Lich Chieu (Offline)
-- MySQL
-- ============================================================

-- Tao database
CREATE DATABASE IF NOT EXISTS QuanLyPhimDB
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE QuanLyPhimDB;

-- ============================================================
-- BANG: TheLoai
-- ============================================================
CREATE TABLE IF NOT EXISTS TheLoai (
    MaTheLoai   INT AUTO_INCREMENT PRIMARY KEY,
    TenTheLoai  VARCHAR(100) NOT NULL UNIQUE
);

-- ============================================================
-- BANG: Phim
-- ============================================================
CREATE TABLE IF NOT EXISTS Phim (
    MaPhim      INT AUTO_INCREMENT PRIMARY KEY,
    TenPhim     VARCHAR(255) NOT NULL,
    MaTheLoai   INT NOT NULL,
    ThoiLuong   INT NOT NULL,
    QuocGia     VARCHAR(100),
    MoTa        TEXT,
    TrangThai   VARCHAR(50) DEFAULT 'Dang chieu',
    NgayTao     DATETIME DEFAULT NOW(),
    NgayCapNhat DATETIME DEFAULT NOW(),
    CONSTRAINT FK_Phim_TheLoai FOREIGN KEY (MaTheLoai) REFERENCES TheLoai(MaTheLoai)
);

-- ============================================================
-- BANG: PhongChieu
-- ============================================================
CREATE TABLE IF NOT EXISTS PhongChieu (
    MaPhong     INT AUTO_INCREMENT PRIMARY KEY,
    TenPhong    VARCHAR(100) NOT NULL,
    SucChua     INT NOT NULL DEFAULT 50,
    LoaiPhong   VARCHAR(20) DEFAULT '2D',
    TrangThai   VARCHAR(50) DEFAULT 'Hoat dong'
);

-- ============================================================
-- BANG: SuatChieu
-- ============================================================
CREATE TABLE IF NOT EXISTS SuatChieu (
    MaSuat      INT AUTO_INCREMENT PRIMARY KEY,
    MaPhim      INT NOT NULL,
    MaPhong     INT NOT NULL,
    NgayChieu   DATE NOT NULL,
    GioChieu    TIME NOT NULL,
    GiaVe       INT NOT NULL DEFAULT 75000,
    CONSTRAINT FK_Suat_Phim FOREIGN KEY (MaPhim) REFERENCES Phim(MaPhim) ON DELETE CASCADE,
    CONSTRAINT FK_Suat_Phong FOREIGN KEY (MaPhong) REFERENCES PhongChieu(MaPhong)
);

CREATE INDEX IX_SuatChieu_NgayChieu ON SuatChieu(NgayChieu);
CREATE INDEX IX_SuatChieu_Phim ON SuatChieu(MaPhim);

-- ============================================================
-- BANG: Ve
-- ============================================================
CREATE TABLE IF NOT EXISTS Ve (
    MaVe            INT AUTO_INCREMENT PRIMARY KEY,
    MaSuat          INT NOT NULL,
    TenKhachHang    VARCHAR(255) NOT NULL,
    SoDienThoai     VARCHAR(20),
    MaGhe           VARCHAR(10) NOT NULL,
    GiaVe           INT NOT NULL,
    NgayBan         DATETIME DEFAULT NOW(),
    CONSTRAINT FK_Ve_Suat FOREIGN KEY (MaSuat) REFERENCES SuatChieu(MaSuat) ON DELETE CASCADE,
    CONSTRAINT UQ_Ve_Ghe UNIQUE (MaSuat, MaGhe)
);

-- ============================================================
-- STORED PROCEDURE: Thong ke ve ban theo phim
-- ============================================================
DROP PROCEDURE IF EXISTS sp_ThongKeVeTheoPhim;

DELIMITER //
CREATE PROCEDURE sp_ThongKeVeTheoPhim()
BEGIN
    SELECT 
        p.MaPhim,
        p.TenPhim,
        tl.TenTheLoai,
        COUNT(v.MaVe) AS SoVeBan,
        IFNULL(SUM(v.GiaVe), 0) AS DoanhThu
    FROM Phim p
    INNER JOIN TheLoai tl ON p.MaTheLoai = tl.MaTheLoai
    LEFT JOIN SuatChieu sc ON p.MaPhim = sc.MaPhim
    LEFT JOIN Ve v ON sc.MaSuat = v.MaSuat
    GROUP BY p.MaPhim, p.TenPhim, tl.TenTheLoai
    ORDER BY SoVeBan DESC;
END //
DELIMITER ;

-- ============================================================
-- STORED PROCEDURE: Tong quan dashboard
-- ============================================================
DROP PROCEDURE IF EXISTS sp_TongQuan;

DELIMITER //
CREATE PROCEDURE sp_TongQuan()
BEGIN
    SELECT
        (SELECT COUNT(*) FROM Phim) AS TongPhim,
        (SELECT COUNT(*) FROM PhongChieu) AS TongPhong,
        (SELECT COUNT(*) FROM SuatChieu) AS TongSuatChieu,
        (SELECT COUNT(*) FROM Ve) AS TongVe,
        (SELECT COUNT(*) FROM Ve WHERE DATE(NgayBan) = CURDATE()) AS VeHomNay,
        (SELECT IFNULL(SUM(GiaVe), 0) FROM Ve) AS TongDoanhThu;
END //
DELIMITER ;

-- ============================================================
-- DU LIEU MAU
-- ============================================================

-- The loai
INSERT INTO TheLoai (TenTheLoai) VALUES 
('Hanh dong'), ('Tinh cam'), ('Kinh di'), ('Hai huoc'),
('Vien tuong'), ('Hoat hinh'), ('Phieu luu'), ('Tam ly');

-- Phim
INSERT INTO Phim (TenPhim, MaTheLoai, ThoiLuong, QuocGia, MoTa, TrangThai) VALUES
('Avengers: Endgame',       5, 181, 'My',       'Bieu tuong sieu anh hung Marvel tro lai', 'Dang chieu'),
('John Wick 4',             1, 169, 'My',       'Sat thu huyen thoai John Wick', 'Dang chieu'),
('Mai',                     2, 130, 'Viet Nam', 'Cau chuyen tinh cam dong dat Sai Gon', 'Dang chieu'),
('Conjuring 4',             3, 112, 'My',       'Am anh kinh hoang tiep tuc', 'Sap chieu'),
('Co May Chien Tranh',      1, 128, 'Han Quoc', 'Phim hanh dong chien tranh', 'Dang chieu'),
('Inside Out 2',            6, 100, 'My',       'Cuoc phieu luu cam xuc moi', 'Dang chieu');

-- Phong chieu
INSERT INTO PhongChieu (TenPhong, SucChua, LoaiPhong, TrangThai) VALUES
('Phong 1', 60, '2D', 'Hoat dong'),
('Phong 2', 80, '3D', 'Hoat dong'),
('Phong 3', 100, 'IMAX', 'Hoat dong'),
('Phong 4', 50, '2D', 'Bao tri');

-- Suat chieu (ngay hien tai va ngay mai)
SET @today = CURDATE();
SET @tomorrow = DATE_ADD(@today, INTERVAL 1 DAY);

INSERT INTO SuatChieu (MaPhim, MaPhong, NgayChieu, GioChieu, GiaVe) VALUES
(1, 3, @today,    '10:00:00', 120000),
(1, 3, @today,    '14:00:00', 120000),
(2, 1, @today,    '09:30:00', 75000),
(2, 1, @today,    '18:00:00', 90000),
(3, 2, @today,    '11:00:00', 85000),
(5, 1, @tomorrow, '10:00:00', 75000),
(6, 2, @tomorrow, '14:30:00', 95000),
(1, 3, @tomorrow, '19:00:00', 130000);

-- Ve mau
INSERT INTO Ve (MaSuat, TenKhachHang, SoDienThoai, MaGhe, GiaVe) VALUES
(1, 'Nguyen Van An',  '0901234567', 'A1', 120000),
(1, 'Nguyen Van An',  '0901234567', 'A2', 120000),
(2, 'Tran Thi Bich',  '0912345678', 'B5', 120000),
(3, 'Le Van Chau',    '0923456789', 'C3', 75000),
(5, 'Pham Thi Dung',  '0934567890', 'A1', 85000);

SELECT '=== Tao co so du lieu QuanLyPhimDB thanh cong! ===' AS Result;
