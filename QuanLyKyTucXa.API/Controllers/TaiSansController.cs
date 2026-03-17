using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.Domain.Entities;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,NhanVien")]
public sealed class TaiSansController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public TaiSansController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaiSan>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _db.TaiSans.AsNoTracking().ToListAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaiSan>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.TaiSans.AsNoTracking().FirstOrDefaultAsync(x => x.MaTaiSan == id, cancellationToken);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaiSan entity, CancellationToken cancellationToken)
    {
        _db.TaiSans.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaTaiSan }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaiSan request, CancellationToken cancellationToken)
    {
        var entity = await _db.TaiSans.FirstOrDefaultAsync(x => x.MaTaiSan == id, cancellationToken);
        if (entity == null) return NotFound();

        entity.TenTaiSan = request.TenTaiSan;
        entity.TinhTrang = request.TinhTrang;
        entity.NgayMua = request.NgayMua;

        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.TaiSans.FirstOrDefaultAsync(x => x.MaTaiSan == id, cancellationToken);
        if (entity == null) return NotFound();

        _db.TaiSans.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}