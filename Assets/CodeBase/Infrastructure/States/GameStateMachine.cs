﻿using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;

namespace CodeBase.Infrastructure.States
{
	public class GameStateMachine
	{
		private readonly Dictionary<Type, IExitableState> _states;
		private IExitableState _activeState;
		public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
		{
			_states = new Dictionary<Type, IExitableState>
			{
				[typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
				[typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain, services.Single<IGameFactory>()),
				[typeof(LoadProgressState)] = new LoadProgressState(this, 
					services.Single<IPersistentProgressService>(), 
					services.Single<ISaveLoadService>()),
				[typeof(GameLoopState)] = new GameLoopState(this),
			};
		}
		public void Enter<TState>() where TState : class, IState
		{
			ChangeState<TState>().Enter();
		}
		public void Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayLoadedState<TPayLoad>
		{
			ChangeState<TState>().Enter(payLoad);
		}
		
		private TState ChangeState<TState>() where TState : class, IExitableState
		{
			_activeState?.Exit();
			
			TState state = GetState<TState>();
			_activeState = state;
			
			return state;
		}
		
		private TState GetState<TState>() where TState : class, IExitableState =>
			_states[typeof(TState)] as TState;

	}
}