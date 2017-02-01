// Decompiled with JetBrains decompiler
// Type: DD.OneS.Helpers.OneSLanguageService
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace DD.OneS.Helpers
{
  public class OneSLanguageService : LanguageService
  {
    private LanguagePreferences m_preferences;
    private OneSLanguageService.TestScanner m_scanner;

    public override string Name
    {
      get
      {
        return "1C Language";
      }
    }

    public override LanguagePreferences GetLanguagePreferences()
    {
      if (this.m_preferences == null)
      {
        this.m_preferences = new LanguagePreferences(this.Site, typeof (OneSLanguageService).GUID, this.Name);
        this.m_preferences.Init();
      }
      return this.m_preferences;
    }

    public override string GetFormatFilterList()
    {
      return "1S(*.1s)|*.1s";
    }

    public override AuthoringScope ParseSource(ParseRequest req)
    {
      return (AuthoringScope) new OneSLanguageService.TestAuthoringScope();
    }

    public override IScanner GetScanner(IVsTextLines buffer)
    {
      if (this.m_scanner == null)
        this.m_scanner = new OneSLanguageService.TestScanner((IVsTextBuffer) buffer);
      return (IScanner) this.m_scanner;
    }

    public override TypeAndMemberDropdownBars CreateDropDownHelper(IVsTextView forView)
    {
      return base.CreateDropDownHelper(forView);
    }

    public class TestAuthoringScope : AuthoringScope
    {
      public override string Goto(VSConstants.VSStd97CmdID cmd, IVsTextView textView, int line, int col, out TextSpan span)
      {
        span = new TextSpan();
        return (string) null;
      }

      public override string GetDataTipText(int line, int col, out TextSpan span)
      {
        span = new TextSpan();
        return (string) null;
      }

      public override Declarations GetDeclarations(IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
      {
        return (Declarations) null;
      }

      public override Methods GetMethods(int line, int col, string name)
      {
        return (Methods) null;
      }
    }

    internal class TestScanner : IScanner
    {
      private IVsTextBuffer m_buffer;
      private string m_source;

      public TestScanner(IVsTextBuffer buffer)
      {
        this.m_buffer = buffer;
      }

      bool IScanner.ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
      {
        tokenInfo.Type = TokenType.Unknown;
        tokenInfo.Color = TokenColor.Text;
        return true;
      }

      void IScanner.SetSource(string source, int offset)
      {
        this.m_source = source.Substring(offset);
      }
    }
  }
}
