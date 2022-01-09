using AYN.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Areas.Administration.Controllers;

[Area("Administration")]
[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
public class AdministrationController : Controller
{
}
