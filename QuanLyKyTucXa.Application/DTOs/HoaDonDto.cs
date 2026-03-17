namespace QuanLyKyTucXa.Application.DTOs;

public sealed class HoaDonDto
{
    public int MaHoaDon { get; set; }
    public DateTime NgayThanhToan { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = string.Empty;
}