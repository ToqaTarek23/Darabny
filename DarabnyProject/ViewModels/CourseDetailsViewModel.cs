using DarabnyProject.Enums;
using DarabnyProject.Models;

namespace DarabnyProject.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public PriceType PriceType { get; set; }
        public CourseLevel Level { get; set; }
        public string Language { get; set; } = "";
        public int CourseLength { get; set; }
        public int StudentNumbers { get; set; }
        public DateTime LastUpdated { get; set; }

        public string CategoryName { get; set; } = "";
        public string InstructorName { get; set; } = "";
        public string? InstructorBio { get; set; }

        public List<string> Requirements { get; set; } = new();
        public List<string> WhatWillLearn { get; set; } = new();
        public List<Chapter> Chapters { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();

        public bool IsEnrolled  { get; set; }
        public bool IsFavorited { get; set; }
        public bool HasReviewed { get; set; }
        public HashSet<int> CompletedLessonIds { get; set; } = new();
        public int CourseProgressPercent { get; set; }

        public decimal AverageRating =>
            Reviews.Any() ? Math.Round(Reviews.Average(r => r.Rating), 1) : 0;
    }
}
