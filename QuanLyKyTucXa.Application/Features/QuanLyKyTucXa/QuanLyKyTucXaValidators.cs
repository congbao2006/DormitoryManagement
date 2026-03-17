using FluentValidation;
using QuanLyKyTucXa.Application.DTOs;

namespace QuanLyKyTucXa.Application.Features.QuanLyKyTucXa;

public sealed class SinhVienDtoValidator : AbstractValidator<SinhVienDto>
{
    public SinhVienDtoValidator()
    {
        RuleFor(x => x.MaSinhVien).GreaterThan(0);
        RuleFor(x => x.HoTen).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SoDienThoai).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.CCCD).NotEmpty().MaximumLength(20);
        RuleFor(x => x.TrangThai).NotEmpty().MaximumLength(50);
    }
}

public sealed class PhongDtoValidator : AbstractValidator<PhongDto>
{
    public PhongDtoValidator()
    {
        RuleFor(x => x.MaPhong).GreaterThan(0);
        RuleFor(x => x.SoPhong).NotEmpty().MaximumLength(20);
        RuleFor(x => x.LoaiPhong).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SucChua).GreaterThan(0);
        RuleFor(x => x.GiaPhong).GreaterThan(0);
        RuleFor(x => x.TrangThai).NotEmpty().MaximumLength(50);
    }
}

public sealed class HopDongDtoValidator : AbstractValidator<HopDongDto>
{
    public HopDongDtoValidator()
    {
        RuleFor(x => x.MaHopDong).GreaterThan(0);
        RuleFor(x => x.NgayKetThuc).GreaterThan(x => x.NgayBatDau);
        RuleFor(x => x.TienDatCoc).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TrangThai).NotEmpty().MaximumLength(50);
    }
}

public sealed class HoaDonDtoValidator : AbstractValidator<HoaDonDto>
{
    public HoaDonDtoValidator()
    {
        RuleFor(x => x.MaHoaDon).GreaterThan(0);
        RuleFor(x => x.TongTien).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TrangThai).NotEmpty().MaximumLength(50);
    }
}

public sealed class ThemSinhVienCommandValidator : AbstractValidator<ThemSinhVienCommand>
{
    public ThemSinhVienCommandValidator()
    {
        RuleFor(x => x.SinhVien).NotNull().SetValidator(new SinhVienDtoValidator());
    }
}

public sealed class CapNhatSinhVienCommandValidator : AbstractValidator<CapNhatSinhVienCommand>
{
    public CapNhatSinhVienCommandValidator()
    {
        RuleFor(x => x.SinhVien).NotNull().SetValidator(new SinhVienDtoValidator());
        RuleFor(x => x.MaPhong).GreaterThan(0);
    }
}

public sealed class XoaSinhVienCommandValidator : AbstractValidator<XoaSinhVienCommand>
{
    public XoaSinhVienCommandValidator()
    {
        RuleFor(x => x.MaSinhVien).GreaterThan(0);
    }
}

public sealed class ThemPhongCommandValidator : AbstractValidator<ThemPhongCommand>
{
    public ThemPhongCommandValidator()
    {
        RuleFor(x => x.Phong).NotNull().SetValidator(new PhongDtoValidator());
    }
}

public sealed class ThemHopDongCommandValidator : AbstractValidator<ThemHopDongCommand>
{
    public ThemHopDongCommandValidator()
    {
        RuleFor(x => x.HopDong).NotNull().SetValidator(new HopDongDtoValidator());
    }
}

public sealed class ThemHoaDonCommandValidator : AbstractValidator<ThemHoaDonCommand>
{
    public ThemHoaDonCommandValidator()
    {
        RuleFor(x => x.HoaDon).NotNull().SetValidator(new HoaDonDtoValidator());
    }
}

public sealed class XoaPhongCommandValidator : AbstractValidator<XoaPhongCommand>
{
    public XoaPhongCommandValidator()
    {
        RuleFor(x => x.MaPhong).GreaterThan(0);
    }
}

public sealed class XoaHopDongCommandValidator : AbstractValidator<XoaHopDongCommand>
{
    public XoaHopDongCommandValidator()
    {
        RuleFor(x => x.MaHopDong).GreaterThan(0);
    }
}

public sealed class XoaHoaDonCommandValidator : AbstractValidator<XoaHoaDonCommand>
{
    public XoaHoaDonCommandValidator()
    {
        RuleFor(x => x.MaHoaDon).GreaterThan(0);
    }
}