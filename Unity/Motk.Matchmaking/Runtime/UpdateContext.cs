using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Motk.Matchmaking
{
  public class UpdateContext<T> : IDisposable where T : new()
  {
    private readonly string _key;
    
    public T Value { get; }

    public UpdateContext(string key)
    {
      _key = key;
      Value = Get(_key);
    }

    public void Dispose() => Set(_key, Value!);

    private static T Get(string key)
    {
      var str = PlayerPrefs.GetString(key);
      if (string.IsNullOrEmpty(str))
      {
        return new T();
      }

      return JsonConvert.DeserializeObject<T>(str)!;
    }
    
    private static void Set(string key, object obj)
    {
      var str = JsonConvert.SerializeObject(obj);
      PlayerPrefs.SetString(key, str);
    }
  }
}