using CodeBase.CameraLogic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class LoadLevelState : IPayLoadedState<string>
	{
		private const string InitialpointTag = "InitialPoint";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _curtain;
		private readonly IGameFactory _gameFactory;
		public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
			_curtain = curtain;
		}
		public void Enter(string sceneName)
		{
			_curtain.Show();
			_sceneLoader.Load(sceneName, OnLoaded);
		}
		public void Exit()
		{
			_curtain.Hide();
		}
		private void OnLoaded()
		{
			GameObject hero = _gameFactory.CreateHero(at: GameObject.FindWithTag(InitialpointTag));

			_gameFactory.CreateHUD();

			CameraFollow(hero);
			
			_stateMachine.Enter<GameLoopState>();
		}

		private void CameraFollow(GameObject hero) =>
			Camera.main
				.GetComponent<CameraFollow>()
				.Follow(hero);

	}
}