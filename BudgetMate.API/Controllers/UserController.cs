using BudgetMate.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        private readonly IConfiguration _configuration;
        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(NewUserDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApplicationUser appUser = new()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            IdentityResult result = await userManager.CreateAsync(appUser, user.Password!);
            if (result.Succeeded)
                return Ok(new { Result = "User created successfully" });
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            if (!ModelState.IsValid)
                return Unauthorized(new {Result = "Invalid Username or Password"});
            ApplicationUser? appUser = await userManager.FindByNameAsync(user.Username!);


            if (appUser != null && await userManager.CheckPasswordAsync(appUser, user.Password!))
            {
                var result = await signInManager.PasswordSignInAsync(appUser.UserName!,
                           user.Password!, false, false);
                
                return Ok(new {Result = "Login Successful"});
            }

            return Unauthorized(new {Result = "Invalid Username or Password"});
        }
    }
}
