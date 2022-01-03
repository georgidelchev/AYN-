using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers;

[Authorize]
public class BaseController : Controller
{
}
