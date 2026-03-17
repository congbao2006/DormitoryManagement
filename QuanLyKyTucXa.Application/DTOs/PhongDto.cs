namespace QuanLyKyTucXa.Application.DTOs;

public sealed class PhongDto
{
    public int MaPhong { get; set; }
    public string SoPhong { get; set; } = string.Empty;
    public string LoaiPhong { get; set; } = string.Empty;
    public int SucChua { get; set; }
    public decimal GiaPhong { get; set; }
    public string TrangThai { get; set; } = string.Empty;
}