# Quản Lý Ký Túc Xá

Hệ thống quản lý ký túc xá được xây dựng theo Clean Architecture với backend ASP.NET Core Web API (.NET 8) và frontend Node.js + Express + EJS.

## Chạy nhanh

Đây là phần ưu tiên nếu chỉ cần chạy dự án.

### 1. Yêu cầu môi trường

- .NET SDK 8
- Node.js 18+
- MySQL 8
- ngrok (nếu cần public local)

### 2. Tạo database MySQL

```sql
CREATE DATABASE QuanLyKyTucXaDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

Sau đó kiểm tra connection string trong [QuanLyKyTucXa.API/appsettings.json](QuanLyKyTucXa.API/appsettings.json).

Mặc định:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=QuanLyKyTucXaDb;User=root;Password=123456;"
}
```

### 3. Chạy backend

Tại thư mục gốc của dự án:

```bash
dotnet restore
dotnet build QuanLyKyTucXa.sln
dotnet run --project QuanLyKyTucXa.API/QuanLyKyTucXa.API.csproj --urls http://localhost:5000
```

Swagger sau khi chạy:

- http://localhost:5000/swagger

Lưu ý: backend đang gọi `EnsureCreated()`, vì vậy database sẽ được tạo schema khi ứng dụng khởi động nếu tài khoản MySQL có đủ quyền.

### 4. Chạy frontend

```bash
cd frontend-node
cp .env.example .env
npm install
npm run dev
```

Frontend mặc định chạy tại:

- http://localhost:3000

Biến môi trường trong file `.env`:

```env
PORT=3000
API_BASE_URL=http://localhost:5000/api
SESSION_SECRET=thay_doi_secret_khi_trien_khai
```

### 5. Đăng nhập mặc định

- Admin
  - Email: `admin@kytu.com`
  - Mật khẩu: `Admin@123`
- Nhân viên / Sinh viên seed sẵn
  - Email: theo dữ liệu seed
  - Mật khẩu: CCCD tương ứng trong seed data
- Sinh viên đăng ký mới
  - Email: theo form đăng ký
  - Mật khẩu: theo form đăng ký

## Chạy bằng VS Code Tasks

Workspace đã có sẵn 2 task để chạy nhanh:

- `Run Backend API`
- `Run Frontend Node`

Nếu dùng VS Code, đây là cách nhanh nhất để khởi động cả hai dịch vụ.

## Kiến trúc

- `QuanLyKyTucXa.Domain`: Entity và nghiệp vụ cốt lõi theo UML.
- `QuanLyKyTucXa.Application`: MediatR, FluentValidation, AutoMapper, service abstractions.
- `QuanLyKyTucXa.Infrastructure`: EF Core 8, MySQL, repository, DbContext, seed data.
- `QuanLyKyTucXa.API`: Web API, controllers, Swagger, CORS, exception handling.
- `frontend-node`: Giao diện Node.js/Express/EJS tiêu thụ API C#.
- `QuanLyKyTucXa.Tests`: xUnit tests cho các nghiệp vụ quan trọng.

## Tính năng hiện có

- Domain model theo UML với tên tiếng Việt được giữ nguyên.
- CRUD API cho SinhVien, Phong, HopDong, HoaDon và các entity còn lại.
- MediatR use cases cho lớp QuanLyKyTucXa.
- FluentValidation pipeline.
- AutoMapper DTO mappings.
- EF Core MySQL DbContext, chỉ mục, ràng buộc khóa ngoại, seed data.
- Frontend dashboard và các màn hình quản lý cơ bản cho Sinh viên, Phòng, Hợp đồng, Hóa đơn.
- JWT Authentication và role-based authorization cho `Admin`, `NhanVien`, `SinhVien`.
- Swagger UI.

## Xác thực và tài khoản

Hệ thống frontend yêu cầu đăng nhập để truy cập các màn hình quản lý.

Đã bổ sung luồng đăng ký tại:

- `GET /register` trên frontend
- `POST /api/Auth/register` trên backend

Tài khoản `SinhVien` đăng ký mới sẽ được tự động đăng nhập sau khi đăng ký thành công.

JWT endpoints:

- `POST /api/Auth/login`
- `POST /api/Auth/register`
- `GET /api/Auth/me`

## Phân quyền vai trò

- `Admin`
  - Toàn quyền tất cả module
  - Quản lý Nhân viên và phân hệ hệ thống
- `NhanVien`
  - Quản lý Sinh viên, Phòng, Tòa nhà, Hợp đồng, Hóa đơn, Thông báo, Yêu cầu, Tài sản, Thanh toán
  - Không truy cập module Nhân viên
- `SinhVien`
  - Truy cập Sinh viên, Hợp đồng, Hóa đơn, Yêu cầu, Thanh toán
  - Không truy cập Phòng, Tòa nhà, Tài sản, Thông báo, Nhân viên

## Ownership Rules cho SinhVien

