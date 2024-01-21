using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners {
    public class SpawnPoint : MonoBehaviour, ISavedProgress {
        public MonsterTypeId MonsterTypeId;
        public string Id { get; set; }

        public bool Slain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public void Construct(IGameFactory factory) {
            _factory = factory;
        }
        public void LoadProgress(PlayerProgress progress) {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                Slain = true;
            else
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress) {
            if (Slain)
                progress.KillData.ClearedSpawners.Add(Id);
        }

        private void Spawn() {
           GameObject monster = _factory.CreateMonster(MonsterTypeId, transform);
           _enemyDeath = monster.GetComponent<EnemyDeath>();
           _enemyDeath.Happened += Slay;
        }

        private void Slay() {
            Slain = true;
            
            if (_enemyDeath != null) 
                _enemyDeath.Happened -= Slay;
        }
    }
}