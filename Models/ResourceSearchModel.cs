using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentELibrary.Models
{
    public class SubjectFilter
    {
        public Subject Subject { get; set; }
        public bool IsChecked { get; set; }
    }

    public class SemesterFilter
    {
        public enum SemesterType { Autumn, Spring }

        public SemesterType Semester { get; set; }
        public bool IsChecked { get; set; }

        public override string ToString()
        {
            if (Semester == SemesterType.Spring)
                return "Летен";
            else if (Semester == SemesterType.Autumn)
                return "Зимски";
            else
                return String.Empty;
        }
    }

    public class YearFilter
    {
        public enum YearType { First, Second, Third, Fourth }

        public YearType Year { get; set; }
        public bool IsChecked { get; set; }

        public override string ToString()
        {
            if (Year == YearType.First)
                return "Прва";
            else if (Year == YearType.Second)
                return "Втора";
            else if (Year == YearType.Third)
                return "Трета";
            else if (Year == YearType.Fourth)
                return "Четврта";
            else
                return String.Empty;
        }
    }

    public class StudentProgrammeFilter
    {
        public StudyProgramme StudyProgramme { get; set; }
        public bool IsChecked { get; set; }
    }

    public class ResourceSearchModel
    {
        public string SearchString { get; set; }
        public List<SubjectFilter> SubjectFilters { get; set; }
        public List<SemesterFilter> SemesterFilters { get; set; }
        public List<YearFilter> YearFilters { get; set; }
        public List<StudentProgrammeFilter> StudentProgrammeFilters { get; set; }
        public string SelectedSortByOption { get; set; }
        public SelectList SortByOptions { get; set; }
        public bool Ascending { get; set; }
        public List<Resource> Resources { get; set; }
        public int PageSize { get; set; } = 6;
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
    }
}