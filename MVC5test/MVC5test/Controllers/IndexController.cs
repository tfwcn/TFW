using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5test.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Hello()
        {
            return Content("Hello");
        }
	}
}