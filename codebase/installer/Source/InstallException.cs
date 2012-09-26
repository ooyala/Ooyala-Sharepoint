
using System;
using System.Collections.Generic;
using System.Text;

namespace SharePointInstaller
{
  public class InstallException : ApplicationException
  {
    public InstallException(string message)
      : base(message)
    {
    }

    public InstallException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
