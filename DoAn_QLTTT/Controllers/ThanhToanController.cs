using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_QLTTT.Controllers;

public class ThanhToanController : AdminControllerBase
{
    private readonly IThanhToanRepository _repository;
    private readonly IHoaDonRepository _hoaDonRepository;
    private readonly INguoiDungRepository _nguoiDungRepository;

    public ThanhToanController(
        IThanhToanRepository repository,
        IHoaDonRepository hoaDonRepository,
        INguoiDungRepository nguoiDungRepository)
    {
        _repository = repository;
        _hoaDonRepository = hoaDonRepository;
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

    public async Task<IActionResult> Create(int? maHoaDon)
    {
        return View(await BuildFormAsync(new ThanhToanFormViewModel
        {
            MaHoaDon = maHoaDon ?? 0,
            MaNguoiThu = 2
        }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ThanhToanFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(await BuildFormAsync(model));
        }

        await _repository.AddAsync(ToEntity(model));
        // TODO: sp_ThanhToan_Insert sẽ thêm thanh toán; trigger DB cập nhật DaThanhToan, ConLai, TrangThai hóa đơn.
        return RedirectToAction("Details", "HoaDon", new { id = model.MaHoaDon });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : View(await BuildFormAsync(ToForm(entity)));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ThanhToanFormViewModel model)
    {
        if (id != model.MaThanhToan)
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

    private async Task<ThanhToanFormViewModel> BuildFormAsync(ThanhToanFormViewModel model)
    {
        model.HoaDonOptions = (await _hoaDonRepository.GetAllAsync())
            .Select(x => new SelectListItem($"HĐơn #{x.MaHoaDon} - {x.Thang}/{x.Nam} - còn {x.ConLai:n0}", x.MaHoaDon.ToString(), x.MaHoaDon == model.MaHoaDon));
        model.NguoiDungOptions = (await _nguoiDungRepository.GetAllAsync())
            .Select(x => new SelectListItem(x.HoTen, x.MaNguoiDung.ToString(), x.MaNguoiDung == model.MaNguoiThu));
        return model;
    }

    private static ThanhToan ToEntity(ThanhToanFormViewModel model) => new()
    {
        MaThanhToan = model.MaThanhToan,
        MaHoaDon = model.MaHoaDon,
        MaNguoiThu = model.MaNguoiThu,
        SoTien = model.SoTien,
        NgayThu = model.NgayThu,
        HinhThuc = model.HinhThuc
    };

    private static ThanhToanFormViewModel ToForm(ThanhToan entity) => new()
    {
        MaThanhToan = entity.MaThanhToan,
        MaHoaDon = entity.MaHoaDon,
        MaNguoiThu = entity.MaNguoiThu,
        SoTien = entity.SoTien,
        NgayThu = entity.NgayThu,
        HinhThuc = entity.HinhThuc
    };
}
