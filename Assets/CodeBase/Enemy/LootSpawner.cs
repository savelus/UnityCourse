using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.RandomService;
using UnityEngine;

namespace CodeBase.Enemy {
    public class LootSpawner : MonoBehaviour {
        public EnemyDeath EnemyDeath;
        private IGameFactory _factory;
        private IRandomService _random;

        private int _lootMax;
        private int _lootMin;

        public void Construct(IGameFactory gameFactory, 
                              IRandomService randomService) {
            _factory = gameFactory;
            _random = randomService;
        }

        private void Start() {
            EnemyDeath.Happened += SpawnLoot;
        }

        private void SpawnLoot() {
            LootPiece loot = _factory.CreateLoot();
            loot.transform.position = transform.position;

            var lootItem = GenerateLoot();
            
            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot() {
            return new Loot {
                Value = _random.Next(_lootMin, _lootMax)
            };
        }

        public void SetLoot(int min, int max) {
            _lootMin = min;
            _lootMax = max;
        }
    }
}