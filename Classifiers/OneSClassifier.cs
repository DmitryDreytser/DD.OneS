// Decompiled with JetBrains decompiler
// Type: DD.OneS.Classifiers.OneSClassifier
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using DD.OneS.Helpers;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DD.OneS.Classifiers
{
  internal class OneSClassifier : IClassifier, ITagger<IOutliningRegionTag>
  {
    private string Subs = "((Процедура|Функция|Procedure|Function)\\s+\\w+?\\(.*?\\)\\s*.+?(КонецПроцедуры|КонецФункции|EndProcedure|EndFunction))";
    private string Blocks = "(\\n[^/]\\w*?Если.+?Тогда.+?(КонецЕсли|Иначе|ИначеЕсли);)";
    private string ellipsis = "...";
    protected IClassificationTypeRegistryService m_registry;
    protected ITextBuffer _buffer;
    protected ClassificationTag _outerParenTag;
    protected IList<ITagSpan<ITag>> _resultTags;
    private ITextSnapshot snapshot;
    private List<OneSClassifier.Region> regions;
    private readonly IClassificationType classificationType;

    public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

    internal OneSClassifier(IClassificationTypeRegistryService registry, ITextBuffer buffer)
    {
      this.m_registry = registry;
      this.classificationType = registry.GetClassificationType("OneSClassifier");
      this._buffer = buffer;
      this._outerParenTag = this.MakeTag("OneSError");
      this.snapshot = buffer.CurrentSnapshot;
      this.regions = new List<OneSClassifier.Region>();
      BufferIdleEventUtil.AddBufferIdleEventListener(this._buffer, new EventHandler(this.ReParse));
    }

    public void Dispose()
    {
      BufferIdleEventUtil.RemoveBufferIdleEventListener(this._buffer, new EventHandler(this.ReParse));
    }

    private ClassificationTag MakeTag(string name)
    {
      return new ClassificationTag(this.m_registry.GetClassificationType(name));
    }

    public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
    {
      return OneSCodeHelper.GetTokens(span, this.m_registry);
    }

    public object GetInputSnapshot()
    {
      return (object) null;
    }

    private static bool TryGetLevel(string text, int startIndex, out int level)
    {
      level = -1;
      return text.Length > startIndex + 3 && int.TryParse(text.Substring(startIndex + 1), out level);
    }

    private static SnapshotSpan AsSnapshotSpan(OneSClassifier.Region region, ITextSnapshot snapshot)
    {
      ITextSnapshotLine lineFromLineNumber = snapshot.GetLineFromLineNumber(region.StartLine);
      ITextSnapshotLine textSnapshotLine = region.StartLine == region.EndLine ? lineFromLineNumber : snapshot.GetLineFromLineNumber(region.EndLine);
      return new SnapshotSpan(lineFromLineNumber.Start + region.StartOffset, textSnapshotLine.End);
    }

    private void ReParse(object sender, EventArgs args)
    {
      ITextSnapshot newSnapshot = this._buffer.CurrentSnapshot;
      List<OneSClassifier.Region> source = new List<OneSClassifier.Region>();
      foreach (Match match in new Regex(this.Subs, RegexOptions.IgnoreCase | RegexOptions.Singleline).Matches(newSnapshot.GetText()))
      {
        ITextSnapshotLine lineFromPosition1 = newSnapshot.GetLineFromPosition(match.Index);
        ITextSnapshotLine lineFromPosition2 = newSnapshot.GetLineFromPosition(match.Index + match.Length);
        if (lineFromPosition1.LineNumber != lineFromPosition2.LineNumber)
        {
          if (lineFromPosition1.GetText().Contains("Далее"))
          {
            int num = Math.Max(match.Value.LastIndexOf("Функция"), match.Value.LastIndexOf("Процедура"));
            lineFromPosition1 = newSnapshot.GetLineFromPosition(match.Index + num);
          }
          List<OneSClassifier.Region> regionList = source;
          OneSClassifier.Region region1 = new OneSClassifier.Region();
          region1.Level = 1;
          region1.StartLine = lineFromPosition1.LineNumber;
          region1.StartOffset = lineFromPosition1.Length;
          region1.EndLine = lineFromPosition2.LineNumber;
          region1.HoverText = newSnapshot.GetText(lineFromPosition1.Start.Position, lineFromPosition2.End.Position - lineFromPosition1.Start.Position);
          OneSClassifier.Region region2 = region1;
          regionList.Add(region2);
        }
      }
      List<Span> spanList1 = new List<Span>(this.regions.Select<OneSClassifier.Region, Span>((Func<OneSClassifier.Region, Span>) (r => OneSClassifier.AsSnapshotSpan(r, this.snapshot).TranslateTo(newSnapshot, SpanTrackingMode.EdgeExclusive).Span)));
      List<Span> spanList2 = new List<Span>(source.Select<OneSClassifier.Region, Span>((Func<OneSClassifier.Region, Span>) (r => OneSClassifier.AsSnapshotSpan(r, newSnapshot).Span)));
      NormalizedSpanCollection normalizedSpanCollection = NormalizedSpanCollection.Difference(new NormalizedSpanCollection((IEnumerable<Span>) spanList1), new NormalizedSpanCollection((IEnumerable<Span>) spanList2));
      int num1 = int.MaxValue;
      int num2 = -1;
      if (normalizedSpanCollection.Count > 0)
      {
        num1 = normalizedSpanCollection[0].Start;
        num2 = normalizedSpanCollection[normalizedSpanCollection.Count - 1].End;
      }
      if (spanList2.Count > 0)
      {
        num1 = Math.Min(num1, spanList2[0].Start);
        num2 = Math.Max(num2, spanList2[spanList2.Count - 1].End);
      }
      this.snapshot = newSnapshot;
      this.regions = source;
      if (num1 > num2 || this.TagsChanged == null)
        return;
      this.TagsChanged((object) this, new SnapshotSpanEventArgs(new SnapshotSpan(this.snapshot, Span.FromBounds(num1, num2))));
    }

    public IEnumerable<ITagSpan<IOutliningRegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    {
      if (spans.Count != 0)
      {
        List<OneSClassifier.Region> currentRegions = this.regions;
        ITextSnapshot currentSnapshot = this.snapshot;
        SnapshotSpan entire = new SnapshotSpan(spans[0].Start, spans[spans.Count - 1].End).TranslateTo(currentSnapshot, SpanTrackingMode.EdgeExclusive);
        int startLineNumber = entire.Start.GetContainingLine().LineNumber;
        int endLineNumber = entire.End.GetContainingLine().LineNumber;
        foreach (OneSClassifier.Region region in currentRegions)
        {
          if (region.StartLine <= endLineNumber && region.EndLine >= startLineNumber)
          {
            ITextSnapshotLine startLine = currentSnapshot.GetLineFromLineNumber(region.StartLine);
            ITextSnapshotLine endLine = currentSnapshot.GetLineFromLineNumber(region.EndLine);
            yield return (ITagSpan<IOutliningRegionTag>) new TagSpan<IOutliningRegionTag>(new SnapshotSpan(startLine.Start + region.StartOffset, endLine.End), (IOutliningRegionTag) new OutliningRegionTag(false, false, (object) this.ellipsis, (object) region.HoverText));
          }
        }
      }
    }

    private void BufferChanged(object sender, TextContentChangedEventArgs e)
    {
      if (e.After != this._buffer.CurrentSnapshot)
        return;
      this.ReParse(sender, (EventArgs) e);
    }

    private class PartialRegion
    {
      public int StartLine { get; set; }

      public int StartOffset { get; set; }

      public int Level { get; set; }

      public OneSClassifier.PartialRegion PartialParent { get; set; }

      public string HoverText { get; set; }
    }

    private class Region : OneSClassifier.PartialRegion
    {
      public int EndLine { get; set; }
    }
  }
}
