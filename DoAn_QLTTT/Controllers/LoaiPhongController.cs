using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_QLTTT.Controllers;

public class LoaiPhongController : AdminControllerBase
{
    private readonly ILoaiPhongRepository _repository;

    public LoaiPhongController(ILoaiPhongRepository repository)
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
        return View(new LoaiPhongFormViewModel { SucChuaToiDa = 1 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LoaiPhongFormViewModel model)
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
    public async Task<IActionResult> Edit(int id, LoaiPhongFormViewModel model)
    {
        if (id != model.MaLoaiPhong)
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

    private static LoaiPhong ToEntity(LoaiPhongFormViewModel model) => new()
    {
        MaLoaiPhong = model.MaLoaiPhong,
        TenLoai = model.TenLoai,
        DienTich = model.DienTich,
        GiaThueCoSo = model.GiaThueCoSo,
        SucChuaToiDa = model.SucChuaToiDa
    };

    private static LoaiPhongFormViewModel ToForm(LoaiPhong entity) => new()
    {
        MaLoaiPhong = entity.MaLoaiPhong,
        TenLoai = entity.TenLoai,
        DienTich = entity.DienTich,
        GiaThueCoSo = entity.GiaThueCoSo,
        SucChuaToiDa = entity.SucChuaToiDa
    };
}
