using DarabnyProject.Enums;

namespace DarabnyProject.ViewModels
{
    public class CourseCardViewModel
    {
        public int    Id           { get; set; }
        public string Name         { get; set; } = "";
        public string Description  { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public int    CategoryId   { get; set; }
        public string Level        { get; set; } = "";
        public string Language     { get; set; } = "";
        public decimal Price       { get; set; }
        public PriceType PriceType { get; set; }

        public string DisplayPrice =>
            PriceType == PriceType.Free ? "Free" : $"EGP {Price:0}";
    }
}
