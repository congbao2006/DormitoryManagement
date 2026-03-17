using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyKyTucXa.Domain.Entities;

public class QuanLyKyTucXa
{
    public List<SinhVien> DanhSachSinhVien { get; set; }
    public List<Phong> DanhSachPhong { get; set; }
    public List<HopDong> DanhSachHopDong { get; set; }
    public List<HoaDon> DanhSachHoaDon { get; set; }

    public QuanLyKyTucXa()
    {
        DanhSachSinhVien = new List<SinhVien>();
        DanhSachPhong = new List<Phong>();
        DanhSachHopDong = new List<HopDong>();
        DanhSachHoaDon = new List<HoaDon>();
    }

    public void ThemSinhVien(SinhVien sv)
    {
        if (sv == null) throw new ArgumentNullException(nameof(sv));
        DanhSachSinhVien.Add(sv);
    }

    public void XoaSinhVien(int maSinhVien)
    {
        var sinhVien = DanhSachSinhVien.FirstOrDefault(x => x.MaSinhVien == maSinhVien);
        if (sinhVien != null)
        {
            DanhSachSinhVien.Remove(sinhVien);
        }
    }

    public void CapNhatSinhVien(SinhVien sv, Phong phong)
    {
        if (sv == null) throw new ArgumentNullException(nameof(sv));
        if (phong == null) throw new ArgumentNullException(nameof(phong));

        var sinhVien = DanhSachSinhVien.FirstOrDefault(x => x.MaSinhVien == sv.MaSinhVien);
        if (sinhVien == null)
        {
            DanhSachSinhVien.Add(sv);
            sinhVien = sv;
        }
        else
        {
            sinhVien.HoTen = sv.HoTen;
            sinhVien.NgaySinh = sv.NgaySinh;
            sinhVien.GioiTinh = sv.GioiTinh;
            sinhVien.SoDienThoai = sv.SoDienThoai;
            sinhVien.Email = sv.Email;
            sinhVien.CCCD = sv.CCCD;
            sinhVien.TrangThai = sv.TrangThai;
        }

        phong.ThemSinhVien(sinhVien);
    }

    public void ThemPhong(Phong phong)
    {
        if (phong == null) throw new ArgumentNullException(nameof(phong));
        DanhSachPhong.Add(phong);
    }

    public void XoaPhong(int maPhong)
    {
        var phong = DanhSachPhong.FirstOrDefault(x => x.MaPhong == maPhong);
        if (phong != null)
        {
            DanhSachPhong.Remove(phong);
        }
    }

    public void ThemHopDong(HopDong hd)
    {
        if (hd == null) throw new ArgumentNullException(nameof(hd));
        DanhSachHopDong.Add(hd);
    }

    public void XoaHopDong(int maHopDong)
    {
        var hopDong = DanhSachHopDong.FirstOrDefault(x => x.MaHopDong == maHopDong);
        if (hopDong != null)
        {
            DanhSachHopDong.Remove(hopDong);
        }
    }

    public void ThemHoaDon(HoaDon hd)
    {
        if (hd == null) throw new ArgumentNullException(nameof(hd));
        DanhSachHoaDon.Add(hd);
    }

    public void XoaHoaDon(int maHoaDon)
    {
        var hoaDon = DanhSachHoaDon.FirstOrDefault(x => x.MaHoaDon == maHoaDon);
        if (hoaDon != null)
        {
            DanhSachHoaDon.Remove(hoaDon);
        }
    }

    public List<Phong> LayDanhSachPhongConChoi()
    {
        return DanhSachPhong.Where(p => p.KiemTraConChoi()).ToList();
    }

    public void QuanLySinhVien()
    {
    }

    public void QuanLyHopDong()
    {
    }

    public void QuanLyHoaDon()
    {
    }
}