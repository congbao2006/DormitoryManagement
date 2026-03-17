using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly QuanLyKyTucXaDbContext _db;

    public AuthController(IConfiguration configuration, QuanLyKyTucXaDbContext db)
    {
        _configuration = configuration;
        _db = db;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.MatKhau))
        {
            return BadRequest(new { message = "Email va mat khau khong duoc de trong." });
        }

        var adminEmail = _configuration["Jwt:AdminEmail"] ?? "admin@kytu.com";
        var adminPassword = _configuration["Jwt:AdminPassword"] ?? "Admin@123";

        string role;
        string userId;
        string hoTen;

        if (request.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && request.MatKhau == adminPassword)
        {
            role = "Admin";
            userId = "0";
            hoTen = "Quan tri vien";
        }
        else
        {
            var nhanVien = await _db.NhanViens.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == request.Email && x.CCCD == request.MatKhau, cancellationToken);

            if (nhanVien != null)
            {
                role = "NhanVien";
                userId = nhanVien.MaNhanVien.ToString();
                hoTen = nhanVien.HoTen;
            }
            else
            {
                var sinhVien = await _db.SinhViens.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == request.Email && x.CCCD == request.MatKhau, cancellationToken);

                if (sinhVien == null)
                {
                    return Unauthorized(new { message = "Thong tin dang nhap khong hop le." });
                }

                role = "SinhVien";
                userId = sinhVien.MaSinhVien.ToString();
                hoTen = sinhVien.HoTen;
            }
        }

        var token = TaoToken(request.Email, role, userId, hoTen);

        return Ok(new
        {
            accessToken = token,
            role,
            id = userId,
            email = request.Email,
            hoTen
        });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.HoTen)
            || string.IsNullOrWhiteSpace(request.Email)
            || string.IsNullOrWhiteSpace(request.MatKhau)
            || string.IsNullOrWhiteSpace(request.XacNhanMatKhau)
            || string.IsNullOrWhiteSpace(request.SoDienThoai)
            || string.IsNullOrWhiteSpace(request.GioiTinh))
        {
            return BadRequest(new { message = "Cac truong bat buoc khong duoc de trong." });
        }

        if (!request.Email.Contains('@'))
        {
            return BadRequest(new { message = "Email khong hop le." });
        }

        if (request.MatKhau != request.XacNhanMatKhau)
        {
            return BadRequest(new { message = "Xac nhan mat khau khong khop." });
        }

        if (request.MatKhau.Length < 6)
        {
            return BadRequest(new { message = "Mat khau phai co it nhat 6 ky tu." });
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var cccd = request.MatKhau.Trim();

        if (await _db.NhanViens.AnyAsync(x => x.Email == normalizedEmail, cancellationToken)
            || await _db.SinhViens.AnyAsync(x => x.Email == normalizedEmail, cancellationToken))
        {
            return Conflict(new { message = "Email da ton tai." });
        }

        if (await _db.NhanViens.AnyAsync(x => x.CCCD == cccd, cancellationToken)
            || await _db.SinhViens.AnyAsync(x => x.CCCD == cccd, cancellationToken))
        {
            return Conflict(new { message = "Mat khau nay da duoc su dung. Vui long chon mat khau khac." });
        }

        var gioiTinh = request.GioiTinh.Trim();
        if (!gioiTinh.Equals("Nam", StringComparison.OrdinalIgnoreCase)
            && !gioiTinh.Equals("Nu", StringComparison.OrdinalIgnoreCase)
            && !gioiTinh.Equals("Khac", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new { message = "Gioi tinh chi chap nhan Nam, Nu hoac Khac." });
        }

        var sinhVien = new SinhVien(
            maSinhVien: 0,
            hoTen: request.HoTen.Trim(),
            ngaySinh: request.NgaySinh,
            gioiTinh: char.ToUpperInvariant(gioiTinh[0]) + gioiTinh[1..].ToLowerInvariant(),
            soDienThoai: request.SoDienThoai.Trim(),
            email: normalizedEmail,
            CCCD: cccd,
            trangThai: "DaDangKy");

        _db.SinhViens.Add(sinhVien);
        await _db.SaveChangesAsync(cancellationToken);

        var token = TaoToken(sinhVien.Email, "SinhVien", sinhVien.MaSinhVien.ToString(), sinhVien.HoTen);

        return Ok(new
        {
            message = "Dang ky thanh cong.",
            accessToken = token,
            role = "SinhVien",
            id = sinhVien.MaSinhVien.ToString(),
            email = sinhVien.Email,
            hoTen = sinhVien.HoTen
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            email = User.FindFirstValue(ClaimTypes.Email),
            role = User.FindFirstValue(ClaimTypes.Role),
            id = User.FindFirstValue(ClaimTypes.NameIdentifier),
            hoTen = User.FindFirstValue(ClaimTypes.Name)
        });
    }

    private string TaoToken(string email, string role, string userId, string hoTen)
    {
        var jwt = _configuration.GetSection("Jwt");
        var secret = jwt["Secret"] ?? throw new InvalidOperationException("Jwt:Secret is missing.");
        var issuer = jwt["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
        var audience = jwt["Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, hoTen),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public sealed class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string MatKhau { get; set; } = string.Empty;
}

public sealed class RegisterRequest
{
    public string HoTen { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MatKhau { get; set; } = string.Empty;
    public string XacNhanMatKhau { get; set; } = string.Empty;
    public DateTime NgaySinh { get; set; }
    public string GioiTinh { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
}