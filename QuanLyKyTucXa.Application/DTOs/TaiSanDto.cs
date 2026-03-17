namespace QuanLyKyTucXa.Application.DTOs;

public sealed class TaiSanDto
{
    public int MaTaiSan { get; set; }
    public string TenTaiSan { get; set; } = string.Empty;
    public string TinhTrang { get; set; } = string.Empty;
    public DateTime NgayMua { get; set; }
}