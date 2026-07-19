namespace DarabnyProject.Models
{
    public class Progress
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public int WatchedSeconds { get; set; }
        public int ProgressPercent { get; set; }
        public DateTime LastAccessedAt { get; set; }

        public string UserId { get; set; } = "";
        public ApplicationUser? User { get; set; }

        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
