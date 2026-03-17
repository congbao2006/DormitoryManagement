using AutoMapper;
using QuanLyKyTucXa.Application.DTOs;
using QuanLyKyTucXa.Domain.Entities;

namespace QuanLyKyTucXa.Application.Common.Mappings;

public sealed class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<SinhVien, SinhVienDto>().ReverseMap()
            .ConstructUsing(src => new SinhVien(
                src.MaSinhVien,
                src.HoTen,
                src.NgaySinh,
                src.GioiTinh,
                src.SoDienThoai,
                src.Email,
                src.CCCD,
                src.TrangThai));

        CreateMap<NhanVien, NhanVienDto>().ReverseMap()
            .ConstructUsing(src => new NhanVien(
                src.MaNhanVien,
                src.HoTen,
                src.NgaySinh,
                src.GioiTinh,
                src.SoDienThoai,
                src.Email,
                src.CCCD,
                src.TrangThai));

        CreateMap<ToaNha, ToaNhaDto>().ReverseMap()
            .ConstructUsing(src => new ToaNha(src.MaToa, src.TenToa, src.SoTang));

        CreateMap<YeuCauSuaChua, YeuCauSuaChuaDto>().ReverseMap()
            .ConstructUsing(src => new YeuCauSuaChua(src.MaYeuCau, src.MoTa));

        CreateMap<ThongBao, ThongBaoDto>().ReverseMap()
            .ConstructUsing(src => new ThongBao(src.MaThongBao, src.TieuDe, src.NoiDung));

        CreateMap<HopDong, HopDongDto>().ReverseMap()
            .ConstructUsing(src => new HopDong(src.MaHopDong, src.NgayBatDau, src.NgayKetThuc, src.TienDatCoc));

        CreateMap<HoaDon, HoaDonDto>().ReverseMap()
            .ConstructUsing(src => new HoaDon(src.MaHoaDon));

        CreateMap<Phong, PhongDto>().ReverseMap()
            .ConstructUsing(src => new Phong(src.MaPhong, src.SoPhong, src.LoaiPhong, src.SucChua, src.GiaPhong));

        CreateMap<TaiSan, TaiSanDto>().ReverseMap()
            .ConstructUsing(src => new TaiSan(src.MaTaiSan, src.TenTaiSan, src.NgayMua));

        CreateMap<ThanhToan, ThanhToanDto>().ReverseMap()
            .ConstructUsing(src => new ThanhToan(src.MaThanhToan, src.SoTien, src.PhuongThuc));
    }
}