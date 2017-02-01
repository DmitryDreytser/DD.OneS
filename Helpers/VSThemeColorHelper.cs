// Decompiled with JetBrains decompiler
// Type: DD.OneS.Helpers.VSThemeColorHelper
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using Microsoft.VisualStudio.PlatformUI;

namespace DD.OneS.Helpers
{
  internal class VSThemeColorHelper
  {
    public const string RED = "#FF8080";
    public const string GREEN = "#139734";
    public const string BLUE = "#007ACC";
    public const string LIGHTBLUE = "#0080FF";
    public const string GREY = "#c0c0c0";
    public const string BLACK = "#000000";
    private static VSThemeColorHelper.Theme s_theme;

    public static VSThemeColorHelper.Theme CurrentTheme
    {
      get
      {
        return VSThemeColorHelper.s_theme;
      }
    }

    static VSThemeColorHelper()
    {
      VSThemeColorHelper.CalculateTheme();
    }

    private static void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
    {
      VSThemeColorHelper.CalculateTheme();
    }

    private static void CalculateTheme()
    {
      switch (VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey).Name)
      {
        case "ff252526":
          VSThemeColorHelper.s_theme = VSThemeColorHelper.Theme.Dark;
          break;
        case "ffffffff":
          VSThemeColorHelper.s_theme = VSThemeColorHelper.Theme.Blue;
          break;
        case "fff5f5f5":
          VSThemeColorHelper.s_theme = VSThemeColorHelper.Theme.Light;
          break;
        default:
          VSThemeColorHelper.s_theme = VSThemeColorHelper.Theme.UNKNOWN;
          break;
      }
    }

    public enum Theme
    {
      Dark,
      Light,
      Blue,
      UNKNOWN,
    }
  }
}
