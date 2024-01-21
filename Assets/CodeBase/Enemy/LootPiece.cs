using System.Collections;
using System.Security.Cryptography;
using CodeBase.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Enemy {
    public class LootPiece : MonoBehaviour {
        public GameObject Skull;
        public GameObject PickUpFxPrefab;
        public TMP_Text LootText;
        public GameObject PickUpPopUp;
        
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Construct(WorldData worldData) {
            _worldData = worldData;
        }
        
        public void Initialize(Loot loot) {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other) {
            PickUp();
        }

        private void PickUp() {
            if(_picked) return;
            
            _picked = true;

            UpdateWorldData();

            HideSkull();
            
            PlayPickUpFx();
            ShowText();
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData() {
            _worldData.LootData.Collect(_loot);
        }

        private void HideSkull() {
            Skull.SetActive(false);
        }

        private IEnumerator StartDestroyTimer() {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }

        private void PlayPickUpFx() {
            Instantiate(PickUpFxPrefab, transform.position, Quaternion.identity);
        }

        private void ShowText() {
            LootText.text = $"{_loot.Value}";
            PickUpPopUp.SetActive(true);
        }
    }
}