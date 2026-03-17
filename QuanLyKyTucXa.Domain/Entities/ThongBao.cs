using System;

namespace QuanLyKyTucXa.Domain.Entities;

public class ThongBao
{
    public int MaThongBao { get; set; }
    public string TieuDe { get; set; }
    public string NoiDung { get; set; }
    public DateTime NgayTao { get; set; }

    public ThongBao(int maThongBao, string tieuDe, string noiDung)
    {
        MaThongBao = maThongBao;
        TieuDe = tieuDe;
        NoiDung = noiDung;
        NgayTao = DateTime.UtcNow;
    }

    public void GuiChoSinhVien(SinhVien sv)
    {
        if (sv == null) throw new ArgumentNullException(nameof(sv));
    }

    public void GuiChoNhanVien(NhanVien nv)
    {
        if (nv == null) throw new ArgumentNullException(nameof(nv));
    }
}