- API đã áp dụng kiểm soát ownership theo JWT claim `NameIdentifier` cho `SinhVien`.
- `SinhVien` chỉ xem được dữ liệu của chính mình trong:
  - `SinhViens`
  - `HopDongs`
  - `HoaDons`
  - `YeuCauSuaChuas`
  - `ThanhToans` thông qua liên kết `MaHoaDon -> HoaDon.MaSinhVien`
- `SinhVien` không được tạo, sửa, xóa hợp đồng và hóa đơn.
- `SinhVien` chỉ được tạo yêu cầu sửa chữa cho chính mình; hệ thống tự gán `MaSinhVien` theo token.
- Frontend đã bổ sung route-level write guard để chặn truy cập trực tiếp vào các route ghi dữ liệu trái quyền.
- Route-level guard đã áp dụng cho các module `SinhVien`, `HopDong`, `HoaDon`, `YeuCauSuaChua`, `ThanhToan`.

Lưu ý khi tạo dữ liệu:

- `POST /api/YeuCauSuaChuas`: với `Admin` hoặc `NhanVien`, cần truyền `maSinhVien` trên query string.
- `POST /api/ThanhToans`: cần truyền `maHoaDon` trên query string.

## EF Migrations và schema

Migration ban đầu đã được tạo tại [QuanLyKyTucXa.Infrastructure/Persistence/Migrations/20260314135109_InitialCreate.cs](QuanLyKyTucXa.Infrastructure/Persistence/Migrations/20260314135109_InitialCreate.cs) và SQL đã xuất tại [database/QuanLyKyTucXaDb.sql](database/QuanLyKyTucXaDb.sql).

Nếu muốn tạo migration mới hoặc cập nhật SQL script:

```bash
dotnet tool restore

dotnet ef migrations add TenMigrationMoi \
  --project QuanLyKyTucXa.Infrastructure/QuanLyKyTucXa.Infrastructure.csproj \
  --context QuanLyKyTucXaDbContext \
  --output-dir Persistence/Migrations

dotnet ef database update \
  --project QuanLyKyTucXa.Infrastructure/QuanLyKyTucXa.Infrastructure.csproj \
  --context QuanLyKyTucXaDbContext

dotnet ef migrations script \
  --project QuanLyKyTucXa.Infrastructure/QuanLyKyTucXa.Infrastructure.csproj \
  --context QuanLyKyTucXaDbContext \
  -o database/QuanLyKyTucXaDb.sql
```

Nếu máy chỉ có .NET 9, có thể thử tạm:

```bash
DOTNET_ROLL_FORWARD=Major dotnet ef ...
```

Tuy nhiên, vẫn nên cài đúng .NET 8 để tránh sai khác runtime.

Schema hiện tại được định nghĩa trong [QuanLyKyTucXa.Infrastructure/Persistence/QuanLyKyTucXaDbContext.cs](QuanLyKyTucXa.Infrastructure/Persistence/QuanLyKyTucXaDbContext.cs).

## Kiểm thử

Backend tests:

```bash
dotnet test QuanLyKyTucXa.Tests/QuanLyKyTucXa.Tests.csproj
dotnet test QuanLyKyTucXa.IntegrationTests/QuanLyKyTucXa.IntegrationTests.csproj
```

Frontend tests:

```bash
cd frontend-node
npm test
```

Các nhóm test hiện có:

- Nghiệp vụ domain trong [QuanLyKyTucXa.Tests/DomainMethodsTests.cs](QuanLyKyTucXa.Tests/DomainMethodsTests.cs)
- Ownership / authorization trong [QuanLyKyTucXa.Tests/OwnershipAuthorizationTests.cs](QuanLyKyTucXa.Tests/OwnershipAuthorizationTests.cs)
- Application service trong [QuanLyKyTucXa.Tests/QuanLyKyTucXaServiceTests.cs](QuanLyKyTucXa.Tests/QuanLyKyTucXaServiceTests.cs)
- Integration tests trong [QuanLyKyTucXa.IntegrationTests/AuthAndOwnershipIntegrationTests.cs](QuanLyKyTucXa.IntegrationTests/AuthAndOwnershipIntegrationTests.cs)
- Frontend route guards trong [frontend-node/tests/route-guards.test.js](frontend-node/tests/route-guards.test.js)
- Frontend auth và dashboard trong [frontend-node/tests/auth-dashboard.test.js](frontend-node/tests/auth-dashboard.test.js)

## Ngrok

Expose frontend:

```bash
ngrok http 3000
```

Expose backend:

```bash
ngrok http 5000
```

## Screenshot placeholders

- Dashboard: cập nhật sau
- SinhVien CRUD: cập nhật sau
- Phong CRUD: cập nhật sau
- HopDong CRUD: cập nhật sau
- HoaDon CRUD: cập nhật sau

## Ghi chú hiện tại

- Build solution đã thành công.
- `dotnet test` và `dotnet ef` đã chạy được sau khi cài .NET 8.
- Toàn bộ test backend đang xanh.
- Frontend test suite cho route guards và auth/dashboard flows cũng đang xanh.
- Frontend hiện đã có CRUD đầy đủ cho các module chính cùng auth session và login/logout.
