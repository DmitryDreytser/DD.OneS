// Decompiled with JetBrains decompiler
// Type: DD.OneS.Classifiers.OneSClassifierClassificationDefinition
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace DD.OneS.Classifiers
{
  internal static class OneSClassifierClassificationDefinition
  {
    [Name("OneSText")]
    [Export(typeof (ClassificationTypeDefinition))]
    internal static ClassificationTypeDefinition text = (ClassificationTypeDefinition) null;
    [Name("OneSDate")]
    [Export(typeof (ClassificationTypeDefinition))]
    internal static ClassificationTypeDefinition date = (ClassificationTypeDefinition) null;
    [Name("OneSComment")]
    [Export(typeof (ClassificationTypeDefinition))]
    internal static ClassificationTypeDefinition comment = (ClassificationTypeDefinition) null;
    [Name("OneSNumber")]
    [Export(typeof (ClassificationTypeDefinition))]
    internal static ClassificationTypeDefinition number = (ClassificationTypeDefinition) null;
    [Export(typeof (ClassificationTypeDefinition))]
    [Name("OneSKeyword")]
    internal static ClassificationTypeDefinition keyword = (ClassificationTypeDefinition) null;
    [Export(typeof (ClassificationTypeDefinition))]
    [Name("OneSError")]
    internal static ClassificationTypeDefinition code = (ClassificationTypeDefinition) null;
    [Name("OneSError")]
    [Export(typeof (ClassificationTypeDefinition))]
    internal static ClassificationTypeDefinition error = (ClassificationTypeDefinition) null;
    [Export(typeof (ClassificationTypeDefinition))]
    [Name("OneSClassifier")]
    private static ClassificationTypeDefinition typeDefinition;
  }
}
