using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace QuanLyKyTucXa.IntegrationTests;

public class AuthAndOwnershipIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthAndOwnershipIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsJwtToken()
    {
        var payload = new { Email = "admin@kytu.com", MatKhau = "Admin@123" };

        var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
        body.GetProperty("role").GetString().Should().Be("Admin");
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var payload = new { Email = "admin@kytu.com", MatKhau = "SaiMatKhau" };

        var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AnonymousUser_CannotAccessProtectedEndpoint()
    {
        var response = await _client.GetAsync("/api/sinhviens");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SinhVien_CannotReadOtherSinhVienContract()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/hopdongs/2");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SinhVien_CanReadOwnContract()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/hopdongs/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SinhVien_CannotReadOtherStudentInvoice()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/hoadons/2");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SinhVien_CanReadOwnInvoice()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/hoadons/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task NhanVien_CanAccessMaintenanceRequests()
    {
        var token = await LoginAndGetToken("ha@kytu.com", "079090000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/yeucausuachuas");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SinhVien_CannotDeleteEntityWithoutPermission()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/sinhviens/1");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AuthenticatedUser_CanReadOwnProfileFromMeEndpoint()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/auth/me");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("email").GetString().Should().Be("an@kytu.com");
        body.GetProperty("role").GetString().Should().Be("SinhVien");
        body.GetProperty("id").GetString().Should().Be("1");
    }

    [Fact]
    public async Task SinhVien_CannotReadOtherStudentPayment()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/thanhtoans/2");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SinhVien_CanReadOwnPayment()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/thanhtoans/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SinhVien_CanCreateAndUpdateOwnMaintenanceRequest()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var maYeuCau = NewEntityId();
        var createPayload = new
        {
            MaYeuCau = maYeuCau,
            MoTa = "Sua khoa cua phong A101",
            NgayYeuCau = new DateTime(2026, 3, 14, 8, 0, 0, DateTimeKind.Utc),
            TrangThai = "DaGui"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/yeucausuachuas", createPayload);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var updatePayload = new
        {
            MaYeuCau = maYeuCau,
            MoTa = "Da sua khoa cua phong A101",
            NgayYeuCau = new DateTime(2026, 3, 15, 8, 0, 0, DateTimeKind.Utc),
            TrangThai = "DangXuLy"
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/yeucausuachuas/{maYeuCau}", updatePayload);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/yeucausuachuas/{maYeuCau}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("moTa").GetString().Should().Be("Da sua khoa cua phong A101");
        body.GetProperty("trangThai").GetString().Should().Be("DangXuLy");
    }

    [Fact]
    public async Task SinhVien_CreatedMaintenanceRequest_CannotBeReadByOtherStudent()
    {
        var ownToken = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ownToken);

        var maYeuCau = NewEntityId();
        var createPayload = new
        {
            MaYeuCau = maYeuCau,
            MoTa = "Sua o cam dien phong A101",
            NgayYeuCau = new DateTime(2026, 3, 14, 9, 0, 0, DateTimeKind.Utc),
            TrangThai = "DaGui"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/yeucausuachuas", createPayload);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var otherToken = await LoginAndGetToken("binh@kytu.com", "001204000002");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", otherToken);

        var otherReadResponse = await _client.GetAsync($"/api/yeucausuachuas/{maYeuCau}");

        otherReadResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SinhVien_CanCreateAndUpdateOwnPayment()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var maThanhToan = NewEntityId();
        var createPayload = new
        {
            MaThanhToan = maThanhToan,
            SoTien = 1200000m,
            PhuongThuc = "ChuyenKhoan",
            NgayThanhToan = new DateTime(2026, 3, 14, 10, 0, 0, DateTimeKind.Utc)
        };

        var createResponse = await _client.PostAsJsonAsync("/api/thanhtoans?maHoaDon=1", createPayload);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var updatePayload = new
        {
            MaThanhToan = maThanhToan,
            SoTien = 1250000m,
            PhuongThuc = "TienMat",
            NgayThanhToan = new DateTime(2026, 3, 15, 10, 0, 0, DateTimeKind.Utc)
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/thanhtoans/{maThanhToan}", updatePayload);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/thanhtoans/{maThanhToan}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("soTien").GetDecimal().Should().Be(1250000m);
        body.GetProperty("phuongThuc").GetString().Should().Be("TienMat");
    }

    [Fact]
    public async Task SinhVien_CannotCreatePaymentForOtherStudentInvoice()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var maThanhToan = NewEntityId();
        var createPayload = new
        {
            MaThanhToan = maThanhToan,
            SoTien = 1250000m,
            PhuongThuc = "ChuyenKhoan",
            NgayThanhToan = new DateTime(2026, 3, 14, 11, 0, 0, DateTimeKind.Utc)
        };

        var response = await _client.PostAsJsonAsync("/api/thanhtoans?maHoaDon=2", createPayload);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SinhVien_CannotUpdateOtherStudentsMaintenanceRequest()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updatePayload = new
        {
            MaYeuCau = 2,
            MoTa = "Thu cap nhat yeu cau cua sinh vien khac",
            NgayYeuCau = new DateTime(2026, 3, 15, 12, 0, 0, DateTimeKind.Utc),
            TrangThai = "DangXuLy"
        };

        var response = await _client.PutAsJsonAsync("/api/yeucausuachuas/2", updatePayload);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task NhanVien_CreateMaintenanceRequest_WithoutMaSinhVien_ReturnsBadRequest()
    {
        var token = await LoginAndGetToken("ha@kytu.com", "079090000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createPayload = new
        {
            MaYeuCau = NewEntityId(),
            MoTa = "Bao tri quat hanh lang",
            NgayYeuCau = new DateTime(2026, 3, 16, 8, 0, 0, DateTimeKind.Utc),
            TrangThai = "DaGui"
        };

        var response = await _client.PostAsJsonAsync("/api/yeucausuachuas", createPayload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task NhanVien_CreateMaintenanceRequest_WithMaSinhVien_ReturnsCreated()
    {
        var token = await LoginAndGetToken("ha@kytu.com", "079090000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var maYeuCau = NewEntityId();
        var createPayload = new
        {
            MaYeuCau = maYeuCau,
            MoTa = "Bao tri he thong den tang 1",
            NgayYeuCau = new DateTime(2026, 3, 16, 9, 0, 0, DateTimeKind.Utc),
            TrangThai = "DaGui"
        };

        var response = await _client.PostAsJsonAsync("/api/yeucausuachuas?maSinhVien=2", createPayload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task NhanVien_CreatePayment_WithoutMaHoaDon_ReturnsBadRequest()
    {
        var token = await LoginAndGetToken("ha@kytu.com", "079090000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createPayload = new
        {
            MaThanhToan = NewEntityId(),
            SoTien = 1000000m,
            PhuongThuc = "ChuyenKhoan",
            NgayThanhToan = new DateTime(2026, 3, 16, 10, 0, 0, DateTimeKind.Utc)
        };

        var response = await _client.PostAsJsonAsync("/api/thanhtoans", createPayload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task NhanVien_CreatePayment_WithMaHoaDon_ReturnsCreated()
    {
        var token = await LoginAndGetToken("ha@kytu.com", "079090000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var maThanhToan = NewEntityId();
        var createPayload = new
        {
            MaThanhToan = maThanhToan,
            SoTien = 1000000m,
            PhuongThuc = "ChuyenKhoan",
            NgayThanhToan = new DateTime(2026, 3, 16, 11, 0, 0, DateTimeKind.Utc)
        };

        var response = await _client.PostAsJsonAsync("/api/thanhtoans?maHoaDon=3", createPayload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task NhanVien_CannotAccessAdminOnlyNhanVienModule()
    {
        var token = await LoginAndGetToken("ha@kytu.com", "079090000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/nhanviens");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Admin_CanAccessNhanVienModule()
    {
        var token = await LoginAndGetToken("admin@kytu.com", "Admin@123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/nhanviens");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task NhanVien_CanCreateThongBao()
    {
        var token = await LoginAndGetToken("ha@kytu.com", "079090000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var maThongBao = NewEntityId();
        var payload = new
        {
            MaThongBao = maThongBao,
            TieuDe = "Thong bao ve dien nuoc",
            NoiDung = "Tam ngung dien 30 phut de bao tri",
            NgayTao = new DateTime(2026, 3, 17, 8, 0, 0, DateTimeKind.Utc)
        };

        var response = await _client.PostAsJsonAsync("/api/thongbaos", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task SinhVien_CannotAccessThongBaoModule()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/thongbaos");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task NhanVien_CanCreateToaNha()
    {
        var token = await LoginAndGetToken("ha@kytu.com", "079090000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var maToa = NewEntityId();
        var payload = new
        {
            MaToa = maToa,
            TenToa = $"Toa Test {maToa}",
            SoTang = 7
        };

        var response = await _client.PostAsJsonAsync("/api/toanhas", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task SinhVien_CannotAccessToaNhaModule()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/toanhas");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task NhanVien_CanCreatePhong()
    {
        var token = await LoginAndGetToken("ha@kytu.com", "079090000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var maPhong = NewEntityId();
        var payload = new
        {
            MaPhong = maPhong,
            SoPhong = $"T{maPhong}",
            LoaiPhong = "Nam",
            SucChua = 4,
            GiaPhong = 1300000m,
            TrangThai = "ConCho"
        };

        var response = await _client.PostAsJsonAsync("/api/phongs", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task SinhVien_CannotAccessPhongModule()
    {
        var token = await LoginAndGetToken("an@kytu.com", "001204000001");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/phongs");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<string> LoginAndGetToken(string email, string matKhau)
    {
        var payload = new { Email = email, MatKhau = matKhau };
        var response = await _client.PostAsJsonAsync("/api/auth/login", payload);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        return body.GetProperty("accessToken").GetString()!;
    }

    private static int NewEntityId()
    {
        return Random.Shared.Next(10000, 99999);
    }
}
