using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentELibrary.Models;

namespace StudentELibrary.Controllers
{
    [Authorize]
    public class ResourcesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Resources
        [AllowAnonymous]
        public ActionResult Index()
        {
            ResourceSearchModel model = new ResourceSearchModel();

            model.SubjectFilters = new List<SubjectFilter>();
            var subjects = db.Subjects.ToList();
            subjects.Sort((subject1, subject2) => subject1.Name.CompareTo(subject2.Name));
            subjects.ForEach(subject => model.SubjectFilters.Add(new SubjectFilter() { Subject = subject, IsChecked = false }));

            model.SemesterFilters = new List<SemesterFilter>()
            {
                new SemesterFilter()
                {
                    Semester = SemesterFilter.SemesterType.Autumn,
                    IsChecked = false
                },
                new SemesterFilter()
                {
                    Semester = SemesterFilter.SemesterType.Spring,
                    IsChecked = false
                }
            };

            model.YearFilters = new List<YearFilter>()
            {
                new YearFilter()
                {
                    Year = YearFilter.YearType.First,
                    IsChecked = false
                },
                new YearFilter()
                {
                    Year = YearFilter.YearType.Second,
                    IsChecked = false
                },
                new YearFilter()
                {
                    Year = YearFilter.YearType.Third,
                    IsChecked = false
                },
                new YearFilter()
                {
                    Year = YearFilter.YearType.Fourth,
                    IsChecked = false
                }
            };

            model.StudentProgrammeFilters = new List<StudentProgrammeFilter>();
            db.StudyProgrammes.ToList().ForEach(studyProgramme => model.StudentProgrammeFilters.Add(new StudentProgrammeFilter() { StudyProgramme = studyProgramme, IsChecked = false }));

            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Наслов", Value = "Title" },
                new SelectListItem() { Text = "Автор", Value = "Author" },
                new SelectListItem() { Text = "Јазик", Value = "Language" },
                new SelectListItem() { Text = "Предмет", Value = "Subject" }
            };
            model.SortByOptions = new SelectList(list, "Value", "Text");

            model.Ascending = true;

            model.TotalPages = (int)Math.Ceiling((decimal)db.Resources.Count() / model.PageSize);

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RenderResources(ResourceSearchModel model)
        {
            List<Resource> resources = db.Resources.ToList();

            if (model.SubjectFilters != null)
            {
                List<int> selectedSubjectsIds = model.SubjectFilters.Where(filter => filter.IsChecked)
                .Select(filter => filter.Subject.Id)
                .ToList();

                if (selectedSubjectsIds.Count > 0)
                    resources = resources.Where(resource => selectedSubjectsIds.Contains(resource.Subject.Id)).ToList();
            }

            List<int> selectedSemesters = model.SemesterFilters.Where(filter => filter.IsChecked)
                .Select(filter => (int)filter.Semester + 1)
                .ToList();

            if (selectedSemesters.Count > 0)
                resources = resources.Where(resource => selectedSemesters.Contains(resource.Subject.Semester)).ToList();

            List<int> selectedYears = model.YearFilters.Where(filter => filter.IsChecked)
                .Select(filter => (int)filter.Year + 1)
                .ToList();
            if (selectedYears.Count > 0)
                resources = resources.Where(resource => selectedYears.Contains(resource.Subject.Year)).ToList();

            if (model.StudentProgrammeFilters != null)
            {
                List<int> selectedStudyProgrammeIds = model.StudentProgrammeFilters.Where(filter => filter.IsChecked)
                .Select(filter => filter.StudyProgramme.Id)
                .ToList();
                if (selectedStudyProgrammeIds.Count > 0)
                    resources = resources.Where(resource => selectedStudyProgrammeIds.Contains(resource.Subject.StudyProgramme.Id)).ToList();
            }

            if (!String.IsNullOrEmpty(model.SearchString))
            {
                model.SearchString = model.SearchString.ToLower();
                resources = resources.Where(resource =>
                {
                    return resource.Title.ToLower().Contains(model.SearchString) ||
                    resource.Author.ToLower().Contains(model.SearchString) ||
                    resource.Subject.Name.ToLower().Contains(model.SearchString);
                }).ToList();
            }

            if (model.SelectedSortByOption == "Title")
                resources.Sort((resource1, resource2) =>
                {
                    if (model.Ascending)
                        return resource1.Title.CompareTo(resource2.Title);
                    else
                        return resource2.Title.CompareTo(resource1.Title);
                });
            else if (model.SelectedSortByOption == "Author")
                resources.Sort((resource1, resource2) =>
                {
                    if (model.Ascending)
                        return resource1.Author.CompareTo(resource2.Author);
                    else
                        return resource2.Author.CompareTo(resource1.Author);
                });
            else if (model.SelectedSortByOption == "Language")
                resources.Sort((resource1, resource2) =>
                {
                    if (model.Ascending)
                        return resource1.Language.CompareTo(resource2.Language);
                    else
                        return resource2.Language.CompareTo(resource1.Language);
                });
            else if (model.SelectedSortByOption == "Subject")
                resources.Sort((resource1, resource2) =>
                {
                    if (model.Ascending)
                        return resource1.Subject.Name.CompareTo(resource2.Subject.Name);
                    else
                        return resource2.Subject.Name.CompareTo(resource1.Subject.Name);
                });

            model.TotalPages = (int)Math.Ceiling((decimal)resources.Count / model.PageSize);

            int count = (model.PageSize < resources.Count) ? model.PageSize : resources.Count;
            model.Resources = resources.Skip(model.PageSize * (model.PageNumber - 1)).Take(count).ToList();

            if (model.Resources.Count == 0 && model.PageNumber > 0)
            {
                --model.PageNumber;
                count = (model.PageSize < resources.Count) ? model.PageSize : resources.Count;
                model.Resources = resources.Skip(model.PageSize * (model.PageNumber - 1)).Take(count).ToList();
            }

            return PartialView(model);
        }

