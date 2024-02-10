using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory {
    public class UIFactory : IUIFactory {
        private readonly IAssets _assetProvider;
        private readonly IStaticDataService _staticData;
        
        private Transform _uiRoot;

        private const string UIRootPath = "UI/UIRoot";

        public UIFactory(IAssets assetProvider, IStaticDataService staticData) {
            _assetProvider = assetProvider;
            _staticData = staticData;
        }

        public void CreateShop() {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            Object.Instantiate(config.Prefab, _uiRoot);
        }

        public void CreateUIRoot() {
             _uiRoot = _assetProvider.Instantiate(UIRootPath).transform;
        }
    }
}