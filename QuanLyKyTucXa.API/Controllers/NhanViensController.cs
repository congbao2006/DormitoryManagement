using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.Domain.Entities;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class NhanViensController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public NhanViensController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<NhanVien>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _db.NhanViens.AsNoTracking().ToListAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NhanVien>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.NhanViens.AsNoTracking().FirstOrDefaultAsync(x => x.MaNhanVien == id, cancellationToken);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NhanVien entity, CancellationToken cancellationToken)
    {
        _db.NhanViens.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaNhanVien }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] NhanVien request, CancellationToken cancellationToken)
    {
        var entity = await _db.NhanViens.FirstOrDefaultAsync(x => x.MaNhanVien == id, cancellationToken);
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
        var entity = await _db.NhanViens.FirstOrDefaultAsync(x => x.MaNhanVien == id, cancellationToken);
        if (entity == null) return NotFound();

        _db.NhanViens.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}