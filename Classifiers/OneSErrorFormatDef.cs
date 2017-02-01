// Decompiled with JetBrains decompiler
// Type: DD.OneS.Classifiers.OneSErrorFormatDef
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using DD.OneS.Helpers;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace DD.OneS.Classifiers
{
  [Order(Before = "Default Priority")]
  [ClassificationType(ClassificationTypeNames = "OneSError")]
  [UserVisible(false)]
  [Export(typeof (EditorFormatDefinition))]
  [Name("OneSErrorFormat")]
  internal sealed class OneSErrorFormatDef : ClassificationFormatDefinition
  {
    public OneSErrorFormatDef()
    {
      this.DisplayName = "This is a program code";
      this.ForegroundColor = new Color?(VSThemeColorHelper.CurrentTheme == VSThemeColorHelper.Theme.Dark ? (Color) ColorConverter.ConvertFromString("#c0c0c0") : Colors.AntiqueWhite);
      this.BackgroundColor = new Color?(Colors.Red);
      this.IsBold = new bool?(true);
    }
  }
}
