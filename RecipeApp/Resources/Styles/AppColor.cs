
static class AppColors {
  public static Color primary => Color.FromUint(0xff7AC5C1);
  public static Color primaryLighter = Color.FromUint(0xffE6F9FF);
  public static Color white = Color.FromUint(0xffffffff);
  public static Color realBlack = Color.FromUint(0xff000000);
  public static Color text = Color.FromUint(0xff0F1E31);
  public static Color black = Color.FromUint(0xff0F1E31);
  public static Color blackLight = Color.FromUint(0xff1B2C41);
  public static Color orangeDark = Color.FromUint(0xffCE5A01);
  public static Color yellow = Color.FromUint(0xffFFEF7D);
  public static Color sugar = Color.FromUint(0xffFBF5E9);
  public static Color honey = Color.FromUint(0xffDA7C16);
  public static Color pinkLight = Color.FromUint(0xffF9B7B6);
  public static Color green = Color.FromUint(0xffADBE56);
  public static Color red = Color.FromUint(0xffCF252F);

//   static MaterialColor primaryMaterialColor =
//       getMaterialColorFromColor(AppColors.primary);

//   public static Color getShade(Color color, {bool darker = false, double value = .1}) {
//     assert(value >= 0 && value <= 1);

//     final hsl = HSLColor.fromColor(color);
//     final hslDark = hsl.withLightness(
//         (darker ? (hsl.lightness - value) : (hsl.lightness + value))
//             .clamp(0.0, 1.0));

//     return hslDark.toColor();
//   }

//   static MaterialColor getMaterialColorFromColor(Color color) {
//     Map<int, Color> colorShades = {
//       50: getShade(color, value: 0.5),
//       100: getShade(color, value: 0.4),
//       200: getShade(color, value: 0.3),
//       300: getShade(color, value: 0.2),
//       400: getShade(color, value: 0.1),
//       500: color, //Primary value
//       600: getShade(color, value: 0.1, darker: true),
//       700: getShade(color, value: 0.15, darker: true),
//       800: getShade(color, value: 0.2, darker: true),
//       900: getShade(color, value: 0.25, darker: true),
//     };
//     return MaterialColor(color.value, colorShades);
//   }

//   public static Color textColorFromBackground(Color background) =>
//       background.computeLuminance() > 0.3 ? Colors.black : Colors.white;

//   static Brightness getBrightness(Color color) {
//     final double relativeLuminance = color.computeLuminance();
//     const double kThreshold = 0.15;
//     return ((relativeLuminance + 0.05) * (relativeLuminance + 0.05) >
//             kThreshold)
//         ? Brightness.light
//         : Brightness.dark;
//   }
}
