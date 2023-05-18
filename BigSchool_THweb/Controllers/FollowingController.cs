using BigSchool_THweb.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool_THweb.Controllers
{
    public class FollowingController : Controller
    {
        // GET: Following
        public ActionResult Following()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser loginUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listFollowings = context.Followings.Where(p => p.FollowerId == loginUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Following temp in listFollowings)
            {
                var listCourse = context.Courses.Where(p => p.LecturerId == temp.FolloweeId).ToList();
                if (listCourse.Count > 0)
                {
                    string Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(listCourse[0].LecturerId).Name;
                    foreach (Course i in listCourse)
                        i.Name = Name;
                    courses.AddRange(listCourse);
                }
            }

            return View(courses);
        }
    }
}