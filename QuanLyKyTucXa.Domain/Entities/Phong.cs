using System;
using System.Collections.Generic;

namespace QuanLyKyTucXa.Domain.Entities;

public class Phong
{
    public int MaPhong { get; set; }
    public string SoPhong { get; set; }
    public string LoaiPhong { get; set; }
    public int SucChua { get; set; }
    public decimal GiaPhong { get; set; }
    public string TrangThai { get; set; }

    private readonly List<SinhVien> _sinhViens;

    public Phong(int maPhong, string soPhong, string loaiPhong, int sucChua, decimal giaPhong)
    {
        MaPhong = maPhong;
        SoPhong = soPhong;
        LoaiPhong = loaiPhong;
        SucChua = sucChua;
        GiaPhong = giaPhong;
        TrangThai = "ConCho";
        _sinhViens = new List<SinhVien>();
    }

    public void ThemSinhVien(SinhVien sv)
    {
        if (sv == null) throw new ArgumentNullException(nameof(sv));
        if (!KiemTraConChoi()) throw new InvalidOperationException("Phong da day.");

        if (!_sinhViens.Contains(sv))
        {
            _sinhViens.Add(sv);
        }

        CapNhatTrangThai();
    }

    public void XoaSinhVien(SinhVien sv)
    {
        if (sv == null) throw new ArgumentNullException(nameof(sv));
        _sinhViens.Remove(sv);
        CapNhatTrangThai();
    }

    public bool KiemTraConChoi()
    {
        return _sinhViens.Count < SucChua;
    }

    public void CapNhatTrangThai()
    {
        TrangThai = KiemTraConChoi() ? "ConCho" : "DaDay";
    }
}