using System.Text.Json;
using System.Text.Json.Serialization;
using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Descriptors;
using Cysharp.Threading.Tasks;
using MessagePipe;
using Motk.Campaign.Client;
using Motk.Campaign.Client.Actors.Descriptors;
using Motk.Campaign.Client.Player;
using Motk.Client.Core;
using Motk.Client.Core.Descriptors;
using Motk.Client.Core.Descriptors.Serialization;
using Motk.Combat.Client.AppStates;
using Motk.Combat.Client.Render.Units.Descriptors;
using Motk.Combat.Shared.Descriptors;
using Motk.Descriptors;
using Motk.Descriptors.FileSystem;
using Motk.Descriptors.Serialization;
using Motk.HexGrid.Core;
using Motk.Matchmaking.Client;
using Motk.Shared.Configuration;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.Client.Bootstrap
{
  public class ApplicationEntryPoint : MonoBehaviour
  {
    [SerializeField]
    private LocationsRegistry _locationsRegistry = null!;
    [SerializeField]
    private CharactersRegistry _charactersRegistry = null!;
    
    private void Awake()
    {
      Debug.Log("Bootstrap started...");
      var appScope = LifetimeScope.Create(ConfigureAppScope);
      appScope.name = "[Application]";
      DontDestroyOnLoad(appScope);
      
      var stateMachine = appScope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<BootstrapAppState>().Forget();
    }
    
    private void ConfigureAppScope(IContainerBuilder builder)
    {
      builder.RegisterInstance(_locationsRegistry);
      builder.RegisterInstance(_charactersRegistry);
      builder.Register<UnitVisualRegistry>(Lifetime.Singleton);
      builder.Register<GameCameraConfigRegistry>(Lifetime.Singleton);
      builder.RegisterDescriptorRegistry<CombatArenaRegistry>();
      builder.Register<MatchmakingClient>(Lifetime.Singleton);
      builder.Register<CurrentPlayerState>(Lifetime.Singleton);
      builder.Register<IConfig, UnityRemoteConfig>(Lifetime.Singleton);
      
      builder.RegisterMessagePipe();
      builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));

      builder.RegisterInstance(new JsonDescriptorSerializer(new JsonSerializerOptions
      {
        IncludeFields = true,
        WriteIndented = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        Converters = { new JsonStringEnumConverter<HexDirection>() }
      })).As<IDescriptorSerializer>();
      builder.Register<DescriptorsBootstrapper>(Lifetime.Singleton);
      builder.Register<IDescriptorLoader, DescriptorLoaderFromFileSystem>(Lifetime.Singleton);
#if UNITY_EDITOR
      builder.Register<IDescriptorsRootDirectoryProvider, EditorDescriptorsRootDirectoryProvider>(Lifetime.Singleton);
#else
      builder.Register<IDescriptorsRootDirectoryProvider, StandaloneDescriptorRootDirectoryProvider>(Lifetime.Singleton);
#endif
      builder.Register<IFileSystemOperations, FileSystemOperations>(Lifetime.Singleton);
      
      RegisterAppStateMachine(builder);
    }

    private void RegisterAppStateMachine(IContainerBuilder builder)
    {
      builder.Register<ApplicationStateMachine>(Lifetime.Singleton);
      builder.Register<ApplicationStateFactory>(Lifetime.Singleton).As<IStateFactory>();
      
      builder.Register<BootstrapAppState>(Lifetime.Transient);
      builder.Register<CampaignAppState>(Lifetime.Transient);
      builder.Register<CombatAppState>(Lifetime.Transient);
    }
  }
}