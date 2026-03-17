using Microsoft.Extensions.Logging;
using QuanLyKyTucXa.Application.Abstractions.Repositories;
using QuanLyKyTucXa.Application.Abstractions.Services;
using QuanLyKyTucXa.Application.Common.Exceptions;
using QuanLyKyTucXa.Domain.Entities;

namespace QuanLyKyTucXa.Application.Services;

public sealed class QuanLyKyTucXaService : IQuanLyKyTucXaService
{
    private readonly IQuanLyKyTucXaRepository _repository;
    private readonly ILogger<QuanLyKyTucXaService> _logger;

    public QuanLyKyTucXaService(IQuanLyKyTucXaRepository repository, ILogger<QuanLyKyTucXaService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task ThemSinhVienAsync(SinhVien sv, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Them sinh vien {MaSinhVien}", sv.MaSinhVien);
        await _repository.ThemSinhVienAsync(sv, cancellationToken);
    }

    public async Task XoaSinhVienAsync(int maSinhVien, CancellationToken cancellationToken)
    {
        var current = await _repository.TimSinhVienAsync(maSinhVien, cancellationToken);
        if (current == null)
        {
            throw new NotFoundException($"Khong tim thay sinh vien {maSinhVien}.");
        }

        _logger.LogInformation("Xoa sinh vien {MaSinhVien}", maSinhVien);
        await _repository.XoaSinhVienAsync(maSinhVien, cancellationToken);
    }

    public async Task CapNhatSinhVienAsync(SinhVien sv, Phong phong, CancellationToken cancellationToken)
    {
        var current = await _repository.TimSinhVienAsync(sv.MaSinhVien, cancellationToken);
        if (current == null)
        {
            throw new NotFoundException($"Khong tim thay sinh vien {sv.MaSinhVien}.");
        }

        var currentPhong = await _repository.TimPhongAsync(phong.MaPhong, cancellationToken);
        if (currentPhong == null)
        {
            throw new NotFoundException($"Khong tim thay phong {phong.MaPhong}.");
        }

        current.HoTen = sv.HoTen;
        current.NgaySinh = sv.NgaySinh;
        current.GioiTinh = sv.GioiTinh;
        current.SoDienThoai = sv.SoDienThoai;
        current.Email = sv.Email;
        current.CCCD = sv.CCCD;
        current.TrangThai = sv.TrangThai;
        currentPhong.ThemSinhVien(current);

        _logger.LogInformation("Cap nhat sinh vien {MaSinhVien} vao phong {MaPhong}", sv.MaSinhVien, phong.MaPhong);
        await _repository.CapNhatSinhVienAsync(current, cancellationToken);
    }

    public async Task ThemPhongAsync(Phong phong, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Them phong {MaPhong}", phong.MaPhong);
        await _repository.ThemPhongAsync(phong, cancellationToken);
    }

    public async Task XoaPhongAsync(int maPhong, CancellationToken cancellationToken)
    {
        var phong = await _repository.TimPhongAsync(maPhong, cancellationToken);
        if (phong == null)
        {
            throw new NotFoundException($"Khong tim thay phong {maPhong}.");
        }

        _logger.LogInformation("Xoa phong {MaPhong}", maPhong);
        await _repository.XoaPhongAsync(maPhong, cancellationToken);
    }

    public async Task ThemHopDongAsync(HopDong hd, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Them hop dong {MaHopDong}", hd.MaHopDong);
        await _repository.ThemHopDongAsync(hd, cancellationToken);
    }

    public async Task XoaHopDongAsync(int maHopDong, CancellationToken cancellationToken)
    {
        var hopDong = await _repository.TimHopDongAsync(maHopDong, cancellationToken);
        if (hopDong == null)
        {
            throw new NotFoundException($"Khong tim thay hop dong {maHopDong}.");
        }

        _logger.LogInformation("Xoa hop dong {MaHopDong}", maHopDong);
        await _repository.XoaHopDongAsync(maHopDong, cancellationToken);
    }

    public async Task ThemHoaDonAsync(HoaDon hd, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Them hoa don {MaHoaDon}", hd.MaHoaDon);
        await _repository.ThemHoaDonAsync(hd, cancellationToken);
    }

    public async Task XoaHoaDonAsync(int maHoaDon, CancellationToken cancellationToken)
    {
        var hoaDon = await _repository.TimHoaDonAsync(maHoaDon, cancellationToken);
        if (hoaDon == null)
        {
            throw new NotFoundException($"Khong tim thay hoa don {maHoaDon}.");
        }

        _logger.LogInformation("Xoa hoa don {MaHoaDon}", maHoaDon);
        await _repository.XoaHoaDonAsync(maHoaDon, cancellationToken);
    }

    public async Task<List<Phong>> LayDanhSachPhongConChoiAsync(CancellationToken cancellationToken)
    {
        var phongs = await _repository.LayDanhSachPhongAsync(cancellationToken);
        return phongs.Where(p => p.KiemTraConChoi()).ToList();
    }

    public Task<List<SinhVien>> LayDanhSachSinhVienAsync(CancellationToken cancellationToken)
    {
        return _repository.LayDanhSachSinhVienAsync(cancellationToken);
    }

    public Task<List<HopDong>> LayDanhSachHopDongAsync(CancellationToken cancellationToken)
    {
        return _repository.LayDanhSachHopDongAsync(cancellationToken);
    }

    public Task<List<HoaDon>> LayDanhSachHoaDonAsync(CancellationToken cancellationToken)
    {
        return _repository.LayDanhSachHoaDonAsync(cancellationToken);
    }

    public Task QuanLySinhVienAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Thuc hien nghiep vu QuanLySinhVien");
        return Task.CompletedTask;
    }

    public Task QuanLyHopDongAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Thuc hien nghiep vu QuanLyHopDong");
        return Task.CompletedTask;
    }

    public Task QuanLyHoaDonAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Thuc hien nghiep vu QuanLyHoaDon");
        return Task.CompletedTask;
    }
}