using Microsoft.EntityFrameworkCore;
using QuanLyKyTucXa.Application.Abstractions.Repositories;
using QuanLyKyTucXa.Domain.Entities;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.Infrastructure.Repositories;

public sealed class QuanLyKyTucXaRepository : IQuanLyKyTucXaRepository
{
    private readonly QuanLyKyTucXaDbContext _dbContext;

    public QuanLyKyTucXaRepository(QuanLyKyTucXaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<SinhVien>> LayDanhSachSinhVienAsync(CancellationToken cancellationToken)
        => _dbContext.SinhViens.AsNoTracking().ToListAsync(cancellationToken);

    public Task<List<Phong>> LayDanhSachPhongAsync(CancellationToken cancellationToken)
        => _dbContext.Phongs.AsNoTracking().ToListAsync(cancellationToken);

    public Task<List<HopDong>> LayDanhSachHopDongAsync(CancellationToken cancellationToken)
        => _dbContext.HopDongs.AsNoTracking().ToListAsync(cancellationToken);

    public Task<List<HoaDon>> LayDanhSachHoaDonAsync(CancellationToken cancellationToken)
        => _dbContext.HoaDons.AsNoTracking().ToListAsync(cancellationToken);

    public Task<SinhVien?> TimSinhVienAsync(int maSinhVien, CancellationToken cancellationToken)
        => _dbContext.SinhViens.FirstOrDefaultAsync(x => x.MaSinhVien == maSinhVien, cancellationToken);

    public Task<Phong?> TimPhongAsync(int maPhong, CancellationToken cancellationToken)
        => _dbContext.Phongs.FirstOrDefaultAsync(x => x.MaPhong == maPhong, cancellationToken);

    public Task<HopDong?> TimHopDongAsync(int maHopDong, CancellationToken cancellationToken)
        => _dbContext.HopDongs.FirstOrDefaultAsync(x => x.MaHopDong == maHopDong, cancellationToken);

    public Task<HoaDon?> TimHoaDonAsync(int maHoaDon, CancellationToken cancellationToken)
        => _dbContext.HoaDons.FirstOrDefaultAsync(x => x.MaHoaDon == maHoaDon, cancellationToken);

    public async Task ThemSinhVienAsync(SinhVien sv, CancellationToken cancellationToken)
    {
        await _dbContext.SinhViens.AddAsync(sv, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CapNhatSinhVienAsync(SinhVien sv, CancellationToken cancellationToken)
    {
        _dbContext.SinhViens.Update(sv);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task XoaSinhVienAsync(int maSinhVien, CancellationToken cancellationToken)
    {
        var entity = await TimSinhVienAsync(maSinhVien, cancellationToken);
        if (entity == null) return;

        _dbContext.SinhViens.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ThemPhongAsync(Phong phong, CancellationToken cancellationToken)
    {
        await _dbContext.Phongs.AddAsync(phong, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task XoaPhongAsync(int maPhong, CancellationToken cancellationToken)
    {
        var entity = await TimPhongAsync(maPhong, cancellationToken);
        if (entity == null) return;

        _dbContext.Phongs.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ThemHopDongAsync(HopDong hopDong, CancellationToken cancellationToken)
    {
        await _dbContext.HopDongs.AddAsync(hopDong, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task XoaHopDongAsync(int maHopDong, CancellationToken cancellationToken)
    {
        var entity = await TimHopDongAsync(maHopDong, cancellationToken);
        if (entity == null) return;

        _dbContext.HopDongs.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ThemHoaDonAsync(HoaDon hoaDon, CancellationToken cancellationToken)
    {
        await _dbContext.HoaDons.AddAsync(hoaDon, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task XoaHoaDonAsync(int maHoaDon, CancellationToken cancellationToken)
    {
        var entity = await TimHoaDonAsync(maHoaDon, cancellationToken);
        if (entity == null) return;

        _dbContext.HoaDons.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}