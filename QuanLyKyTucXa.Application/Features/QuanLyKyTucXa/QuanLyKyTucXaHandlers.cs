using AutoMapper;
using MediatR;
using QuanLyKyTucXa.Application.Abstractions.Repositories;
using QuanLyKyTucXa.Application.Abstractions.Services;
using QuanLyKyTucXa.Application.Common.Exceptions;
using QuanLyKyTucXa.Application.DTOs;
using QuanLyKyTucXa.Domain.Entities;

namespace QuanLyKyTucXa.Application.Features.QuanLyKyTucXa;

public sealed class ThemSinhVienCommandHandler : IRequestHandler<ThemSinhVienCommand>
{
    private readonly IQuanLyKyTucXaService _service;
    private readonly IMapper _mapper;

    public ThemSinhVienCommandHandler(IQuanLyKyTucXaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task Handle(ThemSinhVienCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<SinhVien>(request.SinhVien);
        await _service.ThemSinhVienAsync(entity, cancellationToken);
    }
}

public sealed class XoaSinhVienCommandHandler : IRequestHandler<XoaSinhVienCommand>
{
    private readonly IQuanLyKyTucXaService _service;

    public XoaSinhVienCommandHandler(IQuanLyKyTucXaService service)
    {
        _service = service;
    }

    public async Task Handle(XoaSinhVienCommand request, CancellationToken cancellationToken)
    {
        await _service.XoaSinhVienAsync(request.MaSinhVien, cancellationToken);
    }
}

public sealed class CapNhatSinhVienCommandHandler : IRequestHandler<CapNhatSinhVienCommand>
{
    private readonly IQuanLyKyTucXaService _service;
    private readonly IQuanLyKyTucXaRepository _repository;
    private readonly IMapper _mapper;

    public CapNhatSinhVienCommandHandler(
        IQuanLyKyTucXaService service,
        IQuanLyKyTucXaRepository repository,
        IMapper mapper)
    {
        _service = service;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Handle(CapNhatSinhVienCommand request, CancellationToken cancellationToken)
    {
        var phong = await _repository.TimPhongAsync(request.MaPhong, cancellationToken);
        if (phong == null)
        {
            throw new NotFoundException($"Khong tim thay phong {request.MaPhong}.");
        }

        var sinhVien = _mapper.Map<SinhVien>(request.SinhVien);
        await _service.CapNhatSinhVienAsync(sinhVien, phong, cancellationToken);
    }
}

public sealed class ThemPhongCommandHandler : IRequestHandler<ThemPhongCommand>
{
    private readonly IQuanLyKyTucXaService _service;
    private readonly IMapper _mapper;

    public ThemPhongCommandHandler(IQuanLyKyTucXaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task Handle(ThemPhongCommand request, CancellationToken cancellationToken)
    {
        var phong = _mapper.Map<Phong>(request.Phong);
        await _service.ThemPhongAsync(phong, cancellationToken);
    }
}

public sealed class XoaPhongCommandHandler : IRequestHandler<XoaPhongCommand>
{
    private readonly IQuanLyKyTucXaService _service;

    public XoaPhongCommandHandler(IQuanLyKyTucXaService service)
    {
        _service = service;
    }

    public async Task Handle(XoaPhongCommand request, CancellationToken cancellationToken)
    {
        await _service.XoaPhongAsync(request.MaPhong, cancellationToken);
    }
}

public sealed class ThemHopDongCommandHandler : IRequestHandler<ThemHopDongCommand>
{
    private readonly IQuanLyKyTucXaService _service;
    private readonly IMapper _mapper;

    public ThemHopDongCommandHandler(IQuanLyKyTucXaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task Handle(ThemHopDongCommand request, CancellationToken cancellationToken)
    {
        var hopDong = _mapper.Map<HopDong>(request.HopDong);
        await _service.ThemHopDongAsync(hopDong, cancellationToken);
    }
}

public sealed class XoaHopDongCommandHandler : IRequestHandler<XoaHopDongCommand>
{
    private readonly IQuanLyKyTucXaService _service;

    public XoaHopDongCommandHandler(IQuanLyKyTucXaService service)
    {
        _service = service;
    }

    public async Task Handle(XoaHopDongCommand request, CancellationToken cancellationToken)
    {
        await _service.XoaHopDongAsync(request.MaHopDong, cancellationToken);
    }
}

public sealed class ThemHoaDonCommandHandler : IRequestHandler<ThemHoaDonCommand>
{
    private readonly IQuanLyKyTucXaService _service;
    private readonly IMapper _mapper;

