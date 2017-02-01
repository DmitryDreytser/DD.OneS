// Decompiled with JetBrains decompiler
// Type: DD.OneS.Commands.CommandFilterProvider
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace DD.OneS.Commands
{
  [ContentType("OneS")]
  [TextViewRole("EDITABLE")]
  [Export(typeof (IVsTextViewCreationListener))]
  internal class CommandFilterProvider : IVsTextViewCreationListener
  {
    [Import(typeof (IVsEditorAdaptersFactoryService))]
    internal IVsEditorAdaptersFactoryService editorFactory;
    [Import]
    internal IContentTypeRegistryService ContentTypeRegistryService;
    [Import(typeof (ITextStructureNavigatorSelectorService))]
    internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector;

    [Import]
    public IClassifierAggregatorService Classifiers { get; set; }

    [Import]
    internal SVsServiceProvider ServiceProvider { get; set; }

    public void VsTextViewCreated(IVsTextView textViewAdapter)
    {
      IWpfTextView wpfTextView = (IWpfTextView) this.editorFactory.GetWpfTextView(textViewAdapter);
      if (wpfTextView == null)
        return;
      this.AddCommandFilter(textViewAdapter, new EditorCommandFilter(wpfTextView, this.TextStructureNavigatorSelector, this));
    }

    private void AddCommandFilter(IVsTextView viewAdapter, EditorCommandFilter commandFilter)
    {
      IOleCommandTarget ppNextCmdTarg;
      if (viewAdapter.AddCommandFilter((IOleCommandTarget) commandFilter, out ppNextCmdTarg) != 0 || ppNextCmdTarg == null)
        return;
      commandFilter.SetNextTarget(ppNextCmdTarg);
    }
  }
}
