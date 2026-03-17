using System;

namespace QuanLyKyTucXa.Domain.Entities;

public class TaiSan
{
    public int MaTaiSan { get; set; }
    public string TenTaiSan { get; set; }
    public string TinhTrang { get; set; }
    public DateTime NgayMua { get; set; }

    private Phong? _phong;

    public TaiSan(int maTaiSan, string tenTaiSan, DateTime ngayMua)
    {
        MaTaiSan = maTaiSan;
        TenTaiSan = tenTaiSan;
        NgayMua = ngayMua;
        TinhTrang = "Tot";
    }

    public void CapNhatTinhTrang()
    {
    }

    public void GanChoPhong(Phong phong)
    {
        _phong = phong ?? throw new ArgumentNullException(nameof(phong));
    }
}