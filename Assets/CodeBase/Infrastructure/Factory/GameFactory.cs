﻿using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.RandomService;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{

	public class GameFactory : IGameFactory
	{
		private readonly IAssets _assets;
		private readonly IRandomService _randomService;
		private readonly IStaticDataService _staticData;
		private readonly IPersistentProgressService _progressService;

		public List<ISavedProgressReader> ProgressReaders { get; } = new();
		public List<ISavedProgress> ProgressWriters { get; } = new();
		
		private GameObject HeroGameObject { get; set; }

		public GameFactory(IAssets assets, 
						   IStaticDataService staticData, 
						   IRandomService randomService,
						   IPersistentProgressService progressService) {
			_assets = assets;
			_staticData = staticData;
			_randomService = randomService;
			_progressService = progressService;
		}
		public GameObject CreateHero(GameObject at)
		{
			HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
			return HeroGameObject;
		}

		public GameObject CreateHUD()
		{
			return InstantiateRegistered(AssetPath.HudPath);
		}

		public GameObject CreateMonster(MonsterTypeId typeId, Transform parent) {
			MonsterStaticData monsterData = _staticData.ForMonster(typeId);
			GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

			IHealth health = monster.GetComponent<IHealth>();
			health.Current = monsterData.Hp;
			health.Max = monsterData.Hp;

			monster.GetComponent<ActorUI>().Construct(health);
			monster.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
			monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;
			
			var attack = monster.GetComponent<Attack>();
			attack.Construct(HeroGameObject.transform);
			attack.Damage = monsterData.Damage;
			attack.Cleavage = monsterData.Cleavage;
			attack.EffectiveDistance = monsterData.EffectiveDistance;
			
			monster.GetComponent<AgentRotateToPlayer>()?.Construct(HeroGameObject.transform);
			var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
			lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
			lootSpawner.Construct(this, _randomService);

			return monster;
		}

		public LootPiece CreateLoot() {
			var lootPiece = InstantiateRegistered(AssetPath.Loot).GetComponent<LootPiece>();
			lootPiece.Construct(_progressService.Progress.WorldData);
			
			return lootPiece;
		}

		public void Cleanup()
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

		public void Register(ISavedProgressReader progressReader)
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