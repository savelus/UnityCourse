using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		GameObject CreateHero(GameObject at);
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		GameObject CreateHUD();
		void Cleanup();
		void Register(ISavedProgressReader progressReader);
		GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
		LootPiece CreateLoot();
	}
}