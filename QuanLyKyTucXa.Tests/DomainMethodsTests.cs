using QuanLyKyTucXa.Domain.Entities;

namespace QuanLyKyTucXa.Tests;

public sealed class DomainMethodsTests
{
    [Fact]
    public void Phong_KiemTraConChoi_TraVeTrueKhiChuaDay()
    {
        var phong = new Phong(1, "A101", "Nam", 2, 1200000m);
        var sv = new SinhVien(1, "A", DateTime.UtcNow.AddYears(-20), "Nam", "0901", "a@a.com", "001", "DangO");

        phong.ThemSinhVien(sv);

        Assert.True(phong.KiemTraConChoi());
    }

    [Fact]
    public void Phong_CapNhatTrangThai_TraVeDaDayKhiHetCho()
    {
        var phong = new Phong(1, "A101", "Nam", 1, 1200000m);
        var sv = new SinhVien(1, "A", DateTime.UtcNow.AddYears(-20), "Nam", "0901", "a@a.com", "001", "DangO");

        phong.ThemSinhVien(sv);

        Assert.Equal("DaDay", phong.TrangThai);
    }

    [Fact]
    public void HopDong_TinhSoNgayConLai_KhongAm()
    {
        var hopDong = new HopDong(1, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddDays(-1), 1000000m);

        var soNgay = hopDong.TinhSoNgayConLai();

        Assert.True(soNgay >= 0);
    }

    [Fact]
    public void ThanhToan_KiemTraHopLe_TrueKhiDuDieuKien()
    {
        var thanhToan = new ThanhToan(1, 1000000m, "ChuyenKhoan");

        Assert.True(thanhToan.KiemTraHopLe());
    }

    [Fact]
    public void QuanLyKyTucXa_LayDanhSachPhongConChoi_LocDungPhong()
    {
        var ql = new QuanLyKyTucXa.Domain.Entities.QuanLyKyTucXa();
        var phongConCho = new Phong(1, "A101", "Nam", 2, 1200000m);
        var phongDaDay = new Phong(2, "A102", "Nam", 1, 1200000m);
        phongDaDay.ThemSinhVien(new SinhVien(1, "A", DateTime.UtcNow.AddYears(-20), "Nam", "0901", "a1@a.com", "0011", "DangO"));

        ql.ThemPhong(phongConCho);
        ql.ThemPhong(phongDaDay);

        var ketQua = ql.LayDanhSachPhongConChoi();

        Assert.Single(ketQua);
        Assert.Equal(1, ketQua[0].MaPhong);
    }
}