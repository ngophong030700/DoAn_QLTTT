using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_QLTTT.Controllers;

public class KhachThueController : AdminControllerBase
{
    private readonly IKhachThueRepository _repository;

    public KhachThueController(IKhachThueRepository repository)
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
        return View(new KhachThueFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(KhachThueFormViewModel model)
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
    public async Task<IActionResult> Edit(int id, KhachThueFormViewModel model)
    {
        if (id != model.MaKhach)
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

    private static KhachThue ToEntity(KhachThueFormViewModel model) => new()
    {
        MaKhach = model.MaKhach,
        HoTen = model.HoTen,
        CCCD = model.CCCD,
        SoDienThoai = model.SoDienThoai,
        DiaChi = model.DiaChi
    };

    private static KhachThueFormViewModel ToForm(KhachThue entity) => new()
    {
        MaKhach = entity.MaKhach,
        HoTen = entity.HoTen,
        CCCD = entity.CCCD,
        SoDienThoai = entity.SoDienThoai,
        DiaChi = entity.DiaChi
    };
}
