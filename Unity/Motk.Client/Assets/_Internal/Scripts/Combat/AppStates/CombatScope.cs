using System;
using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Views;
using MessagePipe;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Core;
using Motk.Client.Core.InputSystem;
using Motk.Combat.Client.Core;
using Motk.Combat.Client.Core.InputSystem;
using Motk.Combat.Client.Core.Network;
using Motk.Combat.Client.Core.Units.Controllers;
using Motk.Combat.Client.Core.Units.Services;
using Motk.Combat.Client.gRPC;
using Motk.Combat.Client.Render.Units;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;
using Motk.PathFinding.AStar;
using VContainer;
using VContainer.Unity;

namespace Motk.Combat.Client.AppStates
{
  public class CombatScope : LifetimeScope
  {
    protected override void Configure(IContainerBuilder builder)
    {
      builder.Register<CombatState>(Lifetime.Singleton);
      builder.Register<SelfCombatState>(Lifetime.Singleton);
      
      builder.Register<HexGridVisualState>(Lifetime.Singleton);
      builder.Register<HexGrid.Core.HexGrid>(Lifetime.Singleton);
      builder.Register<InputState>(Lifetime.Singleton);
      builder.Register<CombatInputState>(Lifetime.Singleton);
      builder.Register<CombatInputController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
      builder.RegisterInstance(FindObjectOfType<InputController>());

      builder.Register<AStarPathFindingService<HexCoordinates>>(Lifetime.Singleton);
      builder.Register<IMapNodeProvider<HexCoordinates>, HexGridMapNodeProvider>(Lifetime.Singleton);
      builder.Register<IHeuristicCalculator<HexCoordinates>, HexHeuristicCalculator>(Lifetime.Singleton);
      
      builder.RegisterInstance(FindObjectOfType<GameCameraView>());

      builder.Register<ICombatUnitFactory, CombatUnitFactory>(Lifetime.Singleton);
      builder.Register<CombatUnitsController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();

      builder.Register<CombatHubClient>(Lifetime.Singleton)
        .As<ICombatMessageReceiver>()
        .As<ICombatMessageSender>()
        .As<IDisposable>();
      builder.Register<GrpcChannelState>(Lifetime.Singleton);
      
      builder.Register<ApplicationStateMachine>(Lifetime.Singleton);
      builder.Register<IStateFactory, ApplicationStateFactory>(Lifetime.Singleton);
      builder.Register<EnterToCombatAppState>(Lifetime.Transient);
      builder.Register<PrepareCombatAppState>(Lifetime.Transient);
      builder.Register<PlayerTeamMoveCombatAppState>(Lifetime.Transient);
      builder.Register<OtherTeamMoveCombatAppState>(Lifetime.Transient);
      
      builder.RegisterMessagePipe();
    }
  }
}