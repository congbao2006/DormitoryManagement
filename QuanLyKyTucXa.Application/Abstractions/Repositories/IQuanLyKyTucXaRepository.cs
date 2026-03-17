using QuanLyKyTucXa.Domain.Entities;

namespace QuanLyKyTucXa.Application.Abstractions.Repositories;

public interface IQuanLyKyTucXaRepository
{
    Task<List<SinhVien>> LayDanhSachSinhVienAsync(CancellationToken cancellationToken);
    Task<List<Phong>> LayDanhSachPhongAsync(CancellationToken cancellationToken);
    Task<List<HopDong>> LayDanhSachHopDongAsync(CancellationToken cancellationToken);
    Task<List<HoaDon>> LayDanhSachHoaDonAsync(CancellationToken cancellationToken);

    Task<SinhVien?> TimSinhVienAsync(int maSinhVien, CancellationToken cancellationToken);
    Task<Phong?> TimPhongAsync(int maPhong, CancellationToken cancellationToken);
    Task<HopDong?> TimHopDongAsync(int maHopDong, CancellationToken cancellationToken);
    Task<HoaDon?> TimHoaDonAsync(int maHoaDon, CancellationToken cancellationToken);

    Task ThemSinhVienAsync(SinhVien sv, CancellationToken cancellationToken);
    Task CapNhatSinhVienAsync(SinhVien sv, CancellationToken cancellationToken);
    Task XoaSinhVienAsync(int maSinhVien, CancellationToken cancellationToken);

    Task ThemPhongAsync(Phong phong, CancellationToken cancellationToken);
    Task XoaPhongAsync(int maPhong, CancellationToken cancellationToken);

    Task ThemHopDongAsync(HopDong hopDong, CancellationToken cancellationToken);
    Task XoaHopDongAsync(int maHopDong, CancellationToken cancellationToken);

    Task ThemHoaDonAsync(HoaDon hoaDon, CancellationToken cancellationToken);
    Task XoaHoaDonAsync(int maHoaDon, CancellationToken cancellationToken);
}