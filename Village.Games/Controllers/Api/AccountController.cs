using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Village.Games.Data;
using Village.Games.Helpers;
using Village.Games.Models;
using Village.Games.ViewModels;

namespace Village.Games.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly TodoContext _todoContext;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IMapper mapper, TodoContext todoContext, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _todoContext = todoContext;
            _userManager = userManager;
        }

        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);


            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            //await _todoContext.ApplicationUsers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
            await _todoContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }
}