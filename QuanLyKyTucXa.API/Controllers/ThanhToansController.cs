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
public sealed class ThanhToansController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public ThanhToansController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ThanhToan>>> GetAll(CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            var own = await _db.ThanhToans.AsNoTracking()
                .Where(t => _db.HoaDons.Any(h =>
                    h.MaHoaDon == EF.Property<int>(t, "MaHoaDon") &&
                    EF.Property<int>(h, "MaSinhVien") == maSinhVien.Value))
                .ToListAsync(cancellationToken);

            return Ok(own);
        }

        return Ok(await _db.ThanhToans.AsNoTracking().ToListAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ThanhToan>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _db.ThanhToans.AsNoTracking()
            .Where(x => x.MaThanhToan == id)
            .Select(x => new
            {
                Entity = x,
                MaHoaDon = EF.Property<int>(x, "MaHoaDon")
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            var duocPhep = await _db.HoaDons.AnyAsync(h =>
                h.MaHoaDon == result.MaHoaDon &&
                EF.Property<int>(h, "MaSinhVien") == maSinhVien.Value,
                cancellationToken);

            if (!duocPhep)
            {
                return Forbid();
            }
        }

        return Ok(result.Entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ThanhToan entity, CancellationToken cancellationToken)
    {
        if (!_db.Entry(entity).Property("MaHoaDon").Metadata.IsShadowProperty())
        {
            return BadRequest(new { message = "MaHoaDon khong hop le." });
        }

        var maHoaDonObj = Request.Query["maHoaDon"].ToString();
        if (!int.TryParse(maHoaDonObj, out var maHoaDon))
        {
            return BadRequest(new { message = "Vui long truyen maHoaDon tren query string." });
        }

        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            var hopLe = await _db.HoaDons.AnyAsync(h =>
                h.MaHoaDon == maHoaDon &&
                EF.Property<int>(h, "MaSinhVien") == maSinhVien.Value,
                cancellationToken);

            if (!hopLe)
            {
                return Forbid();
            }
        }

        _db.Entry(entity).Property("MaHoaDon").CurrentValue = maHoaDon;
        _db.ThanhToans.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaThanhToan }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ThanhToan request, CancellationToken cancellationToken)
    {
        var entity = await _db.ThanhToans.FirstOrDefaultAsync(x => x.MaThanhToan == id, cancellationToken);
        if (entity == null) return NotFound();

        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            var maHoaDon = _db.Entry(entity).Property<int>("MaHoaDon").CurrentValue;
            var duocPhep = await _db.HoaDons.AnyAsync(h =>
                h.MaHoaDon == maHoaDon &&
                EF.Property<int>(h, "MaSinhVien") == maSinhVien.Value,
                cancellationToken);

            if (!duocPhep)
            {
                return Forbid();
            }
        }

        entity.NgayThanhToan = request.NgayThanhToan;
        entity.SoTien = request.SoTien;
        entity.PhuongThuc = request.PhuongThuc;

        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.ThanhToans.FirstOrDefaultAsync(x => x.MaThanhToan == id, cancellationToken);
        if (entity == null) return NotFound();

        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            var maHoaDon = _db.Entry(entity).Property<int>("MaHoaDon").CurrentValue;
            var duocPhep = await _db.HoaDons.AnyAsync(h =>
                h.MaHoaDon == maHoaDon &&
                EF.Property<int>(h, "MaSinhVien") == maSinhVien.Value,
                cancellationToken);

            if (!duocPhep)
            {
                return Forbid();
            }
        }

        _db.ThanhToans.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}