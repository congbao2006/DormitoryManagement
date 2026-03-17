namespace QuanLyKyTucXa.Application.DTOs;

public sealed class NhanVienDto
{
    public int MaNhanVien { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public DateTime NgaySinh { get; set; }
    public string GioiTinh { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CCCD { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
}
