using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentELibrary.Models
{
    public class StudyProgramme
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Полето за име е задолжително.")]
        [Display(Name = "Име")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}