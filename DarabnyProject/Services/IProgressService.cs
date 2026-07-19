namespace DarabnyProject.Services
{
    public interface IProgressService
    {
        bool IsCompleted(string userId, int lessonId);
        void MarkComplete(string userId, int lessonId);
        int GetCourseProgress(string userId, int courseId);
    }
}
