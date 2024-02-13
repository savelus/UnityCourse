using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.UI.Windows {
    public class ShopWindow : WindowBase {
        [SerializeField] private TextMeshProUGUI _currentCoins;

        protected override void Initialize() => 
            CoinsChanged();

        protected override void SubscribeUpdates() => 
            Progress.WorldData.LootData.Changed += CoinsChanged;

        protected override void CleanUp() {
            base.CleanUp();
            Progress.WorldData.LootData.Changed -= CoinsChanged;
        }

        private void CoinsChanged() => 
            _currentCoins.text = Progress.WorldData.LootData.Collected.ToString();
    }
}