using System;
using System.Collections.Generic;
using System.Text;

namespace SharePointInstaller
{
  public static class LogManager
  {
    private static readonly ILog defaultLogger = new FileLogger();

    public static ILog GetLogger()
    {
      return defaultLogger;
    }

    private class FileLogger : ILog
    {
      public void Info(object message)
      {
      }

      public void Info(object message, Exception t)
      {
      }

      public void Warn(object message)
      {
      }

      public void Warn(object message, Exception t)
      {
      }

      public void Error(object message)
      {
      }

      public void Error(object message, Exception t)
      {
      }

      public void Fatal(object message)
      {
      }

      public void Fatal(object message, Exception t)
      {
      }
    }
  }
}
