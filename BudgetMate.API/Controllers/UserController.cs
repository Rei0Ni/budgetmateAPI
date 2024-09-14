using BudgetMate.Application.DTO.User;
using BudgetMate.Application.Interfaces.Wallet;
using BudgetMate.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BudgetMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        private readonly IConfiguration _configuration;
        private readonly IWalletRepository _walletRepository;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IWalletRepository walletRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _configuration = configuration;
            _walletRepository = walletRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> CreateUser(NewUserDto user)
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
            {
                await userManager.AddToRoleAsync(appUser, "App_User");
                _walletRepository.AddWallet(appUser.Id.ToString());
                Log.Information(appUser!.UserName!);
                return Ok(new { Result = "User created successfully" });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if (!ModelState.IsValid)
                return Unauthorized(new { Result = "Invalid Username or Password" });
            ApplicationUser? appUser = await userManager.FindByNameAsync(user.Username!);


            if (appUser != null && await userManager.CheckPasswordAsync(appUser, user.Password!))
            {
                var result = await signInManager.PasswordSignInAsync(appUser.UserName!,
                           user.Password!, true, false);

                return Ok(new { Result = "Login Successful" });
            }

            return Unauthorized(new { Result = "Invalid Username or Password" });
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            if (signInManager.IsSignedIn(HttpContext.User))
            {
                signInManager.SignOutAsync();
            }
            return Ok();
        }
    }
}
