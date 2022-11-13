using DiabetsAPI.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiabetsAPI.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        public Doctor Account => (Doctor)HttpContext.Items["Account"];
    }
}
