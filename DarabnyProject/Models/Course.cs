using DarabnyProject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarabnyProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; } 
        public PriceType PriceType { get; set; }
        public int StudentNumbers { get; set; }
        public int CourseLength { get; set; }
        public CourseLevel Level { get; set; }
        public string Language { get; set; }
        public int ReviewsNumbers { get; set; }
        public DateTime LastUpdated { get; set; }
        //
        public string? InstructorId { get; set; }
        public ApplicationUser? Instructor { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Chapter>? Chapters { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<Enrollment>? Enrollments { get; set; }
        public List<Favorites>? Favorites { get; set; }
        public List<Certificates>? Certificates { get; set; }
        public List<Requirements>? Requirements { get; set; }
        public List<WhatWillLearn>? WhatWillLearn { get; set; }

    }
}
