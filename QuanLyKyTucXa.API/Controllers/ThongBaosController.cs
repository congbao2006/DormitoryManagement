using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.Domain.Entities;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,NhanVien")]
public sealed class ThongBaosController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public ThongBaosController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ThongBao>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _db.ThongBaos.AsNoTracking().ToListAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ThongBao>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.ThongBaos.AsNoTracking().FirstOrDefaultAsync(x => x.MaThongBao == id, cancellationToken);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ThongBao entity, CancellationToken cancellationToken)
    {
        _db.ThongBaos.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaThongBao }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ThongBao request, CancellationToken cancellationToken)
    {
        var entity = await _db.ThongBaos.FirstOrDefaultAsync(x => x.MaThongBao == id, cancellationToken);
        if (entity == null) return NotFound();

        entity.TieuDe = request.TieuDe;
        entity.NoiDung = request.NoiDung;
        entity.NgayTao = request.NgayTao;

        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.ThongBaos.FirstOrDefaultAsync(x => x.MaThongBao == id, cancellationToken);
        if (entity == null) return NotFound();

        _db.ThongBaos.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}