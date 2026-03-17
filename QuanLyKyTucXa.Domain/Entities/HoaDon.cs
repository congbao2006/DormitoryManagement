using System;

namespace QuanLyKyTucXa.Domain.Entities;

public class HoaDon
{
    public int MaHoaDon { get; set; }
    public DateTime NgayThanhToan { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; }

    private HopDong? _hopDong;
    private SinhVien? _sinhVien;
    private Phong? _phong;

    public HoaDon(int maHoaDon)
    {
        MaHoaDon = maHoaDon;
        NgayThanhToan = DateTime.UtcNow;
        TrangThai = "ChuaThanhToan";
    }

    public void TaoHoaDon(HopDong hd, SinhVien sv, Phong phong)
    {
        _hopDong = hd ?? throw new ArgumentNullException(nameof(hd));
        _sinhVien = sv ?? throw new ArgumentNullException(nameof(sv));
        _phong = phong ?? throw new ArgumentNullException(nameof(phong));
        TongTien = TinhTongTien();

        var hoaDons = sv.XemHoaDon();
        if (!hoaDons.Contains(this))
        {
            hoaDons.Add(this);
        }
    }

    public decimal TinhTongTien()
    {
        if (_phong == null)
        {
            return TongTien;
        }

        return _phong.GiaPhong;
    }

    public void DanhDauDaThanhToan()
    {
        TrangThai = "DaThanhToan";
        NgayThanhToan = DateTime.UtcNow;
    }
}