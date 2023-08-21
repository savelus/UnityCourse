using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{

	public class GameFactory : IGameFactory, IService
	{
		private readonly IAssets _assets;
		public GameFactory(IAssets assets)
		{
			_assets = assets;
		}
		public GameObject CreateHero(GameObject at) => 
			_assets.Instantiate(AssetPath.HeroPath, at: at.transform.position);

		public void CreateHUD() => 
			_assets.Instantiate(AssetPath.HudPath);

	}

}