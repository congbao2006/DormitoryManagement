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
public sealed class HoaDonsController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public HoaDonsController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<HoaDon>>> GetAll(CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            var own = await _db.HoaDons.AsNoTracking()
                .Where(x => EF.Property<int>(x, "MaSinhVien") == maSinhVien.Value)
                .ToListAsync(cancellationToken);

            return Ok(own);
        }

        return Ok(await _db.HoaDons.AsNoTracking().ToListAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HoaDon>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _db.HoaDons.AsNoTracking()
            .Where(x => x.MaHoaDon == id)
            .Select(x => new
            {
                Entity = x,
                MaSinhVien = EF.Property<int>(x, "MaSinhVien")
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue || result.MaSinhVien != maSinhVien.Value)
            {
                return Forbid();
            }
        }

        return Ok(result.Entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HoaDon entity, CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            return Forbid();
        }

        _db.HoaDons.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaHoaDon }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] HoaDon request, CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            return Forbid();
        }

        var entity = await _db.HoaDons.FirstOrDefaultAsync(x => x.MaHoaDon == id, cancellationToken);
        if (entity == null) return NotFound();

        entity.NgayThanhToan = request.NgayThanhToan;
        entity.TongTien = request.TongTien;
        entity.TrangThai = request.TrangThai;

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

        var entity = await _db.HoaDons.FirstOrDefaultAsync(x => x.MaHoaDon == id, cancellationToken);
        if (entity == null) return NotFound();

        _db.HoaDons.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}