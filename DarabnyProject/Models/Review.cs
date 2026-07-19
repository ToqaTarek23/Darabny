namespace DarabnyProject.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; } = "";
        public decimal Rating { get; set; }
        public bool IsApproved { get; set; }
        public DateTime AddedTime { get; set; }

        public string UserId { get; set; } = "";
        public ApplicationUser? User { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
