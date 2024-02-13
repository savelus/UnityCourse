using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory {
    public class UIFactory : IUIFactory {
        private readonly IAssets _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;

        private Transform _uiRoot;

        private const string UIRootPath = "UI/UIRoot";

        public UIFactory(IAssets assetProvider, IStaticDataService staticData, IPersistentProgressService progressService) {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _progressService = progressService;
        }

        public void CreateShop() {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            window.Construct(_progressService);
        }

        public void CreateUIRoot() {
             _uiRoot = _assetProvider.Instantiate(UIRootPath).transform;
        }
    }
}