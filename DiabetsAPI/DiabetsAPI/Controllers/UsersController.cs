using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using DiabetsAPI.Middleware;
using DiabetsAPI.Models.Requests;
using DiabetsAPI.Models.Responses;
using DiabetsAPI.Services;
using DiabetsAPI.Helpers;

namespace DiabetsAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : BaseController
    {
        private IUserService userService;
        private readonly AppSettings appSettings;

        public UsersController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            this.userService = userService;
            this.appSettings = appSettings.Value;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest authenticateRequest)
        {
            AuthenticateResponse user = userService.Authenticate(authenticateRequest);

            if (user == null)
            {
                return NotFound();
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return Ok(user);
        }

        [Authorize]
        [HttpPost("create-doctor")]
        public IActionResult CreateDoctor(CreateDoctorRequest createDoctorRequest)
        {
            if (!Account.IsAdmin)
            {
                return StatusCode(403, new { message = "Unauthorized" });
            }

            if (userService.CreateDoctor(createDoctorRequest) == null)
            {
                return StatusCode(400);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("create-patient")]
        public IActionResult CreatePatient(CreatePatientRequest createPatientRequest)
        {
            PatientResponse patient = userService.CreatePatient(createPatientRequest);

            if (patient == null)
            {
                return StatusCode(400);
            }

            return Ok(patient);
        }

        [Authorize]
        [HttpGet("get-patients")]
        public IActionResult GetPatients()
        {
            return Ok(userService.GetPatients());
        }

        [Authorize]
        [HttpGet("get-examinations")]
        public IActionResult GetExaminations([FromHeader]long patientId)
        {
            return Ok(userService.GetExaminations(patientId));
        }

        [Authorize]
        [HttpPost("add-examination")]
        public IActionResult AddExamination(AddExaminationRequest addExaminationRequest)
        {
            return Ok(userService.AddExamination(addExaminationRequest));
        }

        [Authorize]
        [HttpPost("train")]
        public IActionResult Train(TrainingRequest trainingRequest)
        {
            if (!Account.IsAdmin)
            {
                return StatusCode(403, new { message = "Unauthorized" });
            }

            return Ok(userService.Train(trainingRequest));
        }

        [Authorize]
        [HttpPost("change-password")]
        public IActionResult ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            if (!userService.ChangePassword(changePasswordRequest, Account.Id))
            {
                return StatusCode(400);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("validate-token")]
        public IActionResult ValidateToken()
        {
            return Ok();
        }
    }
}