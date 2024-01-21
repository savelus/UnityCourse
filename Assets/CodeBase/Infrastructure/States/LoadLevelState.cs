using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
	public class LoadLevelState : IPayLoadedState<string>
	{
		private const string InitialpointTag = "InitialPoint";
		private const string EnemySpawnerTag = "EnemySpawner";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _curtain;
		private readonly IGameFactory _gameFactory;
		private readonly IPersistentProgressService _progressService;
		private readonly IStaticDataService _staticData;

		public LoadLevelState(GameStateMachine stateMachine, 
			SceneLoader sceneLoader, 
			LoadingCurtain curtain,
			IGameFactory gameFactory,
			IPersistentProgressService progressService,
			IStaticDataService staticData)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
			_curtain = curtain;
			_gameFactory = gameFactory;
			_progressService = progressService;
			_staticData = staticData;
		}
		public void Enter(string sceneName)
		{
			_curtain.Show();
			_gameFactory.Cleanup();
			_sceneLoader.Load(sceneName, OnLoaded);
		}
		public void Exit()
		{
			_curtain.Hide();
		}
		private void OnLoaded()
		{
			InitGameWorld();
			InformProgressReaders();
			_stateMachine.Enter<GameLoopState>();
		}

		private void InformProgressReaders()
		{
			foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
				progressReader.LoadProgress(_progressService.Progress);
		}

		private void InitGameWorld() {
			InitSpawners();
			
			GameObject hero = InitHero();

			InitHud(hero);
			CameraFollow(hero);
		}

		private void InitSpawners() {
			string sceneKey = SceneManager.GetActiveScene().name;
			LevelStaticData levelStaticData = _staticData.ForLevel(sceneKey);
			
			foreach (var spawnerData in levelStaticData.EnemySpawners) {
				_gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
			}
		}

		private void InitHud(GameObject hero) {
			GameObject hud = _gameFactory.CreateHUD();
			hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
		}
		private GameObject InitHero() =>
			_gameFactory.CreateHero(at: GameObject.FindWithTag(InitialpointTag));

		private void CameraFollow(GameObject hero) =>
			Camera.main
				.GetComponent<CameraFollow>()
				.Follow(hero);

	}
}