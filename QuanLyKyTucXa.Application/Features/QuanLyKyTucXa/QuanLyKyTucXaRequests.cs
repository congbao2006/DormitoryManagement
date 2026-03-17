using MediatR;
using QuanLyKyTucXa.Application.DTOs;

namespace QuanLyKyTucXa.Application.Features.QuanLyKyTucXa;

public sealed record ThemSinhVienCommand(SinhVienDto SinhVien) : IRequest;
public sealed record XoaSinhVienCommand(int MaSinhVien) : IRequest;
public sealed record CapNhatSinhVienCommand(SinhVienDto SinhVien, int MaPhong) : IRequest;

public sealed record ThemPhongCommand(PhongDto Phong) : IRequest;
public sealed record XoaPhongCommand(int MaPhong) : IRequest;

public sealed record ThemHopDongCommand(HopDongDto HopDong) : IRequest;
public sealed record XoaHopDongCommand(int MaHopDong) : IRequest;

public sealed record ThemHoaDonCommand(HoaDonDto HoaDon) : IRequest;
public sealed record XoaHoaDonCommand(int MaHoaDon) : IRequest;

public sealed record QuanLySinhVienCommand : IRequest;
public sealed record QuanLyHopDongCommand : IRequest;
public sealed record QuanLyHoaDonCommand : IRequest;

public sealed record LayDanhSachPhongConChoiQuery : IRequest<List<PhongDto>>;
public sealed record LayDanhSachSinhVienQuery : IRequest<List<SinhVienDto>>;
public sealed record LayDanhSachHopDongQuery : IRequest<List<HopDongDto>>;
public sealed record LayDanhSachHoaDonQuery : IRequest<List<HoaDonDto>>;