using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using Common;

namespace MVC5test.Controllers
{
    public class IndexController : BaseController
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
                dr = bllPG.GetDataReader("select * from \"TUSER\" ", null);
                while (dr.Read())
                {
                    retStr += String.Format("{0} {1}</br>", dr["TSEQ"], dr["TNAME"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            //try
            //{
                if (Pass == "123")
                {
                    msg = "True";
                    string TNAME = bllPG.GetObject("select \"TNAME\" from \"TUSER\" order by \"TSEQ\" desc limit 1", null) as string;
                    int num = 1;
                    if (!TNAME.IsVoid())
                        num = Convert.ToInt32(TNAME.Substring(1)) + 1;
                    for (int i = num; i < num + 10; i++)
                    {
                        bllPG.RunSQL("INSERT INTO \"TUSER\"(\"TNAME\")VALUES ('A" + i + "')", null);
                    }
                }
            //    throw new Exception("EEEEE1");
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return Redirect("~/Index/Hello?msg=" + msg);
        }

        public ActionResult GetJSON(int? page, int? rows)
        {
            Dictionary<string, object> jsonObj = new Dictionary<string, object>();
            List<Models.TUSERInfo> tmpListTUSER = new List<Models.TUSERInfo>();
            System.Data.Common.DbDataReader dr = null;
            try
            {
                dr = bllPG.GetDataReader("select * from \"TUSER\" limit " + rows + " offset " + (page-1) * rows, null);
                while (dr.Read())
                {
                    Models.TUSERInfo tmpTUSER = new Models.TUSERInfo();
                    tmpTUSER.TSEQ = Convert.ToInt32(dr["TSEQ"]);
                    tmpTUSER.TNAME = dr["TNAME"].ToString();
                    tmpListTUSER.Add(tmpTUSER);
                }
                int rowCount = Convert.ToInt32(bllPG.GetObject("select count(0) from \"TUSER\"", null));
                jsonObj.Add("total", rowCount);
                jsonObj.Add("rows", tmpListTUSER);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
            return Json(jsonObj);
        }
	}
}