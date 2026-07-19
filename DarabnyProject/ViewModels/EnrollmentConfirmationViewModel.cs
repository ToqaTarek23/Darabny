using System;

namespace DarabnyProject.ViewModels
{
    public class EnrollmentConfirmationViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string PaymentStatus { get; set; } = "Paid";
        public string TransactionId { get; set; } = string.Empty;
        public DateTime ConfirmedAt { get; set; }
        public bool IsFreeCourse { get; set; }
    }
}