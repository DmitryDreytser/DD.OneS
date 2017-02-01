// Decompiled with JetBrains decompiler
// Type: DD.OneS.Helpers.OneSLanguagePackage
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace DD.OneS.Helpers
{
  [ProvideLanguageService(typeof (OneSLanguageService), "1C Language Service", 106, CodeSense = true, EnableAsyncCompletion = true, EnableCommenting = true, RequestStockColors = false)]
  [ProvideLanguageCodeExpansion(typeof (OneSLanguageService), "1C Language", 106, "testlanguage", "%InstallRoot%\\Test Language\\SnippetsIndex.xml", SearchPaths = "%InstallRoot%\\Test Language\\Snippets\\%LCID%\\Snippets\\;%TestDocs%\\Code Snippets\\Test Language\\Test Code Snippets")]
  [ProvideService(typeof (OneSLanguageService), ServiceName = "1C Language Service")]
  [ProvideLanguageExtension(typeof (OneSLanguageService), ".1s")]
  [ProvideLanguageService(typeof (OneSLanguageService), "1C Language", 0, AutoOutlining = true, EnableCommenting = true, MatchBraces = true, ShowMatchingBrace = true)]
  public class OneSLanguagePackage : Microsoft.VisualStudio.Shell.Package, IOleComponent
  {
    private uint m_componentID;

    protected override void Initialize()
    {
      base.Initialize();
      IServiceContainer serviceContainer = (IServiceContainer) this;
      OneSLanguageService slanguageService = new OneSLanguageService();
      slanguageService.SetSite((object) this);
      serviceContainer.AddService(typeof (OneSLanguageService), (object) slanguageService, true);
      IOleComponentManager service = this.GetService(typeof (SOleComponentManager)) as IOleComponentManager;
      if ((int) this.m_componentID != 0 || service == null)
        return;
      OLECRINFO[] pcrinfo = new OLECRINFO[1];
      pcrinfo[0].cbSize = (uint) Marshal.SizeOf(typeof (OLECRINFO));
      pcrinfo[0].grfcrf = 3U;
      pcrinfo[0].grfcadvf = 7U;
      pcrinfo[0].uIdleTimeInterval = 1000U;
      service.FRegisterComponent((IOleComponent) this, pcrinfo, out this.m_componentID);
    }

    protected override void Dispose(bool disposing)
    {
      if ((int) this.m_componentID != 0)
      {
        IOleComponentManager service = this.GetService(typeof (SOleComponentManager)) as IOleComponentManager;
        if (service != null)
          service.FRevokeComponent(this.m_componentID);
        this.m_componentID = 0U;
      }
      base.Dispose(disposing);
    }

    public int FDoIdle(uint grfidlef)
    {
      bool periodic = ((int) grfidlef & 1) != 0;
      LanguageService service = this.GetService(typeof (OneSLanguageService)) as LanguageService;
      if (service != null)
        service.OnIdle(periodic);
      return 0;
    }

    public int FContinueMessageLoop(uint uReason, IntPtr pvLoopData, MSG[] pMsgPeeked)
    {
      return 1;
    }

    public int FPreTranslateMessage(MSG[] pMsg)
    {
      return 0;
    }

    public int FQueryTerminate(int fPromptUser)
    {
      return 1;
    }

    public int FReserved1(uint dwReserved, uint message, IntPtr wParam, IntPtr lParam)
    {
      return 1;
    }

    public IntPtr HwndGetWindow(uint dwWhich, uint dwReserved)
    {
      return IntPtr.Zero;
    }

    public void OnActivationChange(IOleComponent pic, int fSameComponent, OLECRINFO[] pcrinfo, int fHostIsActivating, OLECHOSTINFO[] pchostinfo, uint dwReserved)
    {
    }

    public void OnAppActivate(int fActive, uint dwOtherThreadID)
    {
    }

    public void OnEnterState(uint uStateID, int fEnter)
    {
    }

    public void OnLoseActivation()
    {
    }

    public void Terminate()
    {
    }

    internal class OneSVsLanguagePackage : IVsPackage
    {
      public int Close()
      {
        throw new NotImplementedException();
      }

      public int CreateTool(ref Guid rguidPersistenceSlot)
      {
        throw new NotImplementedException();
      }

      public int GetAutomationObject(string pszPropName, out object ppDisp)
      {
        throw new NotImplementedException();
      }

      public int GetPropertyPage(ref Guid rguidPage, VSPROPSHEETPAGE[] ppage)
      {
        throw new NotImplementedException();
      }

      public int QueryClose(out int pfCanClose)
      {
        throw new NotImplementedException();
      }

      public int ResetDefaults(uint grfFlags)
      {
        throw new NotImplementedException();
      }

      public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
      {
        throw new NotImplementedException();
      }
    }
  }
}
