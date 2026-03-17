using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyKyTucXa.Application.DTOs;
using QuanLyKyTucXa.Application.Features.QuanLyKyTucXa;

namespace QuanLyKyTucXa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,NhanVien")]
public sealed class QuanLyKyTucXaController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuanLyKyTucXaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("sinh-vien")]
    public async Task<ActionResult<List<SinhVienDto>>> LayDanhSachSinhVien(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LayDanhSachSinhVienQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("sinh-vien")]
    public async Task<IActionResult> ThemSinhVien([FromBody] SinhVienDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ThemSinhVienCommand(dto), cancellationToken);
        return Ok();
    }

    [HttpPut("sinh-vien/{maPhong:int}")]
    public async Task<IActionResult> CapNhatSinhVien(int maPhong, [FromBody] SinhVienDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CapNhatSinhVienCommand(dto, maPhong), cancellationToken);
        return Ok();
    }

    [HttpDelete("sinh-vien/{maSinhVien:int}")]
    public async Task<IActionResult> XoaSinhVien(int maSinhVien, CancellationToken cancellationToken)
    {
        await _mediator.Send(new XoaSinhVienCommand(maSinhVien), cancellationToken);
        return NoContent();
    }

    [HttpGet("phong")]
    public async Task<ActionResult<List<PhongDto>>> LayDanhSachPhongConChoi(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LayDanhSachPhongConChoiQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("phong")]
    public async Task<IActionResult> ThemPhong([FromBody] PhongDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ThemPhongCommand(dto), cancellationToken);
        return Ok();
    }

    [HttpDelete("phong/{maPhong:int}")]
    public async Task<IActionResult> XoaPhong(int maPhong, CancellationToken cancellationToken)
    {
        await _mediator.Send(new XoaPhongCommand(maPhong), cancellationToken);
        return NoContent();
    }

    [HttpGet("hop-dong")]
    public async Task<ActionResult<List<HopDongDto>>> LayDanhSachHopDong(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LayDanhSachHopDongQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("hop-dong")]
    public async Task<IActionResult> ThemHopDong([FromBody] HopDongDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ThemHopDongCommand(dto), cancellationToken);
        return Ok();
    }

    [HttpDelete("hop-dong/{maHopDong:int}")]
    public async Task<IActionResult> XoaHopDong(int maHopDong, CancellationToken cancellationToken)
    {
        await _mediator.Send(new XoaHopDongCommand(maHopDong), cancellationToken);
        return NoContent();
    }

    [HttpGet("hoa-don")]
    public async Task<ActionResult<List<HoaDonDto>>> LayDanhSachHoaDon(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LayDanhSachHoaDonQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("hoa-don")]
    public async Task<IActionResult> ThemHoaDon([FromBody] HoaDonDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ThemHoaDonCommand(dto), cancellationToken);
        return Ok();
    }

    [HttpDelete("hoa-don/{maHoaDon:int}")]
    public async Task<IActionResult> XoaHoaDon(int maHoaDon, CancellationToken cancellationToken)
    {
        await _mediator.Send(new XoaHoaDonCommand(maHoaDon), cancellationToken);
        return NoContent();
    }
}