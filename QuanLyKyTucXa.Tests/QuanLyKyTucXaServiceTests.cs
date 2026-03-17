using Microsoft.Extensions.Logging.Abstractions;
using QuanLyKyTucXa.Application.Abstractions.Repositories;
using QuanLyKyTucXa.Application.Common.Exceptions;
using QuanLyKyTucXa.Application.Services;
using QuanLyKyTucXa.Domain.Entities;

namespace QuanLyKyTucXa.Tests;

public sealed class QuanLyKyTucXaServiceTests
{
    [Fact]
    public async Task XoaSinhVienAsync_KhiKhongTonTai_ThrowNotFoundException()
    {
        var repository = new FakeQuanLyKyTucXaRepository();
        var service = new QuanLyKyTucXaService(repository, NullLogger<QuanLyKyTucXaService>.Instance);

        await Assert.ThrowsAsync<NotFoundException>(() => service.XoaSinhVienAsync(999, CancellationToken.None));
    }

    [Fact]
    public async Task CapNhatSinhVienAsync_KhiPhongKhongTonTai_ThrowNotFoundException()
    {
        var repository = new FakeQuanLyKyTucXaRepository();
        repository.SinhViens.Add(new SinhVien(1, "SV1", new DateTime(2004, 1, 1), "Nam", "0901", "sv1@test.com", "001", "DangO"));
        var service = new QuanLyKyTucXaService(repository, NullLogger<QuanLyKyTucXaService>.Instance);

        var updatedSinhVien = new SinhVien(1, "SV1 Moi", new DateTime(2004, 1, 1), "Nam", "0909", "sv1moi@test.com", "001", "DangO");
        var phong = new Phong(100, "Z100", "Nam", 2, 1000000m);

        await Assert.ThrowsAsync<NotFoundException>(() => service.CapNhatSinhVienAsync(updatedSinhVien, phong, CancellationToken.None));
    }

    [Fact]
    public async Task CapNhatSinhVienAsync_CapNhatThongTinVaGoiRepository()
    {
        var repository = new FakeQuanLyKyTucXaRepository();
        var currentSinhVien = new SinhVien(1, "SV1", new DateTime(2004, 1, 1), "Nam", "0901", "sv1@test.com", "001", "DangO");
        var phong = new Phong(101, "A101", "Nam", 2, 1200000m);
        repository.SinhViens.Add(currentSinhVien);
        repository.Phongs.Add(phong);

        var service = new QuanLyKyTucXaService(repository, NullLogger<QuanLyKyTucXaService>.Instance);
        var updatedSinhVien = new SinhVien(1, "SV1 Moi", new DateTime(2004, 5, 5), "Nam", "0999", "sv1moi@test.com", "001", "DaDangKy");

        await service.CapNhatSinhVienAsync(updatedSinhVien, phong, CancellationToken.None);

        Assert.True(repository.CapNhatSinhVienDuocGoi);
        Assert.Equal("SV1 Moi", currentSinhVien.HoTen);
        Assert.Equal("sv1moi@test.com", currentSinhVien.Email);
        Assert.Equal("DaDangKy", currentSinhVien.TrangThai);
        Assert.True(phong.KiemTraConChoi());
        Assert.Equal("ConCho", phong.TrangThai);
    }

