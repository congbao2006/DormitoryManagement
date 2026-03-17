# Quan Ly Ky Tuc Xa

He thong Quan ly Ky tuc xa duoc xay dung theo Clean Architecture voi backend ASP.NET Core Web API (.NET 8) va frontend Node.js + Express + EJS.

## Kien truc

- `QuanLyKyTucXa.Domain`: Entity va nghiep vu cot loi theo UML.
- `QuanLyKyTucXa.Application`: MediatR, FluentValidation, AutoMapper, service abstractions.
- `QuanLyKyTucXa.Infrastructure`: EF Core 8, MySQL, repository, DbContext, seed data.
- `QuanLyKyTucXa.API`: Web API, controllers, Swagger, CORS, exception handling.
- `frontend-node`: Giao dien Node.js/Express/EJS tieu thu API C#.
- `QuanLyKyTucXa.Tests`: xUnit tests cho nghiep vu quan trong.

## Tinh nang hien co

- Domain model theo UML voi ten tieng Viet giu nguyen.
- CRUD API cho SinhVien, Phong, HopDong, HoaDon va cac entity con lai.
- MediatR use cases cho lop QuanLyKyTucXa.
- FluentValidation pipeline.
- AutoMapper DTO mappings.
- EF Core MySQL DbContext, chi muc, rang buoc FK, seed data.
- Frontend dashboard va cac man hinh quan ly co ban cho Sinh vien, Phong, Hop dong, Hoa don.
- JWT Authentication va role-based authorization (Admin, NhanVien, SinhVien).
- Swagger UI.

## Yeu cau moi truong

- .NET SDK 8.0.x
- .NET Runtime 8.0.x
- Node.js 18+
- MySQL 8.0+
- ngrok

## Cau hinh MySQL

1. Tao database:

```sql
CREATE DATABASE QuanLyKyTucXaDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

2. Sua connection string trong [QuanLyKyTucXa.API/appsettings.json](QuanLyKyTucXa.API/appsettings.json).

Mac dinh:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=QuanLyKyTucXaDb;User=root;Password=123456;"
}
```

## Chay Backend

```bash
cd /Users/bao/Desktop/new
DOTNET_ROLL_FORWARD=Major dotnet restore
DOTNET_ROLL_FORWARD=Major dotnet build QuanLyKyTucXa.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project QuanLyKyTucXa.API/QuanLyKyTucXa.API.csproj
```

Swagger:

- `https://localhost:xxxx/swagger`
- `http://localhost:xxxx/swagger`

## EF Migrations

Da tao san migration dau tien tai [QuanLyKyTucXa.Infrastructure/Persistence/Migrations/20260314135109_InitialCreate.cs](QuanLyKyTucXa.Infrastructure/Persistence/Migrations/20260314135109_InitialCreate.cs) va xuat SQL tai [database/QuanLyKyTucXaDb.sql](database/QuanLyKyTucXaDb.sql).

De scaffold migration moi hoac cap nhat SQL script, chay:

```bash
cd /Users/bao/Desktop/new

dotnet tool restore

dotnet ef migrations add InitialCreate \
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

Neu muon tao migration tiep theo, thay `InitialCreate` bang ten migration moi.

`dotnet ef database update` can MySQL dang chay voi connection string da cau hinh trong [QuanLyKyTucXa.API/appsettings.json](QuanLyKyTucXa.API/appsettings.json).

Neu chi co .NET 9 tren may, co the thu tam:

```bash
DOTNET_ROLL_FORWARD=Major dotnet ef ...
```

Nhung khuyen nghi van la cai dung .NET 8 runtime.

## Chay Frontend Node.js

```bash
cd /Users/bao/Desktop/new/frontend-node
cp .env.example .env
npm install
npm run dev
```

Frontend mac dinh chay tai:

- `http://localhost:3000`

Bien moi truong:

```env
PORT=3000
API_BASE_URL=http://localhost:5000/api
SESSION_SECRET=thay_doi_secret_khi_trien_khai
```

## Dang nhap

He thong frontend yeu cau dang nhap de truy cap cac man hinh quan ly.

Da bo sung luong dang ky tai `GET /register` (frontend) va `POST /api/Auth/register` (backend).
Tai khoan SinhVien dang ky moi se duoc tu dong dang nhap sau khi dang ky thanh cong.

- Admin mac dinh:
  - Email: `admin@kytu.com`
  - Mat khau: `Admin@123`
- Nhan vien/Sinh vien:
  - Email: theo du lieu seed
  - Mat khau: CCCD tuong ung trong seed data
- Sinh vien dang ky moi:
  - Email: theo form dang ky
  - Mat khau: theo form dang ky

JWT endpoint:

- `POST /api/Auth/login`
- `POST /api/Auth/register`
- `GET /api/Auth/me`

## Phan quyen vai tro

- Admin:
  - Toan quyen tat ca module
  - Quan ly NhanVien va phan he he thong
- NhanVien:
  - Quan ly SinhVien, Phong, ToaNha, HopDong, HoaDon, ThongBao, YeuCau, TaiSan, ThanhToan
  - Khong truy cap module NhanVien
- SinhVien:
  - Truy cap SinhVien, HopDong, HoaDon, YeuCau, ThanhToan
  - Khong truy cap Phong, ToaNha, TaiSan, ThongBao, NhanVien

## Ownership Rules (SinhVien)

