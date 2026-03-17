using System;
using System.Collections.Generic;

namespace QuanLyKyTucXa.Domain.Entities;

public class SinhVien
{
    public int MaSinhVien { get; set; }
    public string HoTen { get; set; }
    public DateTime NgaySinh { get; set; }
    public string GioiTinh { get; set; }
    public string SoDienThoai { get; set; }
    public string Email { get; set; }
    public string CCCD { get; set; }
    public string TrangThai { get; set; }

    private readonly List<HopDong> _hopDongs;
    private readonly List<HoaDon> _hoaDons;

    public SinhVien(
        int maSinhVien,
        string hoTen,
        DateTime ngaySinh,
        string gioiTinh,
        string soDienThoai,
        string email,
        string CCCD,
        string trangThai)
    {
        MaSinhVien = maSinhVien;
        HoTen = hoTen;
        NgaySinh = ngaySinh;
        GioiTinh = gioiTinh;
        SoDienThoai = soDienThoai;
        Email = email;
        this.CCCD = CCCD;
        TrangThai = trangThai;
        _hopDongs = new List<HopDong>();
        _hoaDons = new List<HoaDon>();
    }

    public void DangKy()
    {
        TrangThai = "DaDangKy";
    }

    public void CapNhatThongTin()
    {
    }

    public void TraPhong()
    {
        TrangThai = "DaTraPhong";
    }

    public List<HopDong> XemHopDong()
    {
        return _hopDongs;
    }

    public List<HoaDon> XemHoaDon()
    {
        return _hoaDons;
    }
}