    public ThemHoaDonCommandHandler(IQuanLyKyTucXaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task Handle(ThemHoaDonCommand request, CancellationToken cancellationToken)
    {
        var hoaDon = _mapper.Map<HoaDon>(request.HoaDon);
        await _service.ThemHoaDonAsync(hoaDon, cancellationToken);
    }
}

public sealed class XoaHoaDonCommandHandler : IRequestHandler<XoaHoaDonCommand>
{
    private readonly IQuanLyKyTucXaService _service;

    public XoaHoaDonCommandHandler(IQuanLyKyTucXaService service)
    {
        _service = service;
    }

    public async Task Handle(XoaHoaDonCommand request, CancellationToken cancellationToken)
    {
        await _service.XoaHoaDonAsync(request.MaHoaDon, cancellationToken);
    }
}

public sealed class QuanLySinhVienCommandHandler : IRequestHandler<QuanLySinhVienCommand>
{
    private readonly IQuanLyKyTucXaService _service;

    public QuanLySinhVienCommandHandler(IQuanLyKyTucXaService service)
    {
        _service = service;
    }

    public async Task Handle(QuanLySinhVienCommand request, CancellationToken cancellationToken)
    {
        await _service.QuanLySinhVienAsync(cancellationToken);
    }
}

public sealed class QuanLyHopDongCommandHandler : IRequestHandler<QuanLyHopDongCommand>
{
    private readonly IQuanLyKyTucXaService _service;

    public QuanLyHopDongCommandHandler(IQuanLyKyTucXaService service)
    {
        _service = service;
    }

    public async Task Handle(QuanLyHopDongCommand request, CancellationToken cancellationToken)
    {
        await _service.QuanLyHopDongAsync(cancellationToken);
    }
}

public sealed class QuanLyHoaDonCommandHandler : IRequestHandler<QuanLyHoaDonCommand>
{
    private readonly IQuanLyKyTucXaService _service;

    public QuanLyHoaDonCommandHandler(IQuanLyKyTucXaService service)
    {
        _service = service;
    }

    public async Task Handle(QuanLyHoaDonCommand request, CancellationToken cancellationToken)
    {
        await _service.QuanLyHoaDonAsync(cancellationToken);
    }
}

public sealed class LayDanhSachPhongConChoiQueryHandler : IRequestHandler<LayDanhSachPhongConChoiQuery, List<PhongDto>>
{
    private readonly IQuanLyKyTucXaService _service;
    private readonly IMapper _mapper;

    public LayDanhSachPhongConChoiQueryHandler(IQuanLyKyTucXaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<List<PhongDto>> Handle(LayDanhSachPhongConChoiQuery request, CancellationToken cancellationToken)
    {
        var phongs = await _service.LayDanhSachPhongConChoiAsync(cancellationToken);
        return _mapper.Map<List<PhongDto>>(phongs);
    }
}

public sealed class LayDanhSachSinhVienQueryHandler : IRequestHandler<LayDanhSachSinhVienQuery, List<SinhVienDto>>
{
    private readonly IQuanLyKyTucXaService _service;
    private readonly IMapper _mapper;

    public LayDanhSachSinhVienQueryHandler(IQuanLyKyTucXaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<List<SinhVienDto>> Handle(LayDanhSachSinhVienQuery request, CancellationToken cancellationToken)
    {
        var sinhViens = await _service.LayDanhSachSinhVienAsync(cancellationToken);
        return _mapper.Map<List<SinhVienDto>>(sinhViens);
    }
}

public sealed class LayDanhSachHopDongQueryHandler : IRequestHandler<LayDanhSachHopDongQuery, List<HopDongDto>>
{
    private readonly IQuanLyKyTucXaService _service;
    private readonly IMapper _mapper;

    public LayDanhSachHopDongQueryHandler(IQuanLyKyTucXaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<List<HopDongDto>> Handle(LayDanhSachHopDongQuery request, CancellationToken cancellationToken)
    {
        var hopDongs = await _service.LayDanhSachHopDongAsync(cancellationToken);
        return _mapper.Map<List<HopDongDto>>(hopDongs);
    }
}

public sealed class LayDanhSachHoaDonQueryHandler : IRequestHandler<LayDanhSachHoaDonQuery, List<HoaDonDto>>
{
    private readonly IQuanLyKyTucXaService _service;
    private readonly IMapper _mapper;

    public LayDanhSachHoaDonQueryHandler(IQuanLyKyTucXaService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<List<HoaDonDto>> Handle(LayDanhSachHoaDonQuery request, CancellationToken cancellationToken)
    {
        var hoaDons = await _service.LayDanhSachHoaDonAsync(cancellationToken);
        return _mapper.Map<List<HoaDonDto>>(hoaDons);
    }
}