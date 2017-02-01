// Decompiled with JetBrains decompiler
// Type: DD.OneS.Commands.EditorCommandFilter
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace DD.OneS.Commands
{
  internal class EditorCommandFilter : IOleCommandTarget
  {
    private IWpfTextView m_textView;
    private IOleCommandTarget m_nextTarget;
    [Import(typeof (ITextStructureNavigator))]
    private ITextStructureNavigator TextStructureNavigator;
    [Import(typeof (ITextStructureNavigatorSelectorService))]
    internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector;
    [Import(typeof (IClassifierAggregatorService))]
    internal IClassifierAggregatorService aggregator;
    private CommandFilterProvider m_provider;

    public EditorCommandFilter(IWpfTextView textView, ITextStructureNavigatorSelectorService TextStructureNavigatorSelector, CommandFilterProvider provider)
    {
      this.m_textView = textView;
      this.TextStructureNavigatorSelector = TextStructureNavigatorSelector;
      this.m_provider = provider;
    }

    internal void SetNextTarget(IOleCommandTarget nextTarget)
    {
      this.m_nextTarget = nextTarget;
    }

    public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
    {
      if (pguidCmdGroup == VSConstants.CMDSETID.StandardCommandSet97_guid && (int) nCmdID == 935)
      {
        string currentPredicte = this.GetCurrentPredicte();
        if (string.IsNullOrWhiteSpace(currentPredicte))
          return 0;
        DTE globalService = Package.GetGlobalService(typeof (SDTE)) as DTE;
        if (globalService == null || globalService.ActiveDocument == null || (globalService.ActiveDocument.Object("") == null || !(globalService.ActiveDocument.Object("") is TextDocument)))
          return 0;
        string str = globalService.ActiveDocument[].Substring(globalService.ActiveDocument[].LastIndexOf(".") + 1);
        globalService.Find.Action = vsFindAction.vsFindActionFindAll;
        globalService.Find.Backwards = true;
        globalService.Find.FilesOfType = string.Format("*.{0}", (object) str);
        globalService.Find.FindWhat = "(Процедура|Функция|Перем)[\\s|\\t]*" + currentPredicte;
        globalService.Find.KeepModifiedDocumentsOpen = true;
        globalService.Find.MatchCase = false;
        globalService.Find.MatchInHiddenText = true;
        globalService.Find.MatchWholeWord = false;
        globalService.Find.PatternSyntax = vsFindPatternSyntax.vsFindPatternSyntaxRegExpr;
        globalService.Find.ResultsLocation = vsFindResultsLocation.vsFindResults1;
        globalService.Find.SearchSubfolders = true;
        globalService.Find.Target = vsFindTarget.vsFindTargetCurrentDocument;
        int num = (int) globalService.Find.Execute();
        return 0;
      }
      if (pguidCmdGroup == VSConstants.CMDSETID.StandardCommandSet2K_guid && (int) nCmdID == 3)
      {
        string text = this.m_textView.Caret.Position.BufferPosition.GetContainingLine().GetText();
        bool flag1 = false;
        bool flag2 = false;
        int length = text.Length - text.Replace(" ", "\t").TrimStart('\t').Length;
        string str = text.Substring(0, length);
        this.TextStructureNavigator = this.TextStructureNavigatorSelector.GetTextStructureNavigator(this.m_textView.TextBuffer);
        foreach (ClassificationSpan classificationSpan in (IEnumerable<ClassificationSpan>) this.m_provider.Classifiers.GetClassifier(this.m_textView.Caret.Position.Point.AnchorBuffer).GetClassificationSpans(this.TextStructureNavigator.GetExtentOfWord(this.m_textView.Caret.Position.BufferPosition - 1).Span))
        {
          if (classificationSpan.ClassificationType.IsOfType("OneSComment"))
            flag2 = true;
          if (classificationSpan.ClassificationType.IsOfType("OneSText"))
            flag1 = true;
        }
        int num = this.m_nextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        if (flag1)
        {
          int position1 = this.m_textView.Caret.Position.BufferPosition.Position;
          int position2 = this.m_textView.Caret.Position.BufferPosition.GetContainingLine().Start.Position;
          this.m_textView.TextBuffer.Insert(this.m_textView.Caret.Position.BufferPosition.Position, str + "|");
          this.m_textView.Caret.MoveTo(this.m_textView.Caret.Position.BufferPosition + 1);
        }
        if (flag2)
        {
          this.m_textView.TextBuffer.Insert(this.m_textView.Caret.Position.BufferPosition.Position, str + "//");
          this.m_textView.Caret.MoveTo(this.m_textView.Caret.Position.BufferPosition + 1);
        }
        return num;
      }
      if (this.m_nextTarget != null)
        return this.m_nextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
      return 16;
    }

    public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
    {
      if (pguidCmdGroup == VSConstants.CMDSETID.StandardCommandSet97_guid && (int) prgCmds[0].cmdID == 935)
      {
        prgCmds[0].cmdf = !string.IsNullOrEmpty(this.GetCurrentPredicte()) ? 3U : 17U;
        return 0;
      }
      if (this.m_nextTarget != null)
        return this.m_nextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
      return 16;
    }

    private string GetCurrentPredicte()
    {
      this.TextStructureNavigator = this.TextStructureNavigatorSelector.GetTextStructureNavigator(this.m_textView.TextBuffer);
      TextExtent extentOfWord = this.TextStructureNavigator.GetExtentOfWord(this.m_textView.Caret.Position.BufferPosition);
      string text1 = this.m_textView.Caret.Position.BufferPosition.GetContainingLine().GetText();
      string text2 = extentOfWord.Span.GetText();
      if (string.IsNullOrWhiteSpace(text1))
        return (string) null;
      if (text1.StartsWith("//"))
        return (string) null;
      return text2;
    }
  }
}
