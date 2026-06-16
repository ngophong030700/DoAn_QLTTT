using DoAn_QLTTT.Data;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.Repositories.Dapper;
using DoAn_QLTTT.Repositories.Mock;
using DoAn_QLTTT.Services;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var dataProtectionPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtectionKeys");
Directory.CreateDirectory(dataProtectionPath);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionPath))
    .SetApplicationName("DoAn_QLTTT");

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<MockDataService>();
builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<INhacNoService, NhacNoService>();
builder.Services.AddScoped<ISqlScriptReader, SqlScriptReader>();

var dataProvider = builder.Configuration["DataProvider"] ?? "Mock";
if (dataProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
    || dataProvider.Equals("Dapper", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<ISqlDemoService, SqlDemoService>();
    builder.Services.AddScoped<ILoaiPhongRepository, LoaiPhongDapperRepository>();
    builder.Services.AddScoped<IPhongTroRepository, PhongTroDapperRepository>();
    builder.Services.AddScoped<IKhachThueRepository, KhachThueDapperRepository>();
    builder.Services.AddScoped<IHopDongRepository, HopDongDapperRepository>();
    builder.Services.AddScoped<IDichVuRepository, DichVuDapperRepository>();
    builder.Services.AddScoped<IChiSoDienNuocRepository, ChiSoDienNuocDapperRepository>();
    builder.Services.AddScoped<IHoaDonRepository, HoaDonDapperRepository>();
    builder.Services.AddScoped<IThanhToanRepository, ThanhToanDapperRepository>();
    builder.Services.AddScoped<INguoiDungRepository, NguoiDungDapperRepository>();
}
else
{
    builder.Services.AddScoped<ISqlDemoService, MockSqlDemoService>();
    builder.Services.AddScoped<ILoaiPhongRepository, LoaiPhongMockRepository>();
    builder.Services.AddScoped<IPhongTroRepository, PhongTroMockRepository>();
    builder.Services.AddScoped<IKhachThueRepository, KhachThueMockRepository>();
    builder.Services.AddScoped<IHopDongRepository, HopDongMockRepository>();
    builder.Services.AddScoped<IDichVuRepository, DichVuMockRepository>();
    builder.Services.AddScoped<IChiSoDienNuocRepository, ChiSoDienNuocMockRepository>();
    builder.Services.AddScoped<IHoaDonRepository, HoaDonMockRepository>();
    builder.Services.AddScoped<IThanhToanRepository, ThanhToanMockRepository>();
    builder.Services.AddScoped<INguoiDungRepository, NguoiDungMockRepository>();
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
