﻿using Microsoft.AspNetCore.Mvc;
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
                return Unauthorized(new { message = "Unauthorized" });
            }

            if (userService.CreateDoctor(createDoctorRequest) == null)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("create-patient")]
        public IActionResult CreatePatient(CreatePatientRequest createPatientRequest)
        {
            if (userService.CreatePatient(createPatientRequest) == null)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("get-patients")]
        public IActionResult GetPatients()
        {
            return Ok(userService.GetPatients());
        }

        [Authorize]
        [HttpGet("get-examinations")]
        public IActionResult GetExaminations(long patientId)
        {
            return Ok(userService.GetExaminations(patientId));
        }

        [Authorize]
        [HttpPost("add-examination")]
        public IActionResult AddExamination(AddExaminationRequest addExaminationRequest)
        {
            userService.AddExamination(addExaminationRequest);

            return Ok();
        }

        [Authorize]
        [HttpPost("train")]
        public IActionResult Train()
        {

            return Ok();
        }

        [Authorize]
        [HttpPost("check-diabets")]
        public IActionResult CheckDiabets()
        {

            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public IActionResult ChangePassword()
        {

            return Ok();
        }
    }
}
