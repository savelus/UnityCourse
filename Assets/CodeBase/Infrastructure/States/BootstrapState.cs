using System.ComponentModel;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.RandomService;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
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
			_stateMachine.Enter<LoadProgressState>();
		private void RegisterServices()
		{
			IStaticDataService staticDataService = RegisterStaticData();
			IRandomService randomService = new UnityRandomService();
			IInputService inputService = InputService();
			IAssets assets = new AssetProvider();
			IPersistentProgressService persistentProgressService = new PersistentProgressService();
			IUIFactory uiFactory = new UIFactory(assets, staticDataService, persistentProgressService);
			IWindowService windowService = new WindowService(uiFactory);
			IGameFactory gameFactory = new GameFactory(assets, 
													  staticDataService, 
													  randomService, 
													  persistentProgressService,
													  windowService);
			ISaveLoadService saveLoadService = new SaveLoadService(persistentProgressService, gameFactory);
			
			_services.RegisterSingle(randomService);
			_services.RegisterSingle(inputService);
			_services.RegisterSingle(assets);
			_services.RegisterSingle(persistentProgressService);
			_services.RegisterSingle(uiFactory);
			_services.RegisterSingle(windowService);
			_services.RegisterSingle(gameFactory);
			_services.RegisterSingle(saveLoadService);
			
		}


		private IStaticDataService RegisterStaticData() {
			IStaticDataService staticData = new StaticDataService();
			staticData.LoadMonsters();

			_services.RegisterSingle(staticData);

			return staticData;
		}

		public void Exit()
		{

		}
		private static IInputService InputService() {
			if (Application.isEditor) return new StandaloneInputService();
			return new MobileInputService();
		}

	}
}