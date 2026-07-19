using DarabnyProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DarabnyProject.Filters
{
    public class CategoryNavFilter : IActionFilter
    {
        private readonly DarabnyDbContext _db;

        public CategoryNavFilter(DarabnyDbContext db)
        {
            _db = db;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller ctrl)
                ctrl.ViewBag.NavCategories = _db.Categories.ToList();
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
