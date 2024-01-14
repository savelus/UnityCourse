using System;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using TMPro.EditorUtilities;
using UnityEngine;

namespace CodeBase.Logic {
    public class EnemySpawner : MonoBehaviour, ISavedProgress {
        public MonsterTypeId MonsterTypeId;
        private string _id;

        public bool Slain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        private void Awake() {
            _id = GetComponent<UniqueId>().Id;
            _factory = AllServices.Container.Single<IGameFactory>();
        }

        public void LoadProgress(PlayerProgress progress) {
            if (progress.KillData.ClearedSpawners.Contains(_id))
                Slain = true;
            else {
                Spawn();
            }
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

        public void UpdateProgress(PlayerProgress progress) {
            if (Slain)
                progress.KillData.ClearedSpawners.Add(_id);
        }
    }
}