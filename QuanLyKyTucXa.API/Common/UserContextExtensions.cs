using System.Security.Claims;

namespace QuanLyKyTucXa.API.Common;

public static class UserContextExtensions
{
    public static bool LaSinhVien(this ClaimsPrincipal user)
    {
        return user.IsInRole("SinhVien");
    }

    public static int? LayMaSinhVien(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(claim, out var id) ? id : null;
    }
}