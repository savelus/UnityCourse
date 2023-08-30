﻿using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{

	public class GameFactory : IGameFactory
	{
		private readonly IAssets _assets;

		public List<ISavedProgressReader> ProgressReaders { get; } = new();
		public List<ISavedProgress> ProgressWriters { get; } = new();
		
		public GameObject HeroGameObject { get; set; }
		public event Action HeroCreated;

		public GameFactory(IAssets assets)
		{
			_assets = assets;
	}
		public GameObject CreateHero(GameObject at)
		{
			HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
			HeroCreated?.Invoke();
			return HeroGameObject;
		}

		public void CreateHUD() => InstantiateRegistered(AssetPath.HudPath);

		public void Clenup()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}

		private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
		{
			var gameObject = _assets.Instantiate(prefabPath, at: at);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}
		
		private GameObject InstantiateRegistered(string prefabPath)
		{
			var gameObject = _assets.Instantiate(prefabPath);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private void Register(ISavedProgressReader progressReader)
		{
			if(progressReader is ISavedProgress progressWriter)
				ProgressWriters.Add(progressWriter);
			ProgressReaders.Add(progressReader);
		}

		private void RegisterProgressWatchers(GameObject gameObject)
		{
			foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
		}
	}

}