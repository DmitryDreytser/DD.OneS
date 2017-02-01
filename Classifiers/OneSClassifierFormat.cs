// Decompiled with JetBrains decompiler
// Type: DD.OneS.Classifiers.OneSClassifierFormat
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;

namespace DD.OneS.Classifiers
{
  [ClassificationType(ClassificationTypeNames = "OneSClassifier")]
  [Export(typeof (EditorFormatDefinition))]
  [Order(Before = "Default Priority")]
  [UserVisible(true)]
  [Name("OneSClassifier")]
  internal sealed class OneSClassifierFormat : ClassificationFormatDefinition
  {
    public OneSClassifierFormat()
    {
      this.DisplayName = "OneSClassifier";
      this.BackgroundColor = new Color?(Colors.BlueViolet);
      this.TextDecorations = TextDecorations.Underline;
    }
  }
}
