using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentELibrary.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Полето за име е задолжително.")]
        [Display(Name = "Име")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Полето за семестар е задолжително.")]
        [Range(1, 2, ErrorMessage = "Полето за семестар прима вредности 1 (за зимски семестар) и 2 (за летен семестар).")]
        [Display(Name = "Семестар")]
        public int Semester { get; set; } = 1;
        [Required(ErrorMessage = "Полето за година е задолжително.")]
        [Range(1, 4, ErrorMessage = "Полето за година прима целобројни вредности во интервалот од 1 до 4.")]
        [Display(Name = "Година")]
        public int Year { get; set; } = 1;
        [Required(ErrorMessage = "Полето за студиска насока е задолжително.")]
        [Display(Name = "Студиска насока")]
        public int StudyProgrammeId { get; set; }
        public virtual StudyProgramme StudyProgramme { get; set; }
        [NotMapped]
        public SelectList AllStudyProgrammes { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}. семестар, {2}. година - {3}", Name, Semester, Year, StudyProgramme);
        }
    }
}