using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_QLTTT.Controllers;

public class NguoiDungController : AdminControllerBase
{
    private readonly INguoiDungRepository _repository;

    public NguoiDungController(INguoiDungRepository repository)
    {
        _repository = repository;
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

    public IActionResult Create()
    {
        return View(BuildForm(new NguoiDungFormViewModel { VaiTro = AppStatuses.VaiTro.Staff, DangHoatDong = true }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NguoiDungFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(BuildForm(model));
        }

        await _repository.AddAsync(ToEntity(model));
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : View(BuildForm(ToForm(entity)));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NguoiDungFormViewModel model)
    {
        if (id != model.MaNguoiDung)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(BuildForm(model));
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

    private static NguoiDungFormViewModel BuildForm(NguoiDungFormViewModel model)
    {
        model.VaiTroOptions = AppStatuses.VaiTro.All.Select(x => new SelectListItem(x, x, x == model.VaiTro));
        return model;
    }

    private static NguoiDung ToEntity(NguoiDungFormViewModel model) => new()
    {
        MaNguoiDung = model.MaNguoiDung,
        TenDangNhap = model.TenDangNhap,
        MatKhau = model.MatKhau,
        HoTen = model.HoTen,
        VaiTro = model.VaiTro,
        DangHoatDong = model.DangHoatDong
    };

    private static NguoiDungFormViewModel ToForm(NguoiDung entity) => new()
    {
        MaNguoiDung = entity.MaNguoiDung,
        TenDangNhap = entity.TenDangNhap,
        MatKhau = entity.MatKhau,
        HoTen = entity.HoTen,
        VaiTro = entity.VaiTro,
        DangHoatDong = entity.DangHoatDong
    };
}
