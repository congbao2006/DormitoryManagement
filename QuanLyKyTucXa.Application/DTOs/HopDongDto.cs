namespace QuanLyKyTucXa.Application.DTOs;

public sealed class HopDongDto
{
    public int MaHopDong { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public decimal TienDatCoc { get; set; }
    public string TrangThai { get; set; } = string.Empty;
}