using System.Collections.Generic;

namespace Motk.Shared
{
  public static class CommandLineHelper
  {
    private const string ArgumentMarker = "-";
    
    public static Dictionary<string, string> GetCommandlineArgs()
    {
      var argDictionary = new Dictionary<string, string>();

      var args = System.Environment.GetCommandLineArgs();

      for (var i = 0; i < args.Length - 1; ++i)
      {
        var arg = args[i].ToLower();
        if (!arg.StartsWith(ArgumentMarker))
          continue;

        var value = args[i + 1].ToLower();
        argDictionary.Add(arg, value);
      }
      return argDictionary;
    }
  }
}