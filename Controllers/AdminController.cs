using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SRMS.Interfaces;
using SRMS.Models;
using SRMS.Models.Config;
using SRMS.Models.Database;
using SRMS.Models.Temp;
using SRMS.Utils;
using System.Diagnostics;
using System.Security.Claims;

namespace SRMS.Controllers
{

    public class AdminController : Controller
    {
        private readonly AdminOptions admin;

        private readonly TokenProvider _tokenProvider;

        private readonly IRepository<AcademicMemberModel> AcademicMember;
        private readonly IRepository<ProfileDetailsModel> ProfileDetails;
        private readonly IRepository<StudentRecordModel> StudentRecord;

        private IEnumerable<AcademicMemberModel> MemberList;

        public AdminController(IOptions<AdminOptions> options, IRepository<AcademicMemberModel> member, IRepository<ProfileDetailsModel> profile, IRepository<StudentRecordModel> record, TokenProvider tokenProvider) {
            admin ??= options.Value;
            AcademicMember = member;
            StudentRecord = record;
            ProfileDetails = profile;
            _tokenProvider = tokenProvider;
            MemberList ??= new List<AcademicMemberModel>();
        }

        [Route("/admin/signin")]
        public IActionResult Index()
        {
            return View();

        }

        [HttpPost]
        [Route("/admin/check")]
        public IActionResult Check([FromHeader] string? token)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            var isValid = isTokenValid(token);

            return Json(new { success = isValid, message = isValid ? "Successfully Authenticated!":"Unable to Proceed!"});
        }

        [Route("/admin/home")]
        public IActionResult Home()
        {

            return View("Home");
        }

        [Route("/admin/error")]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [Route("/admin/authenticate")]
        public IActionResult SignIn([FromBody] AuthenticationOptions? Params)
        {
            if (Params == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            var authenticate = (Params.UserName.Equals(admin.UserName) &&
                        (Params.Password.Equals(admin.Password)));

            if (authenticate)
            {

                Claim[] claims = new[]
                {
                    new Claim(ClaimTypes.Name,Params.UserName)
                };

                var token = _tokenProvider.GetAccessToken(claims, admin.Jwt.SecretKey);

                return Json(new { success = true, message = "Successfully Signed In!", token = token });

            }

            return Json(new { success = false, message = "Failed to Sign In!" });
        }

        [HttpPost]
        [Route("/admin/data/add")]
        public async Task<IActionResult> Add([FromHeader] string? token, [FromBody] AcademicMemberInfo? Member)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            if (Member == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            if(!isTokenValid(token))
                return Json(new { success = false, message = "Unable to Proceed!" });

            if (await AcademicMember.Has(Member.account))
                return Json(new { success = false, message = "User Name or User Id is Already Existed!" });

          await AcademicMember.Add(Member.account);
          await ProfileDetails.Add(Member.profile);

            return Json(new { success = true, message = "Successfully Added!"});
        }

        [HttpPost]
        [Route("/admin/data/update")]
        public async Task<IActionResult> Update([FromHeader] string? token,[FromBody] AcademicMemberInfo? Member)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            if (Member == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            if (!isTokenValid(token))
                return Json(new { success = false, message = "Unable to Proceed!" });

            if (!await AcademicMember.CanUpdate(Member.account))
                return Json(new { success=false,message="Duplicate Entry!" });

            await AcademicMember.Update(Member.account);
            await ProfileDetails.Update(Member.profile);
            
            return Json(new { success = true, message = "Successfully Updated!"});
        }

        [HttpPost]
        [Route("/admin/data/delete")]
        public async Task<IActionResult> Delete([FromHeader] string? token,[FromBody]AcademicMemberModel? Member)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            if (Member == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            if (!isTokenValid(token))
                return Json(new { success = false, message = "Unable to Proceed!" });

            if (!(await AcademicMember.Has(Member)))
                return Json(new { success = false, message="Does not exist!"});
            
            await AcademicMember.Delete(Member);
            await ProfileDetails.Delete(Member.UserType, Member.UserId);
            await StudentRecord.Delete("faculty",Member.UserId);

            return Json(new { success = true, message = "Successfully Deleted!"});
        }

        [HttpPost]
        [Route("/admin/data/get/account")]
        public async Task<IActionResult> GetAccount([FromHeader] string? token,[FromBody] PagingOptions? page)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            if (page == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            if (!isTokenValid(token))
                return Json(new { success = false, message = "Unable to Proceed!" });

            var pageCount = await GetPageCount(page.Size);

            if (pageCount <= page.Index)
                page.Index = pageCount-1;

            if (page.Index < 1)
                page.Index = 0;

            MemberList = await AcademicMember?.GetAll(page);

            if (!MemberList.IsNullOrEmpty())
            {
                page.Index += 1;
            }
            
            return Json(new {success=true, page=new {index = page.Index, count = pageCount},memberList=MemberList});
        }

        [HttpPost]
        [Route("/admin/data/get/profile")]
        public async Task<IActionResult> GetProfile([FromHeader] string? token, [FromBody] Membership? membership)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            if (!isTokenValid(token))
                return Json(new { success = false, message = "Unable to Proceed!" });

            var profile = ProfileDetails.Get(membership.type, membership.id).Result;

            return Json(new { success = true, profile = profile});
        }


        [HttpPost]
        [Route("/admin/data/find")]
        public async Task<IActionResult> Find([FromHeader] string? token,[FromBody] QueryFilterOptions? filter)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            if (filter == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            if (!isTokenValid(token))
                return Json(new { success = false, message = "Unable to Proceed!" });

            var pageCount = await GetPageCount(filter);

            if (pageCount <= filter.Page.Index)
                filter.Page.Index = pageCount - 1;

            if (filter.Page.Index < 1)
                filter.Page.Index = 0;

            MemberList = await AcademicMember.FindAll(filter);

            if (!MemberList.IsNullOrEmpty())
            {
                filter.Page.Index += 1;
            }

            return Json(new { success = true,page = new { index = filter.Page.Index, count = pageCount }, memberList = MemberList });
        }

        private bool isTokenValid(string token)
        {
            var claims = _tokenProvider.GetClaims(token);

            var subClaim = claims.FirstOrDefault(c => c?.Type == ClaimTypes.Name);

            return subClaim != null && admin.UserName.Equals(subClaim.Value);
        }

        private async Task<int> GetPageCount(int? pageSize)
        {
            return (int)Math.Ceiling((double)(await AcademicMember.Count()) / pageSize ?? 1);
        }

        private async Task<int> GetPageCount(QueryFilterOptions filter)
        {
            return (int)Math.Ceiling((double)(await AcademicMember.Count(filter.Query)) / filter.Page.Size ?? 1);
        }

    }
}
