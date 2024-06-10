using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.CampaignServer
{
  public class EntryPoint : MonoBehaviour
  {
    private LifetimeScope _scope = null!;
    
    private void Awake()
    {
      _scope = LifetimeScope.Create(ConfigureScope);
      _scope.name = "Application";

      _scope.Container.Resolve<NetworkManager>().OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
    }

    private void Start()
    {
      _scope.Container.Resolve<NetworkManager>().StartServer();
    }

    private void SingletonOnOnClientConnectedCallback(ulong clientId) => Debug.Log($"Client connected. {clientId}");

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.RegisterInstance(FindObjectOfType<NetworkManager>());
    }
  }
}