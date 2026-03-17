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
public sealed class SinhViensController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public SinhViensController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<SinhVien>>> GetAll(CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            var own = await _db.SinhViens.AsNoTracking()
                .Where(x => x.MaSinhVien == maSinhVien.Value)
                .ToListAsync(cancellationToken);

            return Ok(own);
        }

        return Ok(await _db.SinhViens.AsNoTracking().ToListAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SinhVien>> GetById(int id, CancellationToken cancellationToken)
    {
        if (User.LaSinhVien() && User.LayMaSinhVien() != id)
        {
            return Forbid();
        }

        var entity = await _db.SinhViens.AsNoTracking().FirstOrDefaultAsync(x => x.MaSinhVien == id, cancellationToken);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SinhVien entity, CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            return Forbid();
        }

        _db.SinhViens.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaSinhVien }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SinhVien request, CancellationToken cancellationToken)
    {
        if (User.LaSinhVien() && User.LayMaSinhVien() != id)
        {
            return Forbid();
        }

        var entity = await _db.SinhViens.FirstOrDefaultAsync(x => x.MaSinhVien == id, cancellationToken);
        if (entity == null) return NotFound();

        entity.HoTen = request.HoTen;
        entity.NgaySinh = request.NgaySinh;
        entity.GioiTinh = request.GioiTinh;
        entity.SoDienThoai = request.SoDienThoai;
        entity.Email = request.Email;
        entity.CCCD = request.CCCD;
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

        var entity = await _db.SinhViens.FirstOrDefaultAsync(x => x.MaSinhVien == id, cancellationToken);
        if (entity == null) return NotFound();

        _db.SinhViens.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}