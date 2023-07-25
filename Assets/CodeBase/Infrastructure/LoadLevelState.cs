﻿using CodeBase.CameraLogic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class LoadLevelState : IPayLoadedState<string>
	{
		private const string InitialpointTag = "InitialPoint";
		private const string HeroPath = "Hero/hero";
		private const string HudPath = "Hud/Hud";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _curtain;
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
			GameObject initialPoint = GameObject.FindWithTag(InitialpointTag);
			GameObject hero = Instantiate(HeroPath, at: initialPoint.transform.position);
			
			Instantiate(HudPath);

			CameraFollow(hero);
			
			_stateMachine.Enter<GameLoopState>();
		}
		
		private void CameraFollow(GameObject hero) =>
			Camera.main
				.GetComponent<CameraFollow>()
				.Follow(hero);
		private static GameObject Instantiate(string path)
		{
			var prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab);
		}
		
		private static GameObject Instantiate(string path, Vector3 at)
		{
			var prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab, at, Quaternion.identity);
		}
	}
}