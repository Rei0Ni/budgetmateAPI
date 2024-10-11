using System.Security.Claims;
using BudgetMate.Application.DTO.User;
using BudgetMate.Application.Interfaces.User;
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
        private readonly IUserService _userService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IWalletRepository walletRepository,
            IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _configuration = configuration;
            _walletRepository = walletRepository;
            _userService = userService;
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
        [Authorize]
        [Route("Profile")]
        public async Task<ActionResult> GetProfile()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var userProfile = await _userService.GetUserProfileAsync(userId);
            return Ok(userProfile);
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
