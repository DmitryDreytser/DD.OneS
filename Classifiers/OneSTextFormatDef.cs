﻿// Decompiled with JetBrains decompiler
// Type: DD.OneS.Classifiers.OneSTextFormatDef
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
  [UserVisible(false)]
  [Order(Before = "Default Priority")]
  [Export(typeof (EditorFormatDefinition))]
  [ClassificationType(ClassificationTypeNames = "OneSText")]
  [Name("OneSTextFormat")]
  internal sealed class OneSTextFormatDef : ClassificationFormatDefinition
  {
    public OneSTextFormatDef()
    {
      this.DisplayName = "This is a text";
      this.ForegroundColor = new Color?(VSThemeColorHelper.CurrentTheme == VSThemeColorHelper.Theme.Dark ? (Color) ColorConverter.ConvertFromString("#c0c0c0") : Colors.Black);
    }
  }
}
