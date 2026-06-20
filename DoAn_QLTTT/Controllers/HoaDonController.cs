using DoAn_QLTTT.Helpers;
using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_QLTTT.Controllers;

public class HoaDonController : AdminControllerBase
{
    private readonly IHoaDonRepository _repository;
    private readonly IHopDongRepository _hopDongRepository;
    private readonly INguoiDungRepository _nguoiDungRepository;
    private readonly IThanhToanRepository _thanhToanRepository;

    public HoaDonController(
        IHoaDonRepository repository,
        IHopDongRepository hopDongRepository,
        INguoiDungRepository nguoiDungRepository,
        IThanhToanRepository thanhToanRepository)
    {
        _repository = repository;
        _hopDongRepository = hopDongRepository;
        _nguoiDungRepository = nguoiDungRepository;
        _thanhToanRepository = thanhToanRepository;
    }

    public async Task<IActionResult> Index(string? keyword)
    {
        ViewBag.Keyword = keyword;
        return View(await _repository.GetAllAsync(keyword));
    }

    public async Task<IActionResult> Details(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
        {
            return NotFound();
        }

        return View(new HoaDonDetailsViewModel
        {
            HoaDon = entity,
            ChiTietHoaDons = await _repository.GetChiTietAsync(id),
            ThanhToans = await _thanhToanRepository.GetByHoaDonAsync(id)
        });
    }

    public async Task<IActionResult> Create()
    {
        return View(await BuildFormAsync(new HoaDonFormViewModel
        {
            MaNguoiLap = 2,
            TrangThai = AppStatuses.HoaDon.ChuaThanhToan
        }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HoaDonFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(await BuildFormAsync(model));
        }

        await _repository.AddAsync(ToEntity(model));
        // TODO: DB thật sẽ tính tổng từ ChiTietHoaDon qua trigger sau khi thêm dòng chi tiết.
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? NotFound() : View(await BuildFormAsync(ToForm(entity)));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, HoaDonFormViewModel model)
    {
        if (id != model.MaHoaDon)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(await BuildFormAsync(model));
        }

        var current = await _repository.GetByIdAsync(id);
        if (current is null)
        {
            return NotFound();
        }

        var updated = ToEntity(model);
        updated.TongTien = current.TongTien;
        updated.DaThanhToan = current.DaThanhToan;
        updated.ConLai = current.ConLai;
        await _repository.UpdateAsync(updated);
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

    private async Task<HoaDonFormViewModel> BuildFormAsync(HoaDonFormViewModel model)
    {
        model.HopDongOptions = (await _hopDongRepository.GetAllAsync())
            .Select(x => new SelectListItem($"HĐ #{x.MaHopDong} - Phòng {x.SoPhong} - {x.KhachDaiDienHoTen}", x.MaHopDong.ToString(), x.MaHopDong == model.MaHopDong));
        model.NguoiDungOptions = (await _nguoiDungRepository.GetAllAsync())
            .Select(x => new SelectListItem(x.HoTen, x.MaNguoiDung.ToString(), x.MaNguoiDung == model.MaNguoiLap));
        model.TrangThaiOptions = AppStatuses.HoaDon.All.Select(x => new SelectListItem(
            x == AppStatuses.HoaDon.MotPhan ? "Thanh toán một phần" : BadgeHelper.HoaDonStatusText(x),
            x,
            IsSameInvoiceStatus(x, model.TrangThai)));
        return model;
    }

    private static bool IsSameInvoiceStatus(string expected, string? current)
    {
        var normalized = BadgeHelper.HoaDonStatusText(current);
        return expected switch
        {
            AppStatuses.HoaDon.ChuaThanhToan => normalized == "Chưa thanh toán",
            AppStatuses.HoaDon.MotPhan => normalized == "Thanh toán một phần",
            AppStatuses.HoaDon.DaThanhToan => normalized == "Đã thanh toán",
            AppStatuses.HoaDon.QuaHan => normalized == "Quá hạn",
            _ => false
        };
    }

    private static HoaDon ToEntity(HoaDonFormViewModel model) => new()
    {
        MaHoaDon = model.MaHoaDon,
        MaHopDong = model.MaHopDong,
        MaNguoiLap = model.MaNguoiLap,
        Thang = model.Thang,
        Nam = model.Nam,
        HanThanhToan = model.HanThanhToan,
        TrangThai = model.TrangThai
    };

    private static HoaDonFormViewModel ToForm(HoaDon entity) => new()
    {
        MaHoaDon = entity.MaHoaDon,
        MaHopDong = entity.MaHopDong,
        MaNguoiLap = entity.MaNguoiLap,
        Thang = entity.Thang,
        Nam = entity.Nam,
        HanThanhToan = entity.HanThanhToan,
        TrangThai = entity.TrangThai
    };
}
