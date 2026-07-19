using DarabnyProject.Repositories;

namespace DarabnyProject.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressRepository progressRepo;

        public ProgressService(IProgressRepository progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public bool IsCompleted(string userId, int lessonId)
            => progressRepo.IsCompleted(userId, lessonId);

        public void MarkComplete(string userId, int lessonId)
            => progressRepo.MarkComplete(userId, lessonId);

        public int GetCourseProgress(string userId, int courseId)
            => progressRepo.GetCourseProgress(userId, courseId);
    }
}
