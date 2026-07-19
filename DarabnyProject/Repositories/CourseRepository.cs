using DarabnyProject.Data;
using DarabnyProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DarabnyProject.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DarabnyDbContext db;

        public CourseRepository(DarabnyDbContext db)
        {
            this.db = db;
        }

        public List<Course> GetAll() => db.Courses.ToList();

        public List<Course> GetAllWithCategory()
            => db.Courses.Include(c => c.Category).ToList();

        public Course? GetById(int id) => db.Courses.Find(id);

        public Course? GetWithDetails(int id)
            => db.Courses
                .Include(c => c.Category)
                .Include(c => c.Requirements)
                .Include(c => c.WhatWillLearn)
                .Include(c => c.Chapters).ThenInclude(ch => ch.Lessons)
                .FirstOrDefault(c => c.Id == id);

        public Course? GetWithRequirements(int id)
            => db.Courses
                .Include(c => c.Requirements)
                .Include(c => c.WhatWillLearn)
                .FirstOrDefault(c => c.Id == id);

        public List<Course> Search(string? query, int? categoryId, string? sort)
        {
            var q = db.Courses.Include(c => c.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
                q = q.Where(c =>
                    c.Name.Contains(query) ||
                    c.Description.Contains(query));

            if (categoryId.HasValue && categoryId.Value > 0)
                q = q.Where(c => c.CategoryId == categoryId.Value);

            q = sort switch
            {
                "priceAsc"  => q.OrderBy(c => c.Price),
                "priceDesc" => q.OrderByDescending(c => c.Price),
                "newest"    => q.OrderByDescending(c => c.LastUpdated),
                _           => q.OrderBy(c => c.Id)
            };

            return q.ToList();
        }

        public void Add(Course course, string? requirementsText, string? learnText)
        {
            db.Courses.Add(course);
            db.SaveChanges();

            if (!string.IsNullOrWhiteSpace(requirementsText))
                db.Requirements.Add(new Requirements
                {
                    Requirement = requirementsText.Trim(),
                    CourseId    = course.Id
                });

            if (!string.IsNullOrWhiteSpace(learnText))
                db.WhatWillLearn.Add(new WhatWillLearn
                {
                    Objective = learnText.Trim(),
                    CourseId  = course.Id
                });

            db.SaveChanges();
        }

        public void Update(Course course, string? requirementsText, string? learnText)
        {
            var existing = db.Courses.Find(course.Id);
            if (existing == null) return;

            existing.Name         = course.Name;
            existing.Description  = course.Description;
            existing.Price        = course.Price;
            existing.PriceType    = course.PriceType;
            existing.CategoryId   = course.CategoryId;
            existing.Level        = course.Level;
            existing.Language     = course.Language;
            existing.CourseLength = course.CourseLength;
            existing.LastUpdated  = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(requirementsText))
            {
                var req = db.Requirements.FirstOrDefault(r => r.CourseId == course.Id);
                if (req != null) req.Requirement = requirementsText.Trim();
                else db.Requirements.Add(new Requirements { Requirement = requirementsText.Trim(), CourseId = course.Id });
            }

            if (!string.IsNullOrWhiteSpace(learnText))
            {
                var learn = db.WhatWillLearn.FirstOrDefault(w => w.CourseId == course.Id);
                if (learn != null) learn.Objective = learnText.Trim();
                else db.WhatWillLearn.Add(new WhatWillLearn { Objective = learnText.Trim(), CourseId = course.Id });
            }

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var course = GetWithDetails(id);
            if (course == null) return;
            db.Courses.Remove(course);
            db.SaveChanges();
        }

        public void Save() => db.SaveChanges();

        public void Add(Course entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Course entity)
        {
            throw new NotImplementedException();
        }
    }
}
