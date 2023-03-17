using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentELibrary.Models
{
    public class AddToRole
    {
        [Required(ErrorMessage = "Полето за е-пошта е задолжително.")]
        [Display(Name = "Е-пошта")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Полето за улога е задолжително.")]
        [Display(Name = "Улога")]
        public string SelectedRole { get; set; }
        public List<string> Roles { get; set; }
    }
}