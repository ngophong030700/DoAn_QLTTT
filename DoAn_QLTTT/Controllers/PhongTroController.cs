using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_QLTTT.Controllers;

public class PhongTroController : AdminControllerBase
{
    private readonly IPhongTroRepository _repository;
    private readonly ILoaiPhongRepository _loaiPhongRepository;

    public PhongTroController(IPhongTroRepository repository, ILoaiPhongRepository loaiPhongRepository)
    {
        _repository = repository;
        _loaiPhongRepository = loaiPhongRepository;
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
        return View(await BuildFormAsync(new PhongTroFormViewModel { TrangThai = AppStatuses.Phong.Trong, SucChuaToiDa = 1 }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PhongTroFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(await BuildFormAsync(model));
        }

        await _repository.AddAsync(ToEntity(model));
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : View(await BuildFormAsync(ToForm(entity)));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PhongTroFormViewModel model)
    {
        if (id != model.MaPhong)
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

    private async Task<PhongTroFormViewModel> BuildFormAsync(PhongTroFormViewModel model)
    {
        model.LoaiPhongOptions = (await _loaiPhongRepository.GetAllAsync())
            .Select(x => new SelectListItem(x.TenLoai, x.MaLoaiPhong.ToString(), x.MaLoaiPhong == model.MaLoaiPhong));
        model.TrangThaiOptions = AppStatuses.Phong.All
            .Select(x => new SelectListItem(x, x, x == model.TrangThai));
        return model;
    }

    private static PhongTro ToEntity(PhongTroFormViewModel model) => new()
    {
        MaPhong = model.MaPhong,
        MaLoaiPhong = model.MaLoaiPhong,
        SoPhong = model.SoPhong,
        GiaThue = model.GiaThue,
        SucChuaToiDa = model.SucChuaToiDa,
        TrangThai = model.TrangThai,
        GhiChu = model.GhiChu
    };

    private static PhongTroFormViewModel ToForm(PhongTro entity) => new()
    {
        MaPhong = entity.MaPhong,
        MaLoaiPhong = entity.MaLoaiPhong,
        SoPhong = entity.SoPhong,
        GiaThue = entity.GiaThue,
        SucChuaToiDa = entity.SucChuaToiDa,
        TrangThai = entity.TrangThai,
        GhiChu = entity.GhiChu
    };
}
