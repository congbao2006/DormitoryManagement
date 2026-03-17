namespace QuanLyKyTucXa.Application.DTOs;

public sealed class ThongBaoDto
{
    public int MaThongBao { get; set; }
    public string TieuDe { get; set; } = string.Empty;
    public string NoiDung { get; set; } = string.Empty;
    public DateTime NgayTao { get; set; }
}