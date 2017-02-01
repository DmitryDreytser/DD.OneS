// Decompiled with JetBrains decompiler
// Type: DD.OneS.Classifiers.OneSCommentFormat
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
  [Name("OneSCommentFormat")]
  [UserVisible(false)]
  [Order(Before = "Default Priority")]
  [ClassificationType(ClassificationTypeNames = "OneSComment")]
  [Export(typeof (EditorFormatDefinition))]
  internal sealed class OneSCommentFormat : ClassificationFormatDefinition
  {
    public OneSCommentFormat()
    {
      this.DisplayName = "This is a comment";
      this.ForegroundColor = new Color?(VSThemeColorHelper.CurrentTheme == VSThemeColorHelper.Theme.Dark ? (Color) ColorConverter.ConvertFromString("#139734") : Colors.Green);
    }
  }
}
