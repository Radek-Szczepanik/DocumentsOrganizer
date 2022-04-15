using DocumentsOrganizer.Models;
using DocumentsOrganizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsOrganizer.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            accountService.ReqisterUser(dto);
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = accountService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
