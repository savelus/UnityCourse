namespace CodeBase.Infrastructure.Services.RandomService {
    public interface IRandomService : IService {
        int Next(int lootMin, int lootMax);
    }
}