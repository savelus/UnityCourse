using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		GameObject CreateHero(GameObject at);
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		event Action HeroCreated;
		GameObject HeroGameObject { get; }
		GameObject CreateHUD();
		void Cleanup();
		void Register(ISavedProgressReader progressReader);
	}
}