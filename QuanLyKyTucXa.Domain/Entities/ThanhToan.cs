using System;

namespace QuanLyKyTucXa.Domain.Entities;

public class ThanhToan
{
    public int MaThanhToan { get; set; }
    public DateTime NgayThanhToan { get; set; }
    public decimal SoTien { get; set; }
    public string PhuongThuc { get; set; }

    public ThanhToan(int maThanhToan, decimal soTien, string phuongThuc)
    {
        MaThanhToan = maThanhToan;
        SoTien = soTien;
        PhuongThuc = phuongThuc;
        NgayThanhToan = DateTime.UtcNow;
    }

    public void ThucHienThanhToan()
    {
        if (!KiemTraHopLe()) throw new InvalidOperationException("Thanh toan khong hop le.");
        NgayThanhToan = DateTime.UtcNow;
    }

    public bool XulyTraLai()
    {
        return SoTien > 0;
    }

    public bool KiemTraHopLe()
    {
        return SoTien > 0 && !string.IsNullOrWhiteSpace(PhuongThuc);
    }
}