- API da ap dung kiem soat ownership theo JWT claim `NameIdentifier` cho SinhVien.
- SinhVien chi xem duoc du lieu cua chinh minh trong:
  - `SinhViens`
  - `HopDongs`
  - `HoaDons`
  - `YeuCauSuaChuas`
  - `ThanhToans` (thong qua lien ket `MaHoaDon` -> `HoaDon.MaSinhVien`)
- SinhVien khong duoc tao/sua/xoa hop dong va hoa don.
- SinhVien chi duoc tao yeu cau sua chua cho chinh minh (he thong tu gan `MaSinhVien` theo token).
- Frontend da bo sung route-level write guard: khong chi an nut tren UI ma con chan truy cap truc tiep vao cac route ghi du lieu trai quyen.
- Da ap dung route-level guard cho `SinhVien` o cac module `SinhVien`, `HopDong`, `HoaDon`, `YeuCauSuaChua`, `ThanhToan`.

Luu y khi tao du lieu:

- `POST /api/YeuCauSuaChuas`: voi Admin/NhanVien can truyen `maSinhVien` tren query string.
- `POST /api/ThanhToans`: can truyen `maHoaDon` tren query string.

## Ngrok

Expose frontend:

```bash
ngrok http 3000
```

Expose backend:

```bash
ngrok http 5000
```

## Unit Tests

Da tao 5 test nghiep vu trong [QuanLyKyTucXa.Tests/DomainMethodsTests.cs](QuanLyKyTucXa.Tests/DomainMethodsTests.cs).
Da bo sung test ownership/authorization trong [QuanLyKyTucXa.Tests/OwnershipAuthorizationTests.cs](QuanLyKyTucXa.Tests/OwnershipAuthorizationTests.cs), gom:

- SinhVien khong duoc xem/sua du lieu cua sinh vien khac
- SinhVien chi thay hop dong cua minh
- SinhVien bi chan ghi hoa don
- Yeu cau sua chua tu dong gan MaSinhVien theo token
- Thanh toan khong thuoc sinh vien thi bi chan

Da bo sung service tests trong [QuanLyKyTucXa.Tests/QuanLyKyTucXaServiceTests.cs](QuanLyKyTucXa.Tests/QuanLyKyTucXaServiceTests.cs), gom:

- NotFound handling cho xoa/cap nhat nghiep vu
- Cap nhat sinh vien + gan vao phong
- Loc danh sach phong con cho o tang application service

Da bo sung them integration tests trong [QuanLyKyTucXa.IntegrationTests/AuthAndOwnershipIntegrationTests.cs](QuanLyKyTucXa.IntegrationTests/AuthAndOwnershipIntegrationTests.cs) cho:

- Login, `me`, anonymous access blocking
- Ownership read rules cho `HopDongs`, `HoaDons`, `ThanhToans`
- Create/update flow cho `YeuCauSuaChuas` va `ThanhToans`
- Chan thao tac ghi du lieu cua sinh vien tren du lieu khong thuoc minh
- Validate query string bat buoc cho thao tac tao `YeuCauSuaChuas` va `ThanhToans` bang `NhanVien`
- Validate `NhanViens` la module chi `Admin` moi truy cap duoc
- Validate CRUD access cho `ThongBaos`, `ToaNhas`, `Phongs` theo role `Admin/NhanVien`

Da bo sung frontend route-guard tests trong [frontend-node/tests/route-guards.test.js](frontend-node/tests/route-guards.test.js), gom:

- Chuyen huong ve `/login` khi chua co session
- Chan `SinhVien` truy cap module `NhanVien`, `YeuCau` edit, `ThanhToan` delete
- Chan `SinhVien` sua ho so cua nguoi khac tren frontend
- Chuyen huong khoi `/login` khi da dang nhap

Da bo sung them frontend auth/dashboard tests trong [frontend-node/tests/auth-dashboard.test.js](frontend-node/tests/auth-dashboard.test.js), gom:

- Dang nhap thanh cong va that bai
- Dashboard render dung thong ke cho `Admin`
- Dashboard bo qua API call khong thuoc quyen cho `SinhVien`
- Dang xuat xoa session dung cach

Chay test:

```bash
dotnet test QuanLyKyTucXa.Tests/QuanLyKyTucXa.Tests.csproj
dotnet test QuanLyKyTucXa.IntegrationTests/QuanLyKyTucXa.IntegrationTests.csproj
cd frontend-node && npm test
```

## Screenshot placeholders

- Dashboard: cap nhat sau
- SinhVien CRUD: cap nhat sau
- Phong CRUD: cap nhat sau
- HopDong CRUD: cap nhat sau
- HoaDon CRUD: cap nhat sau

## Database schema

Schema duoc dinh nghia trong [QuanLyKyTucXa.Infrastructure/Persistence/QuanLyKyTucXaDbContext.cs](QuanLyKyTucXa.Infrastructure/Persistence/QuanLyKyTucXaDbContext.cs).
Migration SQL hien da co san tai [database/QuanLyKyTucXaDb.sql](database/QuanLyKyTucXaDb.sql).

## Ghi chu hien tai

- Build solution da thanh cong.
- `dotnet test` va `dotnet ef` da chay duoc tren may hien tai sau khi cai `dotnet-sdk@8`.
- Toan bo test backend dang xanh, va frontend test suite cho route guards + auth/dashboard flows cung dang xanh.
- Frontend hien da co CRUD day du cho cac module chinh va bo sung auth session + login/logout.
