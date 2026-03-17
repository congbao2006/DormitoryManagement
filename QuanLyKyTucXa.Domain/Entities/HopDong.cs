using System;

namespace QuanLyKyTucXa.Domain.Entities;

public class HopDong
{
    public int MaHopDong { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public decimal TienDatCoc { get; set; }
    public string TrangThai { get; set; }

    private SinhVien? _sinhVien;
    private Phong? _phong;

    public HopDong(int maHopDong, DateTime ngayBatDau, DateTime ngayKetThuc, decimal tienDatCoc)
    {
        MaHopDong = maHopDong;
        NgayBatDau = ngayBatDau;
        NgayKetThuc = ngayKetThuc;
        TienDatCoc = tienDatCoc;
        TrangThai = "HieuLuc";
    }

    public void TaoHopDong(SinhVien sv, Phong phong)
    {
        _sinhVien = sv ?? throw new ArgumentNullException(nameof(sv));
        _phong = phong ?? throw new ArgumentNullException(nameof(phong));

        var hopDongs = sv.XemHopDong();
        if (!hopDongs.Contains(this))
        {
            hopDongs.Add(this);
        }
    }

    public void ThanhToan(ThanhToan thanhToan)
    {
        if (thanhToan == null) throw new ArgumentNullException(nameof(thanhToan));
        if (!thanhToan.KiemTraHopLe()) throw new InvalidOperationException("Thanh toan khong hop le.");
        TrangThai = "DaThanhToan";
    }

    public int TinhSoNgayConLai()
    {
        return Math.Max(0, (NgayKetThuc.Date - DateTime.UtcNow.Date).Days);
    }
}