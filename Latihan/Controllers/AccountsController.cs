using Latihan.Helpers;
using Latihan.Models;
using Latihan.Repositories;
using Latihan.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Latihan.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AccountsController : ControllerBase
    {
        private AccountRepositories _accountRepositories;
        private EducationRepositories _educationRepositories;
        private IConfiguration _configuration;

        public AccountsController(AccountRepositories accountRepositories, EducationRepositories educationRepositories, IConfiguration configuration)
        {
            _accountRepositories = accountRepositories;
            _educationRepositories = educationRepositories;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginVM loginVM)
        {
            if (String.IsNullOrWhiteSpace(loginVM.EmailOrNik))
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Email atau NIK tidak boleh kosong"));
            } else if (String.IsNullOrWhiteSpace(loginVM.Password)) {
                return BadRequest(ApiResponse.CreateResponse(400, "Password tidak boleh kosong"));
            }

            var loginData = _accountRepositories.Login(loginVM);

            if (loginData == null)
            {
                return NotFound(ApiResponse.CreateResponse(400, "Email atau NIK tidak terdaftar"));
            }

            if (!Hashing.ValidatePassword(loginVM.Password, loginData.Password))
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Password salah"));
            }

            var claims = new List<Claim>
            {
                new Claim("Username", $"{loginData.Employee.FirstName}.{loginData.Employee.LastName}"),
                new Claim("NIK", $"{loginData.Employee.NIK}")
            };

            var accountRoles = _accountRepositories.GetAccountRole(loginData.NIK);

            foreach (var accountRole in accountRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, accountRole.RoleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]) );
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:API"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signIn
                );
            var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

            var data = new
            {
                //FullName = $"{loginData.Employee.FirstName} {loginData.Employee.LastName}",
                Token = tokenResult
            };

            return Ok(ApiResponse.CreateResponse(200, "Login berhasil", data));

        }

        [HttpPost("Register")]
        [Authorize(Roles = "Admin")]
        public IActionResult Register(RegisterVM registerVM)
        {
            if (registerVM.GPA < 0 || registerVM.GPA > 4)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "GPA harus 0-4"));
            }

            var register = _accountRepositories.Register(registerVM);

            if (register == AccountRepositories.EMAILDUPLICATE)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Email sudah terdaftar"));
            } else if (register == AccountRepositories.PHONEDUPLICATE)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Phone sudah terdaftar"));
            } else if (register <= 0)
            {
                return BadRequest(ApiResponse.CreateResponse(400, "Terjadi kesalahan"));
            }

            return Ok(ApiResponse.CreateResponse(201, "Data berhasil ditambahkan", registerVM));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetAllAccounts()
        {
            var accounts = _accountRepositories.GetAllAccounts();

            if (accounts.Count() == 0)
            {
                return NotFound(ApiResponse.CreateResponse(404, "Data tidak ditemukan"));
            }
            return Ok(ApiResponse.CreateResponse(200, $"{accounts.Count()} data berhasil didapatkan", accounts));
        }

        [HttpGet("{nik}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetAccountById(string nik)
        {
            var account = _accountRepositories.GetAccountById(nik);
            if (account == null)
            {
                return BadRequest(ApiResponse.CreateResponse(400, $"Data {nik} tidak ditemukan"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data {nik} berhasil ditemukan", account));
        }

        [HttpPut("{nik}")]
        [Authorize(Roles = "Employee")]
        public IActionResult UpdateAccount(string nik, RegisterVM account)
        {
            var updatedAccount = _accountRepositories.UpdateAccount(account);

            if (updatedAccount == AccountRepositories.NOTFOUND)
            {
                return NotFound(ApiResponse.CreateResponse(404, $"Data {nik} tidak ditemukan"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data {nik} berhasil diupdate", account));
        }

        [HttpGet("Education")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult GetEducation()
        {
            var accounts = _educationRepositories.GetTotalDegree();

            return Ok(ApiResponse.CreateResponse(200, $"{accounts.Count()} data berhasil didapatkan", accounts));
        }

        [HttpPut("EditPassword/{nik}")]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult UpdatePassword(string nik, EditPasswordVM editPassword)
        {
            if (editPassword.NewPassword != editPassword.NewPasswordConfirmation)
            {
                return NotFound(ApiResponse.CreateResponse(400, $"Konfirmasi password tidak sama"));
            }

            var updatedAccount = _accountRepositories.UpdatePassword(editPassword);

            if (updatedAccount == AccountRepositories.NOTFOUND)
            {
                return NotFound(ApiResponse.CreateResponse(404, $"Data {nik} tidak ditemukan"));
            }

            if (updatedAccount == AccountRepositories.PASSWORDNOTMATCH)
            {
                return NotFound(ApiResponse.CreateResponse(400, $"Password lama salah"));
            }

            return Ok(ApiResponse.CreateResponse(200, $"Data password {nik} berhasil diupdate"));
        }


    }
}
