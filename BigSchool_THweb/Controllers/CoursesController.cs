using BigSchool_THweb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool_THweb.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            //get list category
            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.listCategory = context.Categories.ToList();
            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            //get list category
            BigSchoolContext context = new BigSchoolContext();

            // Không xét valid LectureId vì bằng user đăng nhập
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.listCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }

            // Lay login usser Id
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            //add vao csdl
            context.Courses.Add(objCourse);
            context.SaveChanges();


            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var loginUser = User.Identity.GetUserId(); // get login user
            BigSchoolContext context = new BigSchoolContext();
            //lấy danh sách khóa học mà userlogin đó tham dự(ở đây chỉ lấy được id)
            var course = context.Courses.FirstOrDefault(c => c.LecturerId == loginUser && c.Id == id);
            if (course == null)
                return HttpNotFound("Không tìm thấy khóa học");
            course.listCategory = context.Categories.ToList();
            return View("Create", course);
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult Edit(Course editCourse)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        editCourse.listCategory = new BigSchoolContext().Categories.ToList();
        //        return View("Create", editCourse);
        //    }

        //    var loginUser = User.Identity.GetUserId(); // get login user
        //    BigSchoolContext context = new BigSchoolContext();
        //    var course = context.Courses.FirstOrDefault(c => c.LecturerId == loginUser && c.Id == editCourse.Id);
        //    if (course == null)
        //        return HttpNotFound("Không tìm thấy khóa học");

        //    // update course properties
        //    course.Place = editCourse.Place;
        //    course.DateTime = editCourse.DateTime;
        //    course.CategoryId = editCourse.CategoryId;

        //    context.SaveChanges();

        //    return RedirectToAction("Index", "Home");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course editCourse)
        {
            if (ModelState.IsValid)
            {
                editCourse.listCategory = new BigSchoolContext().Categories.ToList();
                return View("Create", editCourse);
            }

            var loginUser = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            var course = context.Courses.FirstOrDefault(c => c.LecturerId == loginUser && c.Id == editCourse.Id);
            if (course == null)
                return HttpNotFound("Không tìm thấy khóa học");

            Debug.WriteLine("Before update: ");
            Debug.WriteLine("Category Id: " + course.CategoryId);
            Debug.WriteLine("Datetime: " + course.DateTime);

            course.Place = editCourse.Place;
            course.DateTime = editCourse.DateTime;
            course.CategoryId = editCourse.CategoryId;

            context.SaveChanges();

            Debug.WriteLine("After update: ");
            Debug.WriteLine("Category Id: " + course.CategoryId);
            Debug.WriteLine("Datetime: " + course.DateTime);

            return RedirectToAction("Index", "Home");
        }


    }
}