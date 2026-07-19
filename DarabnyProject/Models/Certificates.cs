namespace DarabnyProject.Models
{
    public class Certificates
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime IssuedAt { get; set; }
        public string? CertificateUrl { get; set; }

        public string UserId { get; set; } = "";
        public ApplicationUser? User { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
