using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_QLTTT.Controllers;

public class ChiSoDienNuocController : AdminControllerBase
{
    private readonly IChiSoDienNuocRepository _repository;
    private readonly IPhongTroRepository _phongTroRepository;
    private readonly IDichVuRepository _dichVuRepository;
    private readonly INguoiDungRepository _nguoiDungRepository;

    public ChiSoDienNuocController(
        IChiSoDienNuocRepository repository,
        IPhongTroRepository phongTroRepository,
        IDichVuRepository dichVuRepository,
        INguoiDungRepository nguoiDungRepository)
    {
        _repository = repository;
        _phongTroRepository = phongTroRepository;
        _dichVuRepository = dichVuRepository;
        _nguoiDungRepository = nguoiDungRepository;
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
        return View(await BuildFormAsync(new ChiSoDienNuocFormViewModel { MaNguoiNhap = 2 }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ChiSoDienNuocFormViewModel model)
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
    public async Task<IActionResult> Edit(int id, ChiSoDienNuocFormViewModel model)
    {
        if (id != model.MaChiSo)
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

    private async Task<ChiSoDienNuocFormViewModel> BuildFormAsync(ChiSoDienNuocFormViewModel model)
    {
        model.PhongOptions = (await _phongTroRepository.GetAllAsync())
            .Select(x => new SelectListItem(x.SoPhong, x.MaPhong.ToString(), x.MaPhong == model.MaPhong));
        model.DichVuOptions = (await _dichVuRepository.GetAllAsync())
            .Select(x => new SelectListItem($"{x.TenDichVu} ({x.DonVi})", x.MaDichVu.ToString(), x.MaDichVu == model.MaDichVu));
        model.NguoiDungOptions = (await _nguoiDungRepository.GetAllAsync())
            .Select(x => new SelectListItem(x.HoTen, x.MaNguoiDung.ToString(), x.MaNguoiDung == model.MaNguoiNhap));
        return model;
    }

    private static ChiSoDienNuoc ToEntity(ChiSoDienNuocFormViewModel model) => new()
    {
        MaChiSo = model.MaChiSo,
        MaPhong = model.MaPhong,
        MaDichVu = model.MaDichVu,
        MaNguoiNhap = model.MaNguoiNhap,
        Thang = model.Thang,
        Nam = model.Nam,
        ChiSoCu = model.ChiSoCu,
        ChiSoMoi = model.ChiSoMoi,
        TieuThu = Math.Max(0, model.ChiSoMoi - model.ChiSoCu)
    };

    private static ChiSoDienNuocFormViewModel ToForm(ChiSoDienNuoc entity) => new()
    {
        MaChiSo = entity.MaChiSo,
        MaPhong = entity.MaPhong,
        MaDichVu = entity.MaDichVu,
        MaNguoiNhap = entity.MaNguoiNhap,
        Thang = entity.Thang,
        Nam = entity.Nam,
        ChiSoCu = entity.ChiSoCu,
        ChiSoMoi = entity.ChiSoMoi
    };
}
