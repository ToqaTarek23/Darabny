using DarabnyProject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarabnyProject.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LessonType Type { get; set; }
        public int LessonLength { get; set; }
        public bool IsFreePreview { get; set; }
        public string LessonURL { get; set; }
        //
        public int ChapterId { get; set; }
        public Chapter? Chapter { get; set; }
        public List<Progress>? Progresses { get; set; }
    }
}
