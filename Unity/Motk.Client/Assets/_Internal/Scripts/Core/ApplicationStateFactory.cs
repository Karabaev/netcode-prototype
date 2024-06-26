﻿using com.karabaev.applicationLifeCycle.StateMachine;
using VContainer;

namespace Motk.Client.Core
{
  public class ApplicationStateFactory : IStateFactory
  {
    private readonly IObjectResolver _objectResolver;

    public ApplicationStateFactory(IObjectResolver objectResolver) => _objectResolver = objectResolver;

    public TState Create<TState>() => _objectResolver.Resolve<TState>();
  }
}