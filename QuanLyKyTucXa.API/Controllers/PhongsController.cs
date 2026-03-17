using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.Domain.Entities;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,NhanVien")]
public sealed class PhongsController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public PhongsController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Phong>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _db.Phongs.AsNoTracking().ToListAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Phong>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.Phongs.AsNoTracking().FirstOrDefaultAsync(x => x.MaPhong == id, cancellationToken);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Phong entity, CancellationToken cancellationToken)
    {
        _db.Phongs.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaPhong }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Phong request, CancellationToken cancellationToken)
    {
        var entity = await _db.Phongs.FirstOrDefaultAsync(x => x.MaPhong == id, cancellationToken);
        if (entity == null) return NotFound();

        entity.SoPhong = request.SoPhong;
        entity.LoaiPhong = request.LoaiPhong;
        entity.SucChua = request.SucChua;
        entity.GiaPhong = request.GiaPhong;
        entity.TrangThai = request.TrangThai;

        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.Phongs.FirstOrDefaultAsync(x => x.MaPhong == id, cancellationToken);
        if (entity == null) return NotFound();

        _db.Phongs.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}