        [AllowAnonymous]

        // GET: Resources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        [Authorize(Roles = "Administrator,Teacher")]
        // GET: Resources/Create
        public ActionResult Create()
        {
            Resource resource = new Resource();
            var subjects = db.Subjects.ToList();
            subjects.Sort((subject1, subject2) => subject1.Name.CompareTo(subject2.Name));
            List<SelectListItem> list = subjects.Select(subject => new SelectListItem() { Text = subject.Name, Value = subject.Id.ToString() }).ToList();
            resource.AllSubjects = new SelectList(list, "Value", "Text");

            return View(resource);
        }

        // POST: Resources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Author,Description,Language,DatePublished,Notes,SubjectId,ImageURL,FileName,File")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                var fileName = string.Concat(
                       Path.GetFileNameWithoutExtension(resource.File.FileName),
                       DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                       Path.GetExtension(resource.File.FileName)
                   );
                var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
                resource.File.SaveAs(path);

                resource.FileName = fileName;

                db.Resources.Add(resource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var subjects = db.Subjects.ToList();
            subjects.Sort((subject1, subject2) => subject1.Name.CompareTo(subject2.Name));
            List<SelectListItem> list = subjects.Select(subject => new SelectListItem() { Text = subject.Name, Value = subject.Id.ToString() }).ToList();
            resource.AllSubjects = new SelectList(list, "Value", "Text");

            return View(resource);
        }

        // GET: Resources/Edit/5
        [Authorize(Roles = "Administrator,Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> list = db.Subjects.Select(subject => new SelectListItem() { Text = subject.Name, Value = subject.Id.ToString() }).ToList();
            resource.AllSubjects = new SelectList(list, "Value", "Text");

            return View(resource);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Author,Description,Language,DatePublished,Notes,SubjectId,ImageURL,FileName")] Resource resource)
        {
            ModelState.Remove("File");
            if (ModelState.IsValid)
            {
                db.Entry(resource).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                return RedirectToAction("Index");
            }

            List<SelectListItem> list = db.Subjects.Select(subject => new SelectListItem() { Text = subject.Name, Value = subject.Id.ToString() }).ToList();
            resource.AllSubjects = new SelectList(list, "Value", "Text");

            return View(resource);
        }

        // GET: Resources/Delete/5
        [Authorize(Roles = "Administrator,Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Resource resource = db.Resources.Find(id);
            if (resource == null)
                return HttpNotFound();

            return PartialView(resource);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator,Teacher")]
        public void DeleteConfirmed(int id)
        {
            Resource resource = db.Resources.Find(id);

            string path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), resource.FileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            db.Resources.Remove(resource);
            db.SaveChanges();
        }

        [AllowAnonymous]
        public ActionResult DownloadFile(string fileName, string title)
        {
            var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, title);
        }

        [AllowAnonymous]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
