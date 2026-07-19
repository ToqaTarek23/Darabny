using DarabnyProject.Repositories;

namespace DarabnyProject.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository favoriteRepo;

        public FavoriteService(IFavoriteRepository favoriteRepo)
        {
            this.favoriteRepo = favoriteRepo;
        }

        public bool IsFavorited(string userId, int courseId)
            => favoriteRepo.IsFavorited(userId, courseId);

        public void Toggle(string userId, int courseId)
        {
            if (favoriteRepo.IsFavorited(userId, courseId))
                favoriteRepo.Remove(userId, courseId);
            else
                favoriteRepo.Add(userId, courseId);
        }
    }
}
