using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5test.Controllers
{
    public class HelpController : Controller
    {
        //
        // GET: /Help/
        public ActionResult Error(string msg)
        {
            ViewBag.msg = msg;
            return View();
        }
	}
}