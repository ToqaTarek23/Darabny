namespace DarabnyProject.Services
{
    public interface IFavoriteService
    {
        bool IsFavorited(string userId, int courseId);
        void Toggle(string userId, int courseId);
    }
}
