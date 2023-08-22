﻿using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
	public class BootstrapState : IState
	{
		private const string Initial = "Initial";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly AllServices _services;

		public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
		{
			_sceneLoader = sceneLoader;
			_stateMachine = stateMachine;
			_services = services;
			
			RegisterServices();
		}
		public void Enter()
		{
			_sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
		}
		private void EnterLoadLevel() =>
			_stateMachine.Enter<LoadLevelState, string>("Main");
		private void RegisterServices()
		{
			_services.RegisterSingle<IInputService>(InputService());
			_services.RegisterSingle<IAssets>(new AssetProvider());
			_services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>()));
		}
		public void Exit()
		{

		}
		private static IInputService InputService()
		{
			if (Application.isEditor)
				return new StandaloneInputService();
			else
				return new MobileInputService();
		}

	}
}