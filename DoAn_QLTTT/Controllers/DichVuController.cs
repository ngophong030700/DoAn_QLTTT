using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_QLTTT.Controllers;

public class DichVuController : AdminControllerBase
{
    private readonly IDichVuRepository _repository;

    public DichVuController(IDichVuRepository repository)
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
        return View(new DichVuFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DichVuFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _repository.AddAsync(ToEntity(model));
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : View(ToForm(entity));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DichVuFormViewModel model)
    {
        if (id != model.MaDichVu)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
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

    private static DichVu ToEntity(DichVuFormViewModel model) => new()
    {
        MaDichVu = model.MaDichVu,
        TenDichVu = model.TenDichVu,
        DonVi = model.DonVi,
        DonGia = model.DonGia,
        LoaiTinhPhi = model.LoaiTinhPhi
    };

    private static DichVuFormViewModel ToForm(DichVu entity) => new()
    {
        MaDichVu = entity.MaDichVu,
        TenDichVu = entity.TenDichVu,
        DonVi = entity.DonVi,
        DonGia = entity.DonGia,
        LoaiTinhPhi = entity.LoaiTinhPhi
    };
}
