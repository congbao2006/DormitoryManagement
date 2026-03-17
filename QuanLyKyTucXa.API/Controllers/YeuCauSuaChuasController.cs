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
public sealed class YeuCauSuaChuasController : ControllerBase
{
    private readonly QuanLyKyTucXaDbContext _db;

    public YeuCauSuaChuasController(QuanLyKyTucXaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<YeuCauSuaChua>>> GetAll(CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            var own = await _db.YeuCauSuaChuas.AsNoTracking()
                .Where(x => EF.Property<int>(x, "MaSinhVien") == maSinhVien.Value)
                .ToListAsync(cancellationToken);

            return Ok(own);
        }

        return Ok(await _db.YeuCauSuaChuas.AsNoTracking().ToListAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<YeuCauSuaChua>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _db.YeuCauSuaChuas.AsNoTracking()
            .Where(x => x.MaYeuCau == id)
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
    public async Task<IActionResult> Create([FromBody] YeuCauSuaChua entity, CancellationToken cancellationToken)
    {
        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            if (!maSinhVien.HasValue)
            {
                return Forbid();
            }

            _db.Entry(entity).Property("MaSinhVien").CurrentValue = maSinhVien.Value;
            _db.Entry(entity).Property("MaNhanVien").CurrentValue = null;
        }
        else
        {
            var maSinhVienObj = Request.Query["maSinhVien"].ToString();
            if (!int.TryParse(maSinhVienObj, out var maSinhVien))
            {
                return BadRequest(new { message = "Vui long truyen maSinhVien tren query string." });
            }

            var tonTaiSinhVien = await _db.SinhViens.AnyAsync(x => x.MaSinhVien == maSinhVien, cancellationToken);
            if (!tonTaiSinhVien)
            {
                return BadRequest(new { message = "Ma sinh vien khong ton tai." });
            }

            _db.Entry(entity).Property("MaSinhVien").CurrentValue = maSinhVien;
        }

        _db.YeuCauSuaChuas.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.MaYeuCau }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] YeuCauSuaChua request, CancellationToken cancellationToken)
    {
        var entity = await _db.YeuCauSuaChuas.FirstOrDefaultAsync(x => x.MaYeuCau == id, cancellationToken);
        if (entity == null) return NotFound();

        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            var ownerId = _db.Entry(entity).Property<int>("MaSinhVien").CurrentValue;
            if (!maSinhVien.HasValue || ownerId != maSinhVien.Value)
            {
                return Forbid();
            }
        }

        entity.MoTa = request.MoTa;
        entity.NgayYeuCau = request.NgayYeuCau;
        entity.TrangThai = request.TrangThai;

        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _db.YeuCauSuaChuas.FirstOrDefaultAsync(x => x.MaYeuCau == id, cancellationToken);
        if (entity == null) return NotFound();

        if (User.LaSinhVien())
        {
            var maSinhVien = User.LayMaSinhVien();
            var ownerId = _db.Entry(entity).Property<int>("MaSinhVien").CurrentValue;
            if (!maSinhVien.HasValue || ownerId != maSinhVien.Value)
            {
                return Forbid();
            }
        }

        _db.YeuCauSuaChuas.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}