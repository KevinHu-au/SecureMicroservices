using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movies.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class IdentityController : ControllerBase
  {
    [HttpGet]
    public IActionResult Get()
    {
      return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
    }
  }
}
