using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.Domain.Entities;

namespace QuanLyKyTucXa.Infrastructure.Persistence;

public sealed class QuanLyKyTucXaDbContext : DbContext
{
    public QuanLyKyTucXaDbContext(DbContextOptions<QuanLyKyTucXaDbContext> options) : base(options)
    {
    }

    public DbSet<SinhVien> SinhViens => Set<SinhVien>();
    public DbSet<NhanVien> NhanViens => Set<NhanVien>();
    public DbSet<ToaNha> ToaNhas => Set<ToaNha>();
    public DbSet<Phong> Phongs => Set<Phong>();
    public DbSet<HopDong> HopDongs => Set<HopDong>();
    public DbSet<HoaDon> HoaDons => Set<HoaDon>();
    public DbSet<ThongBao> ThongBaos => Set<ThongBao>();
    public DbSet<YeuCauSuaChua> YeuCauSuaChuas => Set<YeuCauSuaChua>();
    public DbSet<TaiSan> TaiSans => Set<TaiSan>();
    public DbSet<ThanhToan> ThanhToans => Set<ThanhToan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(x => x.MaSinhVien);
            entity.Property(x => x.HoTen).HasMaxLength(200).IsRequired();
            entity.Property(x => x.GioiTinh).HasMaxLength(20).IsRequired();
            entity.Property(x => x.SoDienThoai).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.Property(x => x.CCCD).HasMaxLength(20).IsRequired();
            entity.Property(x => x.TrangThai).HasMaxLength(50).IsRequired();

            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.CCCD).IsUnique();
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(x => x.MaNhanVien);
            entity.Property(x => x.HoTen).HasMaxLength(200).IsRequired();
            entity.Property(x => x.GioiTinh).HasMaxLength(20).IsRequired();
            entity.Property(x => x.SoDienThoai).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.Property(x => x.CCCD).HasMaxLength(20).IsRequired();
            entity.Property(x => x.TrangThai).HasMaxLength(50).IsRequired();

            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.CCCD).IsUnique();
        });

        modelBuilder.Entity<ToaNha>(entity =>
        {
            entity.HasKey(x => x.MaToa);
            entity.Property(x => x.TenToa).HasMaxLength(100).IsRequired();
            entity.HasIndex(x => x.TenToa).IsUnique();
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(x => x.MaPhong);
            entity.Property(x => x.SoPhong).HasMaxLength(20).IsRequired();
            entity.Property(x => x.LoaiPhong).HasMaxLength(100).IsRequired();
            entity.Property(x => x.GiaPhong).HasColumnType("decimal(18,2)");
            entity.Property(x => x.TrangThai).HasMaxLength(50).IsRequired();
            entity.Property<int>("MaToa");

            entity.HasIndex(x => x.SoPhong).IsUnique();

            entity.HasOne<ToaNha>()
                .WithMany()
                .HasForeignKey("MaToa")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HopDong>(entity =>
        {
            entity.HasKey(x => x.MaHopDong);
            entity.Property(x => x.TienDatCoc).HasColumnType("decimal(18,2)");
            entity.Property(x => x.TrangThai).HasMaxLength(50).IsRequired();
            entity.Property<int>("MaSinhVien");
            entity.Property<int>("MaPhong");

            entity.HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey("MaSinhVien")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Phong>()
                .WithMany()
                .HasForeignKey("MaPhong")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex("MaSinhVien");
            entity.HasIndex("MaPhong");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(x => x.MaHoaDon);
            entity.Property(x => x.TongTien).HasColumnType("decimal(18,2)");
            entity.Property(x => x.TrangThai).HasMaxLength(50).IsRequired();
            entity.Property<int>("MaHopDong");
            entity.Property<int>("MaSinhVien");
            entity.Property<int>("MaPhong");

            entity.HasOne<HopDong>()
                .WithMany()
                .HasForeignKey("MaHopDong")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey("MaSinhVien")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Phong>()
                .WithMany()
                .HasForeignKey("MaPhong")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<YeuCauSuaChua>(entity =>
        {
            entity.HasKey(x => x.MaYeuCau);
            entity.Property(x => x.MoTa).HasMaxLength(500).IsRequired();
            entity.Property(x => x.TrangThai).HasMaxLength(50).IsRequired();
            entity.Property<int>("MaSinhVien");
            entity.Property<int?>("MaNhanVien");

            entity.HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey("MaSinhVien")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<NhanVien>()
                .WithMany()
                .HasForeignKey("MaNhanVien")
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(x => x.MaThongBao);
            entity.Property(x => x.TieuDe).HasMaxLength(200).IsRequired();
            entity.Property(x => x.NoiDung).HasMaxLength(1000).IsRequired();
        });

        modelBuilder.Entity<TaiSan>(entity =>
        {
            entity.HasKey(x => x.MaTaiSan);
            entity.Property(x => x.TenTaiSan).HasMaxLength(200).IsRequired();
            entity.Property(x => x.TinhTrang).HasMaxLength(50).IsRequired();
            entity.Property<int>("MaPhong");

            entity.HasOne<Phong>()
                .WithMany()
                .HasForeignKey("MaPhong")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(x => x.MaThanhToan);
            entity.Property(x => x.SoTien).HasColumnType("decimal(18,2)");
            entity.Property(x => x.PhuongThuc).HasMaxLength(50).IsRequired();
            entity.Property<int>("MaHoaDon");

            entity.HasOne<HoaDon>()
                .WithMany()
                .HasForeignKey("MaHoaDon")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex("MaHoaDon");
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToaNha>().HasData(
            new { MaToa = 1, TenToa = "Toa A", SoTang = 5 },
            new { MaToa = 2, TenToa = "Toa B", SoTang = 6 },
            new { MaToa = 3, TenToa = "Toa C", SoTang = 4 });

        modelBuilder.Entity<Phong>().HasData(
            new { MaPhong = 101, SoPhong = "A101", LoaiPhong = "Nam", SucChua = 4, GiaPhong = 1200000m, TrangThai = "ConCho", MaToa = 1 },
            new { MaPhong = 102, SoPhong = "A102", LoaiPhong = "Nam", SucChua = 4, GiaPhong = 1200000m, TrangThai = "ConCho", MaToa = 1 },
            new { MaPhong = 103, SoPhong = "A103", LoaiPhong = "Nu", SucChua = 4, GiaPhong = 1250000m, TrangThai = "ConCho", MaToa = 1 },
            new { MaPhong = 201, SoPhong = "B201", LoaiPhong = "Nam", SucChua = 6, GiaPhong = 1000000m, TrangThai = "ConCho", MaToa = 2 },
            new { MaPhong = 202, SoPhong = "B202", LoaiPhong = "Nu", SucChua = 6, GiaPhong = 1000000m, TrangThai = "ConCho", MaToa = 2 },
            new { MaPhong = 203, SoPhong = "B203", LoaiPhong = "Nam", SucChua = 6, GiaPhong = 1000000m, TrangThai = "ConCho", MaToa = 2 },
            new { MaPhong = 204, SoPhong = "B204", LoaiPhong = "Nu", SucChua = 6, GiaPhong = 1000000m, TrangThai = "ConCho", MaToa = 2 },
            new { MaPhong = 301, SoPhong = "C301", LoaiPhong = "Nam", SucChua = 8, GiaPhong = 900000m, TrangThai = "ConCho", MaToa = 3 },
            new { MaPhong = 302, SoPhong = "C302", LoaiPhong = "Nu", SucChua = 8, GiaPhong = 900000m, TrangThai = "ConCho", MaToa = 3 },
            new { MaPhong = 303, SoPhong = "C303", LoaiPhong = "Nam", SucChua = 8, GiaPhong = 900000m, TrangThai = "ConCho", MaToa = 3 });

        modelBuilder.Entity<SinhVien>().HasData(
            new { MaSinhVien = 1, HoTen = "Nguyen Van An", NgaySinh = new DateTime(2004, 1, 15), GioiTinh = "Nam", SoDienThoai = "0901000001", Email = "an@kytu.com", CCCD = "001204000001", TrangThai = "DangO" },
            new { MaSinhVien = 2, HoTen = "Tran Thi Binh", NgaySinh = new DateTime(2004, 4, 20), GioiTinh = "Nu", SoDienThoai = "0901000002", Email = "binh@kytu.com", CCCD = "001204000002", TrangThai = "DangO" },
            new { MaSinhVien = 3, HoTen = "Le Quang Cuong", NgaySinh = new DateTime(2003, 8, 10), GioiTinh = "Nam", SoDienThoai = "0901000003", Email = "cuong@kytu.com", CCCD = "001203000003", TrangThai = "DangO" },
            new { MaSinhVien = 4, HoTen = "Pham Thi Duyen", NgaySinh = new DateTime(2005, 2, 5), GioiTinh = "Nu", SoDienThoai = "0901000004", Email = "duyen@kytu.com", CCCD = "001205000004", TrangThai = "DangO" },
            new { MaSinhVien = 5, HoTen = "Vo Minh Em", NgaySinh = new DateTime(2004, 11, 2), GioiTinh = "Nam", SoDienThoai = "0901000005", Email = "em@kytu.com", CCCD = "001204000005", TrangThai = "DangO" });

        modelBuilder.Entity<NhanVien>().HasData(
            new { MaNhanVien = 1, HoTen = "Nguyen Hoang Ha", NgaySinh = new DateTime(1990, 3, 12), GioiTinh = "Nam", SoDienThoai = "0912000001", Email = "ha@kytu.com", CCCD = "079090000001", TrangThai = "DangLamViec" },
            new { MaNhanVien = 2, HoTen = "Tran Bich Lan", NgaySinh = new DateTime(1992, 7, 23), GioiTinh = "Nu", SoDienThoai = "0912000002", Email = "lan@kytu.com", CCCD = "079092000002", TrangThai = "DangLamViec" });

        modelBuilder.Entity<HopDong>().HasData(
            new { MaHopDong = 1, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2026, 8, 31), TienDatCoc = 1000000m, TrangThai = "HieuLuc", MaSinhVien = 1, MaPhong = 101 },
            new { MaHopDong = 2, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2026, 8, 31), TienDatCoc = 1000000m, TrangThai = "HieuLuc", MaSinhVien = 2, MaPhong = 103 },
            new { MaHopDong = 3, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2026, 8, 31), TienDatCoc = 900000m, TrangThai = "HieuLuc", MaSinhVien = 3, MaPhong = 201 });

        modelBuilder.Entity<HoaDon>().HasData(
            new { MaHoaDon = 1, NgayThanhToan = new DateTime(2026, 1, 5), TongTien = 1200000m, TrangThai = "DaThanhToan", MaHopDong = 1, MaSinhVien = 1, MaPhong = 101 },
            new { MaHoaDon = 2, NgayThanhToan = new DateTime(2026, 1, 7), TongTien = 1250000m, TrangThai = "DaThanhToan", MaHopDong = 2, MaSinhVien = 2, MaPhong = 103 },
            new { MaHoaDon = 3, NgayThanhToan = new DateTime(2026, 1, 10), TongTien = 1000000m, TrangThai = "ChuaThanhToan", MaHopDong = 3, MaSinhVien = 3, MaPhong = 201 });

        modelBuilder.Entity<YeuCauSuaChua>().HasData(
            new { MaYeuCau = 1, MoTa = "Sua den phong A101", NgayYeuCau = new DateTime(2026, 2, 1), TrangThai = "DangXuLy", MaSinhVien = 1, MaNhanVien = 1 },
            new { MaYeuCau = 2, MoTa = "Sua quat phong B201", NgayYeuCau = new DateTime(2026, 2, 2), TrangThai = "DaGui", MaSinhVien = 3, MaNhanVien = (int?)null });

        modelBuilder.Entity<ThongBao>().HasData(
            new { MaThongBao = 1, TieuDe = "Thong bao dong tien", NoiDung = "Sinh vien can dong tien truoc ngay 10 hang thang", NgayTao = new DateTime(2026, 1, 1) },
            new { MaThongBao = 2, TieuDe = "Thong bao bao tri", NoiDung = "Kiem tra he thong dien nuoc dinh ky", NgayTao = new DateTime(2026, 2, 1) });

        modelBuilder.Entity<TaiSan>().HasData(
            new { MaTaiSan = 1, TenTaiSan = "May lanh", TinhTrang = "Tot", NgayMua = new DateTime(2024, 8, 1), MaPhong = 101 },
            new { MaTaiSan = 2, TenTaiSan = "Quat tran", TinhTrang = "Tot", NgayMua = new DateTime(2024, 8, 1), MaPhong = 103 },
            new { MaTaiSan = 3, TenTaiSan = "Binh nuoc nong", TinhTrang = "CanBaoTri", NgayMua = new DateTime(2024, 8, 1), MaPhong = 201 });

        modelBuilder.Entity<ThanhToan>().HasData(
            new { MaThanhToan = 1, NgayThanhToan = new DateTime(2026, 1, 5), SoTien = 1200000m, PhuongThuc = "ChuyenKhoan", MaHoaDon = 1 },
            new { MaThanhToan = 2, NgayThanhToan = new DateTime(2026, 1, 7), SoTien = 1250000m, PhuongThuc = "TienMat", MaHoaDon = 2 },
            new { MaThanhToan = 3, NgayThanhToan = new DateTime(2026, 1, 10), SoTien = 1000000m, PhuongThuc = "ChuyenKhoan", MaHoaDon = 3 });
    }
}