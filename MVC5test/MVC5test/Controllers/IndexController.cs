using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace MVC5test.Controllers
{
    public class IndexController : Controller
    {
        private DAL.PostgreHelper bllPG = new DAL.PostgreHelper();
        //
        // GET: /Index/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Hello(string msg)
        {
            string retStr = "";
            retStr += String.Format("{0}</br>", msg);
            System.Data.Common.DbDataReader dr = null;
            try
            {
                dr = bllPG.GetDataReader("select * from \"TUSER\"", null);
                while (dr.Read())
                {
                    retStr += String.Format("{0} {1}</br>", dr["TSEQ"], dr["TNAME"]);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
            return Content(retStr);//test1
        }

        public ActionResult Submit(string Pass)
        {
            string msg = "False";
            try
            {
                if (Pass == "123")
                {
                    msg = "True";
                    string TNAME = bllPG.GetObject("select max(\"TNAME\") from \"TUSER\"", null) as string;
                    int num = 1;
                    if (!TNAME.IsVoid())
                        num = Convert.ToInt32(TNAME.Substring(1)) + 1;
                    for (int i = num; i < num + 10; i++)
                    {
                        bllPG.RunSQL("INSERT INTO \"TUSER\"(\"TNAME\")VALUES ('A" + i + "')", null);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Redirect("~/Index/Hello?msg=" + msg);
        }
	}
}