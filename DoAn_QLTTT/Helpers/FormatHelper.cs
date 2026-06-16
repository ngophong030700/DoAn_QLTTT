using System.Globalization;

namespace DoAn_QLTTT.Helpers;

public static class FormatHelper
{
    private static readonly CultureInfo Vi = CultureInfo.GetCultureInfo("vi-VN");

    public static string Money(decimal value) => value.ToString("C0", Vi);

    public static string Number(decimal value) => value.ToString("#,##0.##", Vi);
}
