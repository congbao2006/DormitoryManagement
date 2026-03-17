using System;
using System.Collections.Generic;

namespace QuanLyKyTucXa.Domain.Entities;

public class ToaNha
{
    public int MaToa { get; set; }
    public string TenToa { get; set; }
    public int SoTang { get; set; }

    private readonly List<Phong> _phongs;

    public ToaNha(int maToa, string tenToa, int soTang)
    {
        MaToa = maToa;
        TenToa = tenToa;
        SoTang = soTang;
        _phongs = new List<Phong>();
    }

    public void ThemPhong(Phong phong)
    {
        if (phong == null) throw new ArgumentNullException(nameof(phong));
        _phongs.Add(phong);
    }

    public void XoaPhong(Phong phong)
    {
        if (phong == null) throw new ArgumentNullException(nameof(phong));
        _phongs.Remove(phong);
    }

    public List<Phong> LayDanhSachPhong()
    {
        return _phongs;
    }
}