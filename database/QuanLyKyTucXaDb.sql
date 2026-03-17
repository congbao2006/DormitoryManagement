CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `NhanViens` (
    `MaNhanVien` int NOT NULL AUTO_INCREMENT,
    `HoTen` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `NgaySinh` datetime(6) NOT NULL,
    `GioiTinh` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `SoDienThoai` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `CCCD` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `TrangThai` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_NhanViens` PRIMARY KEY (`MaNhanVien`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `SinhViens` (
    `MaSinhVien` int NOT NULL AUTO_INCREMENT,
    `HoTen` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `NgaySinh` datetime(6) NOT NULL,
    `GioiTinh` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `SoDienThoai` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `CCCD` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `TrangThai` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_SinhViens` PRIMARY KEY (`MaSinhVien`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ThongBaos` (
    `MaThongBao` int NOT NULL AUTO_INCREMENT,
    `TieuDe` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `NoiDung` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
    `NgayTao` datetime(6) NOT NULL,
    CONSTRAINT `PK_ThongBaos` PRIMARY KEY (`MaThongBao`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ToaNhas` (
    `MaToa` int NOT NULL AUTO_INCREMENT,
    `TenToa` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `SoTang` int NOT NULL,
    CONSTRAINT `PK_ToaNhas` PRIMARY KEY (`MaToa`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `YeuCauSuaChuas` (
    `MaYeuCau` int NOT NULL AUTO_INCREMENT,
    `MoTa` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
    `NgayYeuCau` datetime(6) NOT NULL,
    `TrangThai` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `MaNhanVien` int NULL,
    `MaSinhVien` int NOT NULL,
    CONSTRAINT `PK_YeuCauSuaChuas` PRIMARY KEY (`MaYeuCau`),
    CONSTRAINT `FK_YeuCauSuaChuas_NhanViens_MaNhanVien` FOREIGN KEY (`MaNhanVien`) REFERENCES `NhanViens` (`MaNhanVien`) ON DELETE SET NULL,
    CONSTRAINT `FK_YeuCauSuaChuas_SinhViens_MaSinhVien` FOREIGN KEY (`MaSinhVien`) REFERENCES `SinhViens` (`MaSinhVien`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Phongs` (
    `MaPhong` int NOT NULL AUTO_INCREMENT,
    `SoPhong` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `LoaiPhong` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `SucChua` int NOT NULL,
    `GiaPhong` decimal(18,2) NOT NULL,
    `TrangThai` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `MaToa` int NOT NULL,
    CONSTRAINT `PK_Phongs` PRIMARY KEY (`MaPhong`),
    CONSTRAINT `FK_Phongs_ToaNhas_MaToa` FOREIGN KEY (`MaToa`) REFERENCES `ToaNhas` (`MaToa`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `HopDongs` (
    `MaHopDong` int NOT NULL AUTO_INCREMENT,
    `NgayBatDau` datetime(6) NOT NULL,
    `NgayKetThuc` datetime(6) NOT NULL,
    `TienDatCoc` decimal(18,2) NOT NULL,
    `TrangThai` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `MaPhong` int NOT NULL,
    `MaSinhVien` int NOT NULL,
    CONSTRAINT `PK_HopDongs` PRIMARY KEY (`MaHopDong`),
    CONSTRAINT `FK_HopDongs_Phongs_MaPhong` FOREIGN KEY (`MaPhong`) REFERENCES `Phongs` (`MaPhong`) ON DELETE CASCADE,
    CONSTRAINT `FK_HopDongs_SinhViens_MaSinhVien` FOREIGN KEY (`MaSinhVien`) REFERENCES `SinhViens` (`MaSinhVien`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `TaiSans` (
    `MaTaiSan` int NOT NULL AUTO_INCREMENT,
    `TenTaiSan` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `TinhTrang` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `NgayMua` datetime(6) NOT NULL,
    `MaPhong` int NOT NULL,
    CONSTRAINT `PK_TaiSans` PRIMARY KEY (`MaTaiSan`),
    CONSTRAINT `FK_TaiSans_Phongs_MaPhong` FOREIGN KEY (`MaPhong`) REFERENCES `Phongs` (`MaPhong`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `HoaDons` (
    `MaHoaDon` int NOT NULL AUTO_INCREMENT,
    `NgayThanhToan` datetime(6) NOT NULL,
    `TongTien` decimal(18,2) NOT NULL,
    `TrangThai` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `MaHopDong` int NOT NULL,
    `MaPhong` int NOT NULL,
    `MaSinhVien` int NOT NULL,
    CONSTRAINT `PK_HoaDons` PRIMARY KEY (`MaHoaDon`),
    CONSTRAINT `FK_HoaDons_HopDongs_MaHopDong` FOREIGN KEY (`MaHopDong`) REFERENCES `HopDongs` (`MaHopDong`) ON DELETE CASCADE,
    CONSTRAINT `FK_HoaDons_Phongs_MaPhong` FOREIGN KEY (`MaPhong`) REFERENCES `Phongs` (`MaPhong`) ON DELETE CASCADE,
    CONSTRAINT `FK_HoaDons_SinhViens_MaSinhVien` FOREIGN KEY (`MaSinhVien`) REFERENCES `SinhViens` (`MaSinhVien`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `ThanhToans` (
    `MaThanhToan` int NOT NULL AUTO_INCREMENT,
    `NgayThanhToan` datetime(6) NOT NULL,
    `SoTien` decimal(18,2) NOT NULL,
    `PhuongThuc` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `MaHoaDon` int NOT NULL,
    CONSTRAINT `PK_ThanhToans` PRIMARY KEY (`MaThanhToan`),
    CONSTRAINT `FK_ThanhToans_HoaDons_MaHoaDon` FOREIGN KEY (`MaHoaDon`) REFERENCES `HoaDons` (`MaHoaDon`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

INSERT INTO `NhanViens` (`MaNhanVien`, `CCCD`, `Email`, `GioiTinh`, `HoTen`, `NgaySinh`, `SoDienThoai`, `TrangThai`)
VALUES (1, '079090000001', 'ha@kytu.com', 'Nam', 'Nguyen Hoang Ha', TIMESTAMP '1990-03-12 00:00:00', '0912000001', 'DangLamViec'),
(2, '079092000002', 'lan@kytu.com', 'Nu', 'Tran Bich Lan', TIMESTAMP '1992-07-23 00:00:00', '0912000002', 'DangLamViec');

INSERT INTO `SinhViens` (`MaSinhVien`, `CCCD`, `Email`, `GioiTinh`, `HoTen`, `NgaySinh`, `SoDienThoai`, `TrangThai`)
VALUES (1, '001204000001', 'an@kytu.com', 'Nam', 'Nguyen Van An', TIMESTAMP '2004-01-15 00:00:00', '0901000001', 'DangO'),
(2, '001204000002', 'binh@kytu.com', 'Nu', 'Tran Thi Binh', TIMESTAMP '2004-04-20 00:00:00', '0901000002', 'DangO'),
(3, '001203000003', 'cuong@kytu.com', 'Nam', 'Le Quang Cuong', TIMESTAMP '2003-08-10 00:00:00', '0901000003', 'DangO'),
(4, '001205000004', 'duyen@kytu.com', 'Nu', 'Pham Thi Duyen', TIMESTAMP '2005-02-05 00:00:00', '0901000004', 'DangO'),
(5, '001204000005', 'em@kytu.com', 'Nam', 'Vo Minh Em', TIMESTAMP '2004-11-02 00:00:00', '0901000005', 'DangO');

INSERT INTO `ThongBaos` (`MaThongBao`, `NgayTao`, `NoiDung`, `TieuDe`)
VALUES (1, TIMESTAMP '2026-01-01 00:00:00', 'Sinh vien can dong tien truoc ngay 10 hang thang', 'Thong bao dong tien'),
(2, TIMESTAMP '2026-02-01 00:00:00', 'Kiem tra he thong dien nuoc dinh ky', 'Thong bao bao tri');

INSERT INTO `ToaNhas` (`MaToa`, `SoTang`, `TenToa`)
VALUES (1, 5, 'Toa A'),
(2, 6, 'Toa B'),
(3, 4, 'Toa C');

INSERT INTO `Phongs` (`MaPhong`, `GiaPhong`, `LoaiPhong`, `MaToa`, `SoPhong`, `SucChua`, `TrangThai`)
VALUES (101, 1200000.0, 'Nam', 1, 'A101', 4, 'ConCho'),
(102, 1200000.0, 'Nam', 1, 'A102', 4, 'ConCho'),
(103, 1250000.0, 'Nu', 1, 'A103', 4, 'ConCho'),
(201, 1000000.0, 'Nam', 2, 'B201', 6, 'ConCho'),
(202, 1000000.0, 'Nu', 2, 'B202', 6, 'ConCho'),
(203, 1000000.0, 'Nam', 2, 'B203', 6, 'ConCho'),
(204, 1000000.0, 'Nu', 2, 'B204', 6, 'ConCho'),
(301, 900000.0, 'Nam', 3, 'C301', 8, 'ConCho'),
(302, 900000.0, 'Nu', 3, 'C302', 8, 'ConCho'),
(303, 900000.0, 'Nam', 3, 'C303', 8, 'ConCho');

INSERT INTO `YeuCauSuaChuas` (`MaYeuCau`, `MaNhanVien`, `MaSinhVien`, `MoTa`, `NgayYeuCau`, `TrangThai`)
VALUES (1, 1, 1, 'Sua den phong A101', TIMESTAMP '2026-02-01 00:00:00', 'DangXuLy'),
(2, NULL, 3, 'Sua quat phong B201', TIMESTAMP '2026-02-02 00:00:00', 'DaGui');

INSERT INTO `HopDongs` (`MaHopDong`, `MaPhong`, `MaSinhVien`, `NgayBatDau`, `NgayKetThuc`, `TienDatCoc`, `TrangThai`)
VALUES (1, 101, 1, TIMESTAMP '2025-09-01 00:00:00', TIMESTAMP '2026-08-31 00:00:00', 1000000.0, 'HieuLuc'),
(2, 103, 2, TIMESTAMP '2025-09-01 00:00:00', TIMESTAMP '2026-08-31 00:00:00', 1000000.0, 'HieuLuc'),
(3, 201, 3, TIMESTAMP '2025-09-01 00:00:00', TIMESTAMP '2026-08-31 00:00:00', 900000.0, 'HieuLuc');

INSERT INTO `TaiSans` (`MaTaiSan`, `MaPhong`, `NgayMua`, `TenTaiSan`, `TinhTrang`)
VALUES (1, 101, TIMESTAMP '2024-08-01 00:00:00', 'May lanh', 'Tot'),
(2, 103, TIMESTAMP '2024-08-01 00:00:00', 'Quat tran', 'Tot'),
(3, 201, TIMESTAMP '2024-08-01 00:00:00', 'Binh nuoc nong', 'CanBaoTri');

INSERT INTO `HoaDons` (`MaHoaDon`, `MaHopDong`, `MaPhong`, `MaSinhVien`, `NgayThanhToan`, `TongTien`, `TrangThai`)
VALUES (1, 1, 101, 1, TIMESTAMP '2026-01-05 00:00:00', 1200000.0, 'DaThanhToan'),
(2, 2, 103, 2, TIMESTAMP '2026-01-07 00:00:00', 1250000.0, 'DaThanhToan'),
(3, 3, 201, 3, TIMESTAMP '2026-01-10 00:00:00', 1000000.0, 'ChuaThanhToan');

INSERT INTO `ThanhToans` (`MaThanhToan`, `MaHoaDon`, `NgayThanhToan`, `PhuongThuc`, `SoTien`)
VALUES (1, 1, TIMESTAMP '2026-01-05 00:00:00', 'ChuyenKhoan', 1200000.0),
(2, 2, TIMESTAMP '2026-01-07 00:00:00', 'TienMat', 1250000.0),
(3, 3, TIMESTAMP '2026-01-10 00:00:00', 'ChuyenKhoan', 1000000.0);

CREATE INDEX `IX_HoaDons_MaHopDong` ON `HoaDons` (`MaHopDong`);

CREATE INDEX `IX_HoaDons_MaPhong` ON `HoaDons` (`MaPhong`);

CREATE INDEX `IX_HoaDons_MaSinhVien` ON `HoaDons` (`MaSinhVien`);

CREATE INDEX `IX_HopDongs_MaPhong` ON `HopDongs` (`MaPhong`);

CREATE INDEX `IX_HopDongs_MaSinhVien` ON `HopDongs` (`MaSinhVien`);

CREATE UNIQUE INDEX `IX_NhanViens_CCCD` ON `NhanViens` (`CCCD`);

CREATE UNIQUE INDEX `IX_NhanViens_Email` ON `NhanViens` (`Email`);

CREATE INDEX `IX_Phongs_MaToa` ON `Phongs` (`MaToa`);

CREATE UNIQUE INDEX `IX_Phongs_SoPhong` ON `Phongs` (`SoPhong`);

CREATE UNIQUE INDEX `IX_SinhViens_CCCD` ON `SinhViens` (`CCCD`);

CREATE UNIQUE INDEX `IX_SinhViens_Email` ON `SinhViens` (`Email`);

CREATE INDEX `IX_TaiSans_MaPhong` ON `TaiSans` (`MaPhong`);

CREATE INDEX `IX_ThanhToans_MaHoaDon` ON `ThanhToans` (`MaHoaDon`);

CREATE UNIQUE INDEX `IX_ToaNhas_TenToa` ON `ToaNhas` (`TenToa`);

CREATE INDEX `IX_YeuCauSuaChuas_MaNhanVien` ON `YeuCauSuaChuas` (`MaNhanVien`);

CREATE INDEX `IX_YeuCauSuaChuas_MaSinhVien` ON `YeuCauSuaChuas` (`MaSinhVien`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260314135109_InitialCreate', '8.0.2');

COMMIT;

