using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarabnyProject.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LessonCounter { get; set; }
        public int ChapterLength { get; set; }
        //
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public List<Lesson>? Lessons { get; set; }

    }
}
