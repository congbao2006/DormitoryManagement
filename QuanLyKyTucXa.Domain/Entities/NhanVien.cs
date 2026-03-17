using System;

namespace QuanLyKyTucXa.Domain.Entities;

public class NhanVien
{
    public int MaNhanVien { get; set; }
    public string HoTen { get; set; }
    public DateTime NgaySinh { get; set; }
    public string GioiTinh { get; set; }
    public string SoDienThoai { get; set; }
    public string Email { get; set; }
    public string CCCD { get; set; }
    public string TrangThai { get; set; }

    public NhanVien(
        int maNhanVien,
        string hoTen,
        DateTime ngaySinh,
        string gioiTinh,
        string soDienThoai,
        string email,
        string CCCD,
        string trangThai)
    {
        MaNhanVien = maNhanVien;
        HoTen = hoTen;
        NgaySinh = ngaySinh;
        GioiTinh = gioiTinh;
        SoDienThoai = soDienThoai;
        Email = email;
        this.CCCD = CCCD;
        TrangThai = trangThai;
    }

    public void LapHopDong()
    {
    }

    public void LapHoaDon()
    {
    }

    public void QuanLySinhVien()
    {
    }
}