    [Fact]
    public async Task LayDanhSachPhongConChoiAsync_ChiTraVePhongConCho()
    {
        var repository = new FakeQuanLyKyTucXaRepository();
        var phongConCho = new Phong(1, "A101", "Nam", 2, 1000000m);
        var phongDaDay = new Phong(2, "A102", "Nam", 1, 1000000m);
        phongDaDay.ThemSinhVien(new SinhVien(2, "SV2", new DateTime(2004, 2, 1), "Nam", "0902", "sv2@test.com", "002", "DangO"));
        repository.Phongs.AddRange(new[] { phongConCho, phongDaDay });

        var service = new QuanLyKyTucXaService(repository, NullLogger<QuanLyKyTucXaService>.Instance);

        var result = await service.LayDanhSachPhongConChoiAsync(CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(1, result[0].MaPhong);
    }

    private sealed class FakeQuanLyKyTucXaRepository : IQuanLyKyTucXaRepository
    {
        public List<SinhVien> SinhViens { get; } = new();
        public List<Phong> Phongs { get; } = new();
        public List<HopDong> HopDongs { get; } = new();
        public List<HoaDon> HoaDons { get; } = new();
        public bool CapNhatSinhVienDuocGoi { get; private set; }

        public Task<List<SinhVien>> LayDanhSachSinhVienAsync(CancellationToken cancellationToken) => Task.FromResult(SinhViens);
        public Task<List<Phong>> LayDanhSachPhongAsync(CancellationToken cancellationToken) => Task.FromResult(Phongs);
        public Task<List<HopDong>> LayDanhSachHopDongAsync(CancellationToken cancellationToken) => Task.FromResult(HopDongs);
        public Task<List<HoaDon>> LayDanhSachHoaDonAsync(CancellationToken cancellationToken) => Task.FromResult(HoaDons);

        public Task<SinhVien?> TimSinhVienAsync(int maSinhVien, CancellationToken cancellationToken) => Task.FromResult(SinhViens.FirstOrDefault(x => x.MaSinhVien == maSinhVien));
        public Task<Phong?> TimPhongAsync(int maPhong, CancellationToken cancellationToken) => Task.FromResult(Phongs.FirstOrDefault(x => x.MaPhong == maPhong));
        public Task<HopDong?> TimHopDongAsync(int maHopDong, CancellationToken cancellationToken) => Task.FromResult(HopDongs.FirstOrDefault(x => x.MaHopDong == maHopDong));
        public Task<HoaDon?> TimHoaDonAsync(int maHoaDon, CancellationToken cancellationToken) => Task.FromResult(HoaDons.FirstOrDefault(x => x.MaHoaDon == maHoaDon));

        public Task ThemSinhVienAsync(SinhVien sv, CancellationToken cancellationToken)
        {
            SinhViens.Add(sv);
            return Task.CompletedTask;
        }

        public Task CapNhatSinhVienAsync(SinhVien sv, CancellationToken cancellationToken)
        {
            CapNhatSinhVienDuocGoi = true;
            return Task.CompletedTask;
        }

        public Task XoaSinhVienAsync(int maSinhVien, CancellationToken cancellationToken)
        {
            SinhViens.RemoveAll(x => x.MaSinhVien == maSinhVien);
            return Task.CompletedTask;
        }

        public Task ThemPhongAsync(Phong phong, CancellationToken cancellationToken)
        {
            Phongs.Add(phong);
            return Task.CompletedTask;
        }

        public Task XoaPhongAsync(int maPhong, CancellationToken cancellationToken)
        {
            Phongs.RemoveAll(x => x.MaPhong == maPhong);
            return Task.CompletedTask;
        }

        public Task ThemHopDongAsync(HopDong hopDong, CancellationToken cancellationToken)
        {
            HopDongs.Add(hopDong);
            return Task.CompletedTask;
        }

        public Task XoaHopDongAsync(int maHopDong, CancellationToken cancellationToken)
        {
            HopDongs.RemoveAll(x => x.MaHopDong == maHopDong);
            return Task.CompletedTask;
        }

        public Task ThemHoaDonAsync(HoaDon hoaDon, CancellationToken cancellationToken)
        {
            HoaDons.Add(hoaDon);
            return Task.CompletedTask;
        }

        public Task XoaHoaDonAsync(int maHoaDon, CancellationToken cancellationToken)
        {
            HoaDons.RemoveAll(x => x.MaHoaDon == maHoaDon);
            return Task.CompletedTask;
        }
    }
}