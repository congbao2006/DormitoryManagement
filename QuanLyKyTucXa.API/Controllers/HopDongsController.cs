using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.API.Common;
using QuanLyKyTucXa.Domain.Entities;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,NhanVien,SinhVien")]
public sealed class HopDongsController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public HopDongsController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<HopDongResponse>>> GetAll(CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            var own = await _db.HopDongs.AsNoTracking()
                .Where(x => EF.Property<int>(x, "MaSinhVien") == maSinhVien.Value)
                .Select(x => new HopDongResponse
                {
                    MaHopDong = x.MaHopDong,
                    NgayBatDau = x.NgayBatDau,
                    NgayKetThuc = x.NgayKetThuc,
                    TienDatCoc = x.TienDatCoc,
                    TrangThai = x.TrangThai,
                    MaSinhVien = EF.Property<int>(x, "MaSinhVien"),
                    MaPhong = EF.Property<int>(x, "MaPhong")
                })
                .ToListAsync(cancellationToken);

            return Ok(own);
        }

        var all = await _db.HopDongs.AsNoTracking()
            .Select(x => new HopDongResponse
            {
                MaHopDong = x.MaHopDong,
                NgayBatDau = x.NgayBatDau,
                NgayKetThuc = x.NgayKetThuc,
                TienDatCoc = x.TienDatCoc,
                TrangThai = x.TrangThai,
                MaSinhVien = EF.Property<int>(x, "MaSinhVien"),
                MaPhong = EF.Property<int>(x, "MaPhong")
            })
            .ToListAsync(cancellationToken);

        return Ok(all);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HopDongResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _db.HopDongs.AsNoTracking()
            .Where(x => x.MaHopDong == id)
            .Select(x => new
            {
                Response = new HopDongResponse
                {
                    MaHopDong = x.MaHopDong,
                    NgayBatDau = x.NgayBatDau,
                    NgayKetThuc = x.NgayKetThuc,
                    TienDatCoc = x.TienDatCoc,
                    TrangThai = x.TrangThai,
                    MaSinhVien = EF.Property<int>(x, "MaSinhVien"),
                    MaPhong = EF.Property<int>(x, "MaPhong")
                }
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue || result.Response.MaSinhVien != maSinhVien.Value)
            {
                return Forbid();
            }
        }

        return Ok(result.Response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HopDongUpsertRequest request, CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            return Forbid();
        }

        var entity = new HopDong(request.MaHopDong, request.NgayBatDau, request.NgayKetThuc, request.TienDatCoc)
        {
            TrangThai = request.TrangThai
        };

        _db.HopDongs.Add(entity);
        _db.Entry(entity).Property("MaSinhVien").CurrentValue = request.MaSinhVien;
        _db.Entry(entity).Property("MaPhong").CurrentValue = request.MaPhong;

        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaHopDong }, new HopDongResponse
        {
            MaHopDong = entity.MaHopDong,
            NgayBatDau = entity.NgayBatDau,
            NgayKetThuc = entity.NgayKetThuc,
            TienDatCoc = entity.TienDatCoc,
            TrangThai = entity.TrangThai,
            MaSinhVien = request.MaSinhVien,
            MaPhong = request.MaPhong
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] HopDongUpsertRequest request, CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            return Forbid();
        }

        var entity = await _db.HopDongs.FirstOrDefaultAsync(x => x.MaHopDong == id, cancellationToken);
        if (entity == null) return NotFound();

        entity.NgayBatDau = request.NgayBatDau;
        entity.NgayKetThuc = request.NgayKetThuc;
        entity.TienDatCoc = request.TienDatCoc;
        entity.TrangThai = request.TrangThai;
        _db.Entry(entity).Property("MaSinhVien").CurrentValue = request.MaSinhVien;
        _db.Entry(entity).Property("MaPhong").CurrentValue = request.MaPhong;

        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            return Forbid();
        }

        var entity = await _db.HopDongs.FirstOrDefaultAsync(x => x.MaHopDong == id, cancellationToken);
        if (entity == null) return NotFound();

        _db.HopDongs.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}

public sealed class HopDongUpsertRequest
{
    public int MaHopDong { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public decimal TienDatCoc { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public int MaSinhVien { get; set; }
    public int MaPhong { get; set; }
}

public sealed class HopDongResponse
{
    public int MaHopDong { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public decimal TienDatCoc { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public int MaSinhVien { get; set; }
    public int MaPhong { get; set; }
}