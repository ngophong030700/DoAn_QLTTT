using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_QLTTT.Controllers;

public class HopDongController : AdminControllerBase
{
    private readonly IHopDongRepository _repository;
    private readonly IPhongTroRepository _phongTroRepository;
    private readonly IKhachThueRepository _khachThueRepository;

    public HopDongController(
        IHopDongRepository repository,
        IPhongTroRepository phongTroRepository,
        IKhachThueRepository khachThueRepository)
    {
        _repository = repository;
        _phongTroRepository = phongTroRepository;
        _khachThueRepository = khachThueRepository;
    }

    public async Task<IActionResult> Index(string? keyword)
    {
        ViewBag.Keyword = keyword;
        return View(await _repository.GetAllAsync(keyword));
    }

    public async Task<IActionResult> Details(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : View(entity);
    }

    public async Task<IActionResult> Create()
    {
        return View(await BuildFormAsync(new HopDongFormViewModel
        {
            TrangThai = AppStatuses.HopDong.HieuLuc,
            NgayBatDau = DateOnly.FromDateTime(DateTime.Today)
        }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HopDongFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(await BuildFormAsync(model));
        }

        await _repository.AddAsync(ToEntity(model));
        // TODO: DB thật nên có trigger cập nhật PhongTro.TrangThai = 'Đang thuê' khi lập hợp đồng hiệu lực.
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : View(await BuildFormAsync(ToForm(entity)));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, HopDongFormViewModel model)
    {
        if (id != model.MaHopDong)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(await BuildFormAsync(model));
        }

        await _repository.UpdateAsync(ToEntity(model));
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : View(entity);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task<HopDongFormViewModel> BuildFormAsync(HopDongFormViewModel model)
    {
        model.PhongOptions = (await _phongTroRepository.GetAllAsync())
            .Select(x => new SelectListItem($"{x.SoPhong} - {x.TrangThai}", x.MaPhong.ToString(), x.MaPhong == model.MaPhong));
        model.KhachOptions = (await _khachThueRepository.GetAllAsync())
            .Select(x => new SelectListItem($"{x.HoTen} ({x.SoDienThoai})", x.MaKhach.ToString(), x.MaKhach == model.MaKhachDaiDien));
        model.TrangThaiOptions = AppStatuses.HopDong.All.Select(x => new SelectListItem(x, x, x == model.TrangThai));
        return model;
    }

    private static HopDong ToEntity(HopDongFormViewModel model) => new()
    {
        MaHopDong = model.MaHopDong,
        MaPhong = model.MaPhong,
        MaKhachDaiDien = model.MaKhachDaiDien,
        NgayBatDau = model.NgayBatDau,
        NgayKetThuc = model.NgayKetThuc,
        TienThueThang = model.TienThueThang,
        TienCoc = model.TienCoc,
        TrangThai = model.TrangThai
    };

    private static HopDongFormViewModel ToForm(HopDong entity) => new()
    {
        MaHopDong = entity.MaHopDong,
        MaPhong = entity.MaPhong,
        MaKhachDaiDien = entity.MaKhachDaiDien,
        NgayBatDau = entity.NgayBatDau,
        NgayKetThuc = entity.NgayKetThuc,
        TienThueThang = entity.TienThueThang,
        TienCoc = entity.TienCoc,
        TrangThai = entity.TrangThai
    };
}
