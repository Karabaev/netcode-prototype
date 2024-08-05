using System;
using System.IO;
using JetBrains.Annotations;
using Motk.Descriptors.FileSystem;

namespace Motk.Client.Core.Descriptors
{
  [UsedImplicitly]
  public class EditorDescriptorsRootDirectoryProvider : IDescriptorsRootDirectoryProvider
  {
    public string GetRootDirectory()
    {
      return Path.Combine(Environment.CurrentDirectory, "..", "..", "Descriptors");
    }
  }
}