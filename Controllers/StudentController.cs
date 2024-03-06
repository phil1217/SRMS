using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SRMS.Interfaces;
using SRMS.Models.Database;
using SRMS.Models.Temp;
using SRMS.Models;
using SRMS.Utils;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using SRMS.Models.Config;
using SRMS.Data.Repository;

namespace SRMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly TokenProvider _tokenProvider;

        private readonly MemberOptions _memberOptions;

        private readonly IRepository<StudentRecordModel> StudentRecord;

        private readonly IRepository<AcademicMemberModel> AcademicMember;

        private readonly IRepository<ProfileDetailsModel> ProfileDetails;

        private IEnumerable<StudentRecordModel> StudentRecordList;

        public StudentController(IOptions<MemberOptions> options, IRepository<StudentRecordModel> record, IRepository<AcademicMemberModel> member, IRepository<ProfileDetailsModel> profile,TokenProvider tokenProvider)
        {
            _memberOptions = options.Value;
            AcademicMember = member;
            StudentRecord = record;
            ProfileDetails = profile;
            _tokenProvider = tokenProvider;
            StudentRecordList ??= new List<StudentRecordModel>();
        }

        [Route("/student/signin")]
        public IActionResult Index()
        {
            return View();

        }

        [HttpPost]
        [Route("/student/check")]
        public async Task<IActionResult> Check([FromHeader] string? token)
        {
            if (token == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            var isValid = await isTokenValid(token);

            return Json(new { success = isValid, message = isValid ? "Successfully Authenticated!" : "Unable to Proceed!" });
        }

        [Route("/student/home")]
        public IActionResult Home()
        {

            return View("Home");
        }

        [Route("/student/error")]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [Route("/student/authenticate")]
        public async Task<IActionResult> SignIn([FromBody] AuthenticationOptions? Options)
        {
            if (Options == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            var authenticate = await AcademicMember.Authenticate("student", Options);

            if (authenticate)
            {

                Claim[] claims = new[]
                {
                    new Claim(ClaimTypes.Name,Options.UserName)
                };

                var token = _tokenProvider.GetAccessToken(claims, _memberOptions.Student.SecretKey);

                var student = await AcademicMember.Get("student", Options);

                return Json(new { success = true, message = "Successfully Signed In!", token = token, id = student.UserId});

            }

            return Json(new { success = false, message = "Failed to Sign In!" });
        }

        [HttpPost]
        [Route("/student/data/get/account")]
        public async Task<IActionResult> Get([FromHeader] string? token, [FromBody] QueryExtension<PagingOptions?> query)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            if (query == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            if (!await isTokenValid(token))
                return Json(new { success = false, message = "Unable to Proceed!" });

            var pageCount = await GetPageCount(query.obj.Size, query.id);

            if (pageCount <= query.obj.Index)
                query.obj.Index = pageCount - 1;

            if (query.obj.Index < 1)
                query.obj.Index = 0;

            StudentRecordList = await StudentRecord?.GetAll("student", query.id, query.obj);

            if (!StudentRecordList.IsNullOrEmpty())
            {
                query.obj.Index += 1;
            }

            return Json(new { success = true, page = new { index = query.obj.Index, count = pageCount }, List = StudentRecordList });
        }


        [HttpPost]
        [Route("/student/data/get/profile")]
        public async Task<IActionResult> GetProfile([FromHeader] string? token, [FromBody] Membership? membership)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            if (!await isTokenValid(token))
                return Json(new { success = false, message = "Unable to Proceed!" });

            var profile = ProfileDetails.Get(membership.type, membership.id).Result;

            return Json(new { success = true, profile = profile });
        }

        [HttpPost]
        [Route("/student/data/find")]
        public async Task<IActionResult> Find([FromHeader] string? token, [FromBody] QueryExtension<QueryFilterOptions?> query)
        {
            if (token == null)
                return Json(new { success = false, message = "Unauthorized access!" });

            if (query == null)
                return Json(new { success = false, message = "Invalid JSON!" });

            if (!await isTokenValid(token))
                return Json(new { success = false, message = "Unable to Proceed!" });

            var pageCount = await GetPageCount(query);

            if (pageCount <= query.obj.Page.Index)
                query.obj.Page.Index = pageCount - 1;

            if (query.obj.Page.Index < 1)
                query.obj.Page.Index = 0;

            StudentRecordList = await StudentRecord.FindAll("student", query.id, query.obj);

            if (!StudentRecordList.IsNullOrEmpty())
            {
                query.obj.Page.Index += 1;
            }

            return Json(new { success = true, page = new { index = query.obj.Page.Index, count = pageCount }, List = StudentRecordList });
        }

        private async Task<bool> isTokenValid(string? token)
        {
            var claims = _tokenProvider.GetClaims(token);

            var subClaim = claims.FirstOrDefault(c => c?.Type == ClaimTypes.Name);

            return subClaim != null && await AcademicMember.Has(new AcademicMemberModel { UserName = subClaim.Value, UserType = "student" });
        }

        private async Task<int> GetPageCount(int? pageSize, string? id)
        {
            return (int)Math.Ceiling((double)(await StudentRecord.Count("student", id)) / pageSize ?? 1);
        }

        private async Task<int> GetPageCount(QueryExtension<QueryFilterOptions> filter)
        {
            return (int)Math.Ceiling((double)(await StudentRecord.Count("student", filter.id, filter.obj.Query)) / filter.obj.Page.Size ?? 1);
        }


    }
}
