using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentELibrary.Models
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return false;
            }

            if (file.ContentLength > 104857600)
            {
                return false;
            }

            return true;
        }
    }

    public class Resource
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Полето за наслов е задолжително.")]
        [Display(Name = "Наслов")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Полето за автор е задолжително.")]
        [Display(Name = "Автор\\и")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Полето за опис е задолжително.")]
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Полето за јазик е задолжително.")]
        [Display(Name = "Јазик")]
        public string Language { get; set; }
        [Required(ErrorMessage = "Полето за датум на издавање е задолжително.")]
        [Display(Name = "Датум на издавање")]
        public DateTime DatePublished { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Полето за забелешки е задолжително.")]
        [Display(Name = "Забелешки")]
        public string Notes { get; set; }
        [Required(ErrorMessage = "Полето за предмет е задолжително.")]
        [Display(Name = "Предмет")]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        [NotMapped]
        public SelectList AllSubjects { get; set; }
        [Required(ErrorMessage = "Полето за слика е задолжително.")]
        [Display(Name = "Слика [URL]")]
        public string ImageURL { get; set; } = "https://via.placeholder.com/150";
        public string FileName { get; set; }
        [Required(ErrorMessage = "Полето за датотека е задолжително.")]
        [MaxFileSize(ErrorMessage = "Големината на датотеката не смее да надминува 100 MB.")]
        [NotMapped]
        [Display(Name = "Датотека")]
        public HttpPostedFileBase File { get; set; }
    }
}