using QuanLyKyTucXa.Domain.Entities;

namespace QuanLyKyTucXa.Application.Abstractions.Services;

public interface IQuanLyKyTucXaService
{
    Task ThemSinhVienAsync(SinhVien sv, CancellationToken cancellationToken);
    Task XoaSinhVienAsync(int maSinhVien, CancellationToken cancellationToken);
    Task CapNhatSinhVienAsync(SinhVien sv, Phong phong, CancellationToken cancellationToken);

    Task ThemPhongAsync(Phong phong, CancellationToken cancellationToken);
    Task XoaPhongAsync(int maPhong, CancellationToken cancellationToken);

    Task ThemHopDongAsync(HopDong hd, CancellationToken cancellationToken);
    Task XoaHopDongAsync(int maHopDong, CancellationToken cancellationToken);

    Task ThemHoaDonAsync(HoaDon hd, CancellationToken cancellationToken);
    Task XoaHoaDonAsync(int maHoaDon, CancellationToken cancellationToken);

    Task<List<Phong>> LayDanhSachPhongConChoiAsync(CancellationToken cancellationToken);

    Task<List<SinhVien>> LayDanhSachSinhVienAsync(CancellationToken cancellationToken);
    Task<List<HopDong>> LayDanhSachHopDongAsync(CancellationToken cancellationToken);
    Task<List<HoaDon>> LayDanhSachHoaDonAsync(CancellationToken cancellationToken);

    Task QuanLySinhVienAsync(CancellationToken cancellationToken);
    Task QuanLyHopDongAsync(CancellationToken cancellationToken);
    Task QuanLyHoaDonAsync(CancellationToken cancellationToken);
}