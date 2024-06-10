using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Game.Core
{
  [UsedImplicitly]
  public class SceneService
  {
    public void Open(string sceneName, LifetimeScope scope) => SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

    public async UniTask OpenAsync(string sceneName, IProgress<float>? progress = null, float maxProgress = 0.1f)
    {
      var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

      while(!operation.isDone)
      {
        progress?.Report(operation.progress * maxProgress);
        await UniTask.Yield();
      }
    }
  }
}