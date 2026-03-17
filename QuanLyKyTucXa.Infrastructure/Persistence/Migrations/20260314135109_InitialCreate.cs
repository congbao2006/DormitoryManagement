using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyKyTucXa.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HoTen = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgaySinh = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GioiTinh = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SoDienThoai = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CCCD = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TrangThai = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNhanVien);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SinhViens",
                columns: table => new
                {
                    MaSinhVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HoTen = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgaySinh = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GioiTinh = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SoDienThoai = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CCCD = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TrangThai = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhViens", x => x.MaSinhVien);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ThongBaos",
                columns: table => new
                {
                    MaThongBao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TieuDe = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NoiDung = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgayTao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaos", x => x.MaThongBao);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ToaNhas",
                columns: table => new
                {
                    MaToa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenToa = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SoTang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToaNhas", x => x.MaToa);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "YeuCauSuaChuas",
                columns: table => new
                {
                    MaYeuCau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MoTa = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgayYeuCau = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TrangThai = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaNhanVien = table.Column<int>(type: "int", nullable: true),
                    MaSinhVien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauSuaChuas", x => x.MaYeuCau);
                    table.ForeignKey(
                        name: "FK_YeuCauSuaChuas_NhanViens_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_YeuCauSuaChuas_SinhViens_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "SinhViens",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Phongs",
                columns: table => new
                {
                    MaPhong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SoPhong = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoaiPhong = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SucChua = table.Column<int>(type: "int", nullable: false),
                    GiaPhong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaToa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phongs", x => x.MaPhong);
                    table.ForeignKey(
                        name: "FK_Phongs_ToaNhas_MaToa",
                        column: x => x.MaToa,
                        principalTable: "ToaNhas",
                        principalColumn: "MaToa",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HopDongs",
                columns: table => new
                {
                    MaHopDong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NgayBatDau = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TienDatCoc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaPhong = table.Column<int>(type: "int", nullable: false),
                    MaSinhVien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopDongs", x => x.MaHopDong);
                    table.ForeignKey(
                        name: "FK_HopDongs_Phongs_MaPhong",
                        column: x => x.MaPhong,
                        principalTable: "Phongs",
                        principalColumn: "MaPhong",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HopDongs_SinhViens_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "SinhViens",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaiSans",
                columns: table => new
                {
                    MaTaiSan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenTaiSan = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TinhTrang = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgayMua = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MaPhong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiSans", x => x.MaTaiSan);
                    table.ForeignKey(
                        name: "FK_TaiSans_Phongs_MaPhong",
                        column: x => x.MaPhong,
                        principalTable: "Phongs",
                        principalColumn: "MaPhong",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    MaHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaHopDong = table.Column<int>(type: "int", nullable: false),
                    MaPhong = table.Column<int>(type: "int", nullable: false),
                    MaSinhVien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDons_HopDongs_MaHopDong",
                        column: x => x.MaHopDong,
                        principalTable: "HopDongs",
                        principalColumn: "MaHopDong",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDons_Phongs_MaPhong",
                        column: x => x.MaPhong,
                        principalTable: "Phongs",
                        principalColumn: "MaPhong",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDons_SinhViens_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "SinhViens",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ThanhToans",
                columns: table => new
                {
                    MaThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhuongThuc = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaHoaDon = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToans", x => x.MaThanhToan);
                    table.ForeignKey(
                        name: "FK_ThanhToans_HoaDons_MaHoaDon",
                        column: x => x.MaHoaDon,
                        principalTable: "HoaDons",
                        principalColumn: "MaHoaDon",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "NhanViens",
                columns: new[] { "MaNhanVien", "CCCD", "Email", "GioiTinh", "HoTen", "NgaySinh", "SoDienThoai", "TrangThai" },
                values: new object[,]
                {
                    { 1, "079090000001", "ha@kytu.com", "Nam", "Nguyen Hoang Ha", new DateTime(1990, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912000001", "DangLamViec" },
                    { 2, "079092000002", "lan@kytu.com", "Nu", "Tran Bich Lan", new DateTime(1992, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912000002", "DangLamViec" }
                });

            migrationBuilder.InsertData(
                table: "SinhViens",
                columns: new[] { "MaSinhVien", "CCCD", "Email", "GioiTinh", "HoTen", "NgaySinh", "SoDienThoai", "TrangThai" },
                values: new object[,]
                {
                    { 1, "001204000001", "an@kytu.com", "Nam", "Nguyen Van An", new DateTime(2004, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901000001", "DangO" },
                    { 2, "001204000002", "binh@kytu.com", "Nu", "Tran Thi Binh", new DateTime(2004, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901000002", "DangO" },
                    { 3, "001203000003", "cuong@kytu.com", "Nam", "Le Quang Cuong", new DateTime(2003, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901000003", "DangO" },
                    { 4, "001205000004", "duyen@kytu.com", "Nu", "Pham Thi Duyen", new DateTime(2005, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901000004", "DangO" },
                    { 5, "001204000005", "em@kytu.com", "Nam", "Vo Minh Em", new DateTime(2004, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901000005", "DangO" }
                });

            migrationBuilder.InsertData(
                table: "ThongBaos",
                columns: new[] { "MaThongBao", "NgayTao", "NoiDung", "TieuDe" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sinh vien can dong tien truoc ngay 10 hang thang", "Thong bao dong tien" },
                    { 2, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kiem tra he thong dien nuoc dinh ky", "Thong bao bao tri" }
                });

            migrationBuilder.InsertData(
                table: "ToaNhas",
                columns: new[] { "MaToa", "SoTang", "TenToa" },
                values: new object[,]
                {
                    { 1, 5, "Toa A" },
                    { 2, 6, "Toa B" },
                    { 3, 4, "Toa C" }
                });

            migrationBuilder.InsertData(
                table: "Phongs",
                columns: new[] { "MaPhong", "GiaPhong", "LoaiPhong", "MaToa", "SoPhong", "SucChua", "TrangThai" },
                values: new object[,]
                {
                    { 101, 1200000m, "Nam", 1, "A101", 4, "ConCho" },
                    { 102, 1200000m, "Nam", 1, "A102", 4, "ConCho" },
                    { 103, 1250000m, "Nu", 1, "A103", 4, "ConCho" },
                    { 201, 1000000m, "Nam", 2, "B201", 6, "ConCho" },
                    { 202, 1000000m, "Nu", 2, "B202", 6, "ConCho" },
                    { 203, 1000000m, "Nam", 2, "B203", 6, "ConCho" },
                    { 204, 1000000m, "Nu", 2, "B204", 6, "ConCho" },
                    { 301, 900000m, "Nam", 3, "C301", 8, "ConCho" },
                    { 302, 900000m, "Nu", 3, "C302", 8, "ConCho" },
                    { 303, 900000m, "Nam", 3, "C303", 8, "ConCho" }
                });

            migrationBuilder.InsertData(
                table: "YeuCauSuaChuas",
                columns: new[] { "MaYeuCau", "MaNhanVien", "MaSinhVien", "MoTa", "NgayYeuCau", "TrangThai" },
                values: new object[,]
                {
                    { 1, 1, 1, "Sua den phong A101", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DangXuLy" },
                    { 2, null, 3, "Sua quat phong B201", new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "DaGui" }
                });

            migrationBuilder.InsertData(
                table: "HopDongs",
                columns: new[] { "MaHopDong", "MaPhong", "MaSinhVien", "NgayBatDau", "NgayKetThuc", "TienDatCoc", "TrangThai" },
                values: new object[,]
                {
                    { 1, 101, 1, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "HieuLuc" },
                    { 2, 103, 2, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "HieuLuc" },
                    { 3, 201, 3, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 900000m, "HieuLuc" }
                });

            migrationBuilder.InsertData(
                table: "TaiSans",
                columns: new[] { "MaTaiSan", "MaPhong", "NgayMua", "TenTaiSan", "TinhTrang" },
                values: new object[,]
                {
                    { 1, 101, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "May lanh", "Tot" },
                    { 2, 103, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quat tran", "Tot" },
                    { 3, 201, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Binh nuoc nong", "CanBaoTri" }
                });

            migrationBuilder.InsertData(
                table: "HoaDons",
                columns: new[] { "MaHoaDon", "MaHopDong", "MaPhong", "MaSinhVien", "NgayThanhToan", "TongTien", "TrangThai" },
                values: new object[,]
                {
                    { 1, 1, 101, 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1200000m, "DaThanhToan" },
                    { 2, 2, 103, 2, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1250000m, "DaThanhToan" },
                    { 3, 3, 201, 3, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "ChuaThanhToan" }
                });

            migrationBuilder.InsertData(
                table: "ThanhToans",
                columns: new[] { "MaThanhToan", "MaHoaDon", "NgayThanhToan", "PhuongThuc", "SoTien" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "ChuyenKhoan", 1200000m },
                    { 2, 2, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "TienMat", 1250000m },
                    { 3, 3, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "ChuyenKhoan", 1000000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaHopDong",
                table: "HoaDons",
                column: "MaHopDong");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaPhong",
                table: "HoaDons",
                column: "MaPhong");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaSinhVien",
                table: "HoaDons",
                column: "MaSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_HopDongs_MaPhong",
                table: "HopDongs",
                column: "MaPhong");

            migrationBuilder.CreateIndex(
                name: "IX_HopDongs_MaSinhVien",
                table: "HopDongs",
                column: "MaSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_CCCD",
                table: "NhanViens",
                column: "CCCD",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_Email",
                table: "NhanViens",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phongs_MaToa",
                table: "Phongs",
                column: "MaToa");

            migrationBuilder.CreateIndex(
                name: "IX_Phongs_SoPhong",
                table: "Phongs",
                column: "SoPhong",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_CCCD",
                table: "SinhViens",
                column: "CCCD",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_Email",
                table: "SinhViens",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaiSans_MaPhong",
                table: "TaiSans",
                column: "MaPhong");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_MaHoaDon",
                table: "ThanhToans",
                column: "MaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ToaNhas_TenToa",
                table: "ToaNhas",
                column: "TenToa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauSuaChuas_MaNhanVien",
                table: "YeuCauSuaChuas",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauSuaChuas_MaSinhVien",
                table: "YeuCauSuaChuas",
                column: "MaSinhVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaiSans");

            migrationBuilder.DropTable(
                name: "ThanhToans");

            migrationBuilder.DropTable(
                name: "ThongBaos");

            migrationBuilder.DropTable(
                name: "YeuCauSuaChuas");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "HopDongs");

            migrationBuilder.DropTable(
                name: "Phongs");

            migrationBuilder.DropTable(
                name: "SinhViens");

            migrationBuilder.DropTable(
                name: "ToaNhas");
        }
    }
}
