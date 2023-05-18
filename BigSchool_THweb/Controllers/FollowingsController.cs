using BigSchool_THweb.DTOs;
using BigSchool_THweb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool_THweb.Controllers
{
    public class FollowingsController : ApiController
    {
        private readonly BigSchoolContext context;

        public FollowingsController()
        {
            context = new BigSchoolContext();
        }
        [HttpPost]
        public IHttpActionResult Follow(FollowingDto followingDto)
        {
            var loginUser = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loginUser))
            {
                return BadRequest("Login user not found");
            }

            if (followingDto == null || followingDto.FolloweeId == null)
            {
                return BadRequest("FolloweeId is required");
            }

            var existingFollow = context.Followings.FirstOrDefault(f => f.FollowerId == loginUser && f.FolloweeId == followingDto.FolloweeId);

            if (existingFollow == null)
            {
                // Nếu không có theo dõi, thêm vào CSDL
                var follow = new Following
                {
                    FollowerId = loginUser,
                    FolloweeId = followingDto.FolloweeId
                };
                context.Followings.Add(follow);
            }
            else
            {
                // Nếu đã có theo dõi, xóa khỏi CSDL
                context.Followings.Remove(existingFollow);
            }

            // Lưu thay đổi vào CSDL
            context.SaveChanges();

            // Trả về kết quả thành công
            return Ok();
        }


    }
}
