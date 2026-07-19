using DarabnyProject.Data;
using DarabnyProject.Models;

namespace DarabnyProject.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DarabnyDbContext db;

        public CategoryRepository(DarabnyDbContext db)
        {
            this.db = db;
        }

        public List<Category> GetAll() => db.Categories.ToList();

        public Category? GetById(int id) => db.Categories.Find(id);

        public void Add(Category cat)
        {
            db.Categories.Add(cat);
            db.SaveChanges();
        }

        public void Update(Category cat)
        {
            var existing = db.Categories.Find(cat.Id);
            if (existing == null) return;
            existing.Name        = cat.Name;
            existing.Description = cat.Description;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var cat = db.Categories.Find(id);
            if (cat == null) return;
            db.Categories.Remove(cat);
            db.SaveChanges();
        }

        public void Save() => db.SaveChanges();
    }
}
