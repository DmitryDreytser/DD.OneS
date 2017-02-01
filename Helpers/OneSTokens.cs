// Decompiled with JetBrains decompiler
// Type: DD.OneS.Helpers.OneSTokens
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

namespace DD.OneS.Helpers
{
  internal class OneSTokens
  {
    public enum OneSTokenTypes
    {
      OneSComment,
      OneSKeyword,
      OneSText,
      OneSCode,
      OneSDate,
      OneSNumber,
      OneSError,
    }

    public sealed class OneSTokenHelper
    {
      public const string OneSComment = "OneSComment";
      public const string OneSKeyword = "OneSKeyword";
      public const string OneSText = "OneSText";
      public const string OneSCode = "OneSCode";
      public const string OneSDate = "OneSDate";
      public const string OneSNumber = "OneSNumber";
      public const string OneSError = "OneSError";
    }
  }
}
