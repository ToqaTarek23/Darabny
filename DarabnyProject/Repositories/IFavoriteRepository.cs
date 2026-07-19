namespace DarabnyProject.Repositories
{
    public interface IFavoriteRepository
    {
        bool IsFavorited(string userId, int courseId);
        void Add(string userId, int courseId);
        void Remove(string userId, int courseId);
    }
}
