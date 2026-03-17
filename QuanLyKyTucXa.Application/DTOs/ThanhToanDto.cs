namespace QuanLyKyTucXa.Application.DTOs;

public sealed class ThanhToanDto
{
    public int MaThanhToan { get; set; }
    public DateTime NgayThanhToan { get; set; }
    public decimal SoTien { get; set; }
    public string PhuongThuc { get; set; } = string.Empty;
}