using System;

namespace QuanLyKyTucXa.Domain.Entities;

public class YeuCauSuaChua
{
    public int MaYeuCau { get; set; }
    public string MoTa { get; set; }
    public DateTime NgayYeuCau { get; set; }
    public string TrangThai { get; set; }

    private SinhVien? _sinhVien;
    private NhanVien? _nhanVien;

    public YeuCauSuaChua(int maYeuCau, string moTa)
    {
        MaYeuCau = maYeuCau;
        MoTa = moTa;
        NgayYeuCau = DateTime.UtcNow;
        TrangThai = "Moi";
    }

    public void GuiYeuCau(SinhVien sv)
    {
        _sinhVien = sv ?? throw new ArgumentNullException(nameof(sv));
        NgayYeuCau = DateTime.UtcNow;
        TrangThai = "DaGui";
    }

    public void PhanCongNhanVien(NhanVien nv)
    {
        _nhanVien = nv ?? throw new ArgumentNullException(nameof(nv));
        TrangThai = "DangXuLy";
    }

    public void HoanThanhYeuCau()
    {
        TrangThai = "HoanThanh";
    }
}
