using System;

namespace CodeBase.Infrastructure.Services.RandomService {
    public class UnityRandomService : IRandomService {
        private Random _random = new();

        public int Next(int lootMin, int lootMax) => _random.Next(lootMin, lootMax);
    }
}