using DoAn_QLTTT.Data;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.Repositories.Dapper;
using DoAn_QLTTT.Services;
using Dapper;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

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
builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<INhacNoService, NhacNoService>();
builder.Services.AddScoped<ISqlScriptReader, SqlScriptReader>();

// Register SQL Server/Dapper repositories unconditionally
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
