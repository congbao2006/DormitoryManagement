using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.Domain.Entities;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,NhanVien")]
public sealed class ToaNhasController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public ToaNhasController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ToaNha>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _db.ToaNhas.AsNoTracking().ToListAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ToaNha>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.ToaNhas.AsNoTracking().FirstOrDefaultAsync(x => x.MaToa == id, cancellationToken);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ToaNha entity, CancellationToken cancellationToken)
    {
        _db.ToaNhas.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaToa }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ToaNha request, CancellationToken cancellationToken)
    {
        var entity = await _db.ToaNhas.FirstOrDefaultAsync(x => x.MaToa == id, cancellationToken);
        if (entity == null) return NotFound();

        entity.TenToa = request.TenToa;
        entity.SoTang = request.SoTang;

        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.ToaNhas.FirstOrDefaultAsync(x => x.MaToa == id, cancellationToken);
        if (entity == null) return NotFound();

        _db.ToaNhas.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}