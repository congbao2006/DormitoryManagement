namespace QuanLyKyTucXa.Application.DTOs;

public sealed class YeuCauSuaChuaDto
{
    public int MaYeuCau { get; set; }
    public string MoTa { get; set; } = string.Empty;
    public DateTime NgayYeuCau { get; set; }
    public string TrangThai { get; set; } = string.Empty;
}