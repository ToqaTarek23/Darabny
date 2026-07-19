namespace DarabnyProject.Repositories
{
    public interface IProgressRepository
    {
        bool IsCompleted(string userId, int lessonId);
        void MarkComplete(string userId, int lessonId);
        int GetCourseProgress(string userId, int courseId);
    }
}
