using SpaceTraders.Model;

namespace SpaceTraders
{
    public interface IGlobalStateRepository
    {
        Task Clear();
        Task<GlobalState?> Get();
        Task Persist(GlobalState globalState);
    }
}
