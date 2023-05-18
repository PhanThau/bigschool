using BigSchool_THweb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace BigSchool_THweb.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            var userID = User.Identity.GetUserId(); // get login user
            //lấy danh sách khóa học mà userlogin đó tham dự(ở đây chỉ lấy được id)
            var listAttendances = context.Attendances.Where(p => p.Attendee == userID).ToList();
            var courses = new List<Course>();
            //Tìm chi tiết khóa học từ listAttendances(mã khóa học, tên gv phải truy cập từ asnetuser.name)
            foreach(Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }

        [Authorize]
        public ActionResult Mine()
        {
            
            var loginUser = User.Identity.GetUserId(); // get login user
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(loginUser);
            BigSchoolContext context = new BigSchoolContext();
            //lấy danh sách khóa học mà userlogin đó tham dự(ở đây chỉ lấy được id)
            var courses = context.Courses.Where(c => c.LecturerId == loginUser && c.IsCanceled != true).ToList();
            foreach (Course i in courses)
            {
                i.Name = user.Name;
            }
            return View(courses);
        }

        public ActionResult Delete(int id)
        {
            var UserID = User.Identity.GetUserId(); // get login user
            BigSchoolContext context = new BigSchoolContext();
            //lấy danh sách khóa học mà userlogin đó tham dự(ở đây chỉ lấy được id)
            var findCourse = context.Courses.FirstOrDefault(p => p.Id == id);
            findCourse.IsCanceled = true;
            context.SaveChanges();
            return RedirectToAction("Mine");
        }
    }
}