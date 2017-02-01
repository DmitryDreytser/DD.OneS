// Decompiled with JetBrains decompiler
// Type: DD.OneS.Helpers.BufferIdleEventUtil
// Assembly: DD.OneS, Version=1.0.0.107, Culture=neutral, PublicKeyToken=null
// MVID: 7D35E576-412D-4EAD-87A5-CAAF17A76DA3
// Assembly location: C:\Temp\Wyvujal\93054f28a8\DD.OneS.dll

using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace DD.OneS.Helpers
{
  internal static class BufferIdleEventUtil
  {
    private static object bufferListenersKey = new object();
    private static object bufferTimerKey = new object();

    public static bool AddBufferIdleEventListener(ITextBuffer buffer, EventHandler handler)
    {
      HashSet<EventHandler> listeners;
      if (!BufferIdleEventUtil.TryGetBufferListeners(buffer, out listeners))
        listeners = BufferIdleEventUtil.ConnectToBuffer(buffer);
      if (listeners.Contains(handler))
        return false;
      listeners.Add(handler);
      return true;
    }

    public static bool RemoveBufferIdleEventListener(ITextBuffer buffer, EventHandler handler)
    {
      HashSet<EventHandler> listeners;
      if (!BufferIdleEventUtil.TryGetBufferListeners(buffer, out listeners) || !listeners.Contains(handler))
        return false;
      listeners.Remove(handler);
      if (listeners.Count == 0)
        BufferIdleEventUtil.DisconnectFromBuffer(buffer);
      return true;
    }

    private static bool TryGetBufferListeners(ITextBuffer buffer, out HashSet<EventHandler> listeners)
    {
      return buffer.Properties.TryGetProperty<HashSet<EventHandler>>(BufferIdleEventUtil.bufferListenersKey, out listeners);
    }

    private static void ClearBufferListeners(ITextBuffer buffer)
    {
      buffer.Properties.RemoveProperty(BufferIdleEventUtil.bufferListenersKey);
    }

    private static bool TryGetBufferTimer(ITextBuffer buffer, out DispatcherTimer timer)
    {
      return buffer.Properties.TryGetProperty<DispatcherTimer>(BufferIdleEventUtil.bufferTimerKey, out timer);
    }

    private static void ClearBufferTimer(ITextBuffer buffer)
    {
      DispatcherTimer timer;
      if (!BufferIdleEventUtil.TryGetBufferTimer(buffer, out timer))
        return;
      if (timer != null)
        timer.Stop();
      buffer.Properties.RemoveProperty(BufferIdleEventUtil.bufferTimerKey);
    }

    private static void DisconnectFromBuffer(ITextBuffer buffer)
    {
      buffer.Changed -= new EventHandler<TextContentChangedEventArgs>(BufferIdleEventUtil.BufferChanged);
      BufferIdleEventUtil.ClearBufferListeners(buffer);
      BufferIdleEventUtil.ClearBufferTimer(buffer);
      buffer.Properties.RemoveProperty(BufferIdleEventUtil.bufferListenersKey);
    }

    private static HashSet<EventHandler> ConnectToBuffer(ITextBuffer buffer)
    {
      buffer.Changed += new EventHandler<TextContentChangedEventArgs>(BufferIdleEventUtil.BufferChanged);
      BufferIdleEventUtil.RestartTimerForBuffer(buffer);
      HashSet<EventHandler> eventHandlerSet = new HashSet<EventHandler>();
      buffer.Properties[BufferIdleEventUtil.bufferListenersKey] = (object) eventHandlerSet;
      return eventHandlerSet;
    }

    private static void RestartTimerForBuffer(ITextBuffer buffer)
    {
      DispatcherTimer timer;
      if (BufferIdleEventUtil.TryGetBufferTimer(buffer, out timer))
      {
        timer.Stop();
      }
      else
      {
        timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle)
        {
          Interval = TimeSpan.FromMilliseconds(500.0)
        };
        timer.Tick += (EventHandler) ((s, e) =>
        {
          BufferIdleEventUtil.ClearBufferTimer(buffer);
          HashSet<EventHandler> listeners;
          if (!BufferIdleEventUtil.TryGetBufferListeners(buffer, out listeners))
            return;
          foreach (EventHandler eventHandler in listeners)
            eventHandler((object) buffer, new EventArgs());
        });
        buffer.Properties[BufferIdleEventUtil.bufferTimerKey] = (object) timer;
      }
      timer.Start();
    }

    private static void BufferChanged(object sender, TextContentChangedEventArgs e)
    {
      ITextBuffer buffer = sender as ITextBuffer;
      if (buffer == null)
        return;
      BufferIdleEventUtil.RestartTimerForBuffer(buffer);
    }
  }
}
