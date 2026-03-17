using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.API.Controllers;
using QuanLyKyTucXa.Domain.Entities;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.Tests;

public sealed class OwnershipAuthorizationTests
{
    [Fact]
    public async Task SinhVien_GetById_KhacChinhMinh_TraVeForbid()
    {
        await using var db = TaoDbContext(nameof(SinhVien_GetById_KhacChinhMinh_TraVeForbid));
        var controller = GanNguoiDung(new SinhViensController(db), "SinhVien", 1);

        var result = await controller.GetById(2, CancellationToken.None);

        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task HopDong_GetAll_SinhVienChiThayCuaMinh()
    {
        await using var db = TaoDbContext(nameof(HopDong_GetAll_SinhVienChiThayCuaMinh));
        var controller = GanNguoiDung(new HopDongsController(db), "SinhVien", 1);

        var result = await controller.GetAll(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var data = Assert.IsType<List<HopDongResponse>>(ok.Value);
        Assert.Single(data);
        Assert.Equal(1, data[0].MaHopDong);
        Assert.Equal(1, data[0].MaSinhVien);
        Assert.Equal(101, data[0].MaPhong);
    }

    [Fact]
    public async Task HoaDon_Create_BangSinhVien_TraVeForbid()
    {
        await using var db = TaoDbContext(nameof(HoaDon_Create_BangSinhVien_TraVeForbid));
        var controller = GanNguoiDung(new HoaDonsController(db), "SinhVien", 1);

        var result = await controller.Create(new HoaDon(99), CancellationToken.None);

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task YeuCau_Create_BangSinhVien_TuGanMaSinhVienTuToken()
    {
        await using var db = TaoDbContext(nameof(YeuCau_Create_BangSinhVien_TuGanMaSinhVienTuToken));
        var controller = GanNguoiDung(new YeuCauSuaChuasController(db), "SinhVien", 1);

        var model = new YeuCauSuaChua(99, "Kiem tra den phong")
        {
            NgayYeuCau = DateTime.UtcNow,
            TrangThai = "DaGui"
        };

        var result = await controller.Create(model, CancellationToken.None);

        Assert.IsType<CreatedAtActionResult>(result);
        var saved = await db.YeuCauSuaChuas.FirstAsync(x => x.MaYeuCau == 99);
        var maSinhVien = db.Entry(saved).Property<int>("MaSinhVien").CurrentValue;
        Assert.Equal(1, maSinhVien);
    }

    [Fact]
    public async Task ThanhToan_GetById_KhongThuocSinhVien_TraVeForbid()
    {
        await using var db = TaoDbContext(nameof(ThanhToan_GetById_KhongThuocSinhVien_TraVeForbid));
        var controller = GanNguoiDung(new ThanhToansController(db), "SinhVien", 1);

        var result = await controller.GetById(2, CancellationToken.None);

        Assert.IsType<ForbidResult>(result.Result);
    }

    private static QuanLyKyTucXaDbContext TaoDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<QuanLyKyTucXaDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var db = new QuanLyKyTucXaDbContext(options);

        var sinhVien1 = new SinhVien(1, "SV1", new DateTime(2004, 1, 1), "Nam", "0901", "sv1@kytu.com", "001", "DangO");
        var sinhVien2 = new SinhVien(2, "SV2", new DateTime(2004, 2, 1), "Nu", "0902", "sv2@kytu.com", "002", "DangO");
        db.SinhViens.AddRange(sinhVien1, sinhVien2);

        var hopDong1 = new HopDong(1, new DateTime(2025, 9, 1), new DateTime(2026, 8, 31), 1000000m);
        var hopDong2 = new HopDong(2, new DateTime(2025, 9, 1), new DateTime(2026, 8, 31), 1000000m);
        db.HopDongs.AddRange(hopDong1, hopDong2);

        var hoaDon1 = new HoaDon(1) { TongTien = 1000000m, TrangThai = "DaThanhToan", NgayThanhToan = new DateTime(2026, 1, 1) };
        var hoaDon2 = new HoaDon(2) { TongTien = 1200000m, TrangThai = "ChuaThanhToan", NgayThanhToan = new DateTime(2026, 1, 2) };
        db.HoaDons.AddRange(hoaDon1, hoaDon2);

        var thanhToan1 = new ThanhToan(1, 1000000m, "ChuyenKhoan") { NgayThanhToan = new DateTime(2026, 1, 3) };
        var thanhToan2 = new ThanhToan(2, 1200000m, "TienMat") { NgayThanhToan = new DateTime(2026, 1, 4) };
        db.ThanhToans.AddRange(thanhToan1, thanhToan2);

        db.Entry(hopDong1).Property("MaSinhVien").CurrentValue = 1;
        db.Entry(hopDong1).Property("MaPhong").CurrentValue = 101;
        db.Entry(hopDong2).Property("MaSinhVien").CurrentValue = 2;
        db.Entry(hopDong2).Property("MaPhong").CurrentValue = 102;

        db.Entry(hoaDon1).Property("MaSinhVien").CurrentValue = 1;
        db.Entry(hoaDon1).Property("MaHopDong").CurrentValue = 1;
        db.Entry(hoaDon1).Property("MaPhong").CurrentValue = 101;
        db.Entry(hoaDon2).Property("MaSinhVien").CurrentValue = 2;
        db.Entry(hoaDon2).Property("MaHopDong").CurrentValue = 2;
        db.Entry(hoaDon2).Property("MaPhong").CurrentValue = 102;

        db.Entry(thanhToan1).Property("MaHoaDon").CurrentValue = 1;
        db.Entry(thanhToan2).Property("MaHoaDon").CurrentValue = 2;

        db.SaveChanges();
        return db;
    }

    private static TController GanNguoiDung<TController>(TController controller, string role, int userId)
        where TController : ControllerBase
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "TestAuth");

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            }
        };

        return controller;
    }
}
