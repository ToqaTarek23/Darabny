using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarabnyProject.Models
{
    public class WhatWillLearn
    {
        public int Id { get; set; }
        public string Objective { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
