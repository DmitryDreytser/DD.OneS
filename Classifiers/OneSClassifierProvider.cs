// Decompiled with JetBrains decompiler
// Type: DD.OneS.Classifiers.OneSClassifierProvider
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;

namespace DD.OneS.Classifiers
{
  [TagType(typeof (IOutliningRegionTag))]
  [ContentType("OneS")]
  [Export(typeof (IClassifierProvider))]
  [Export(typeof (ITaggerProvider))]
  internal class OneSClassifierProvider : IClassifierProvider, ITaggerProvider
  {
    [Export]
    [BaseDefinition("code")]
    [Name("OneS")]
    internal static ContentTypeDefinition OneSContentType;
    [FileExtension(".1s")]
    [Export]
    [ContentType("OneS")]
    internal static FileExtensionToContentTypeDefinition OneS1sFileType;
    [ContentType("OneS")]
    [Export]
    [FileExtension(".frm")]
    internal static FileExtensionToContentTypeDefinition PrologFrmFileType;
    [Import]
    private IClassificationTypeRegistryService classificationRegistry;

    public OneSClassifier Get(IClassificationTypeRegistryService classificationRegistry, ITextBuffer buffer)
    {
      return buffer.Properties.GetOrCreateSingletonProperty<OneSClassifier>((Func<OneSClassifier>) (() => new OneSClassifier(this.classificationRegistry, buffer)));
    }

    public IClassifier GetClassifier(ITextBuffer buffer)
    {
      return (IClassifier) this.Get(this.classificationRegistry, buffer);
    }

    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
    {
      return this.Get(this.classificationRegistry, buffer) as ITagger<T>;
    }
  }
}
