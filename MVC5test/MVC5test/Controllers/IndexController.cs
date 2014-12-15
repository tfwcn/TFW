using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using Common;
using System.Data.SqlClient;

namespace MVC5test.Controllers
{
    public class IndexController : BaseController
    {
        private DAL.PostgreHelper bllPG = new DAL.PostgreHelper();
        //
        // GET: /Index/
        public ActionResult Index()
        {
            string skuvalues = "07736FC6-CE1A-431D-9412-79E8785FEFE1,86BA99E2-F519-42CA-8009-055285C03FC8,4D8CE66E-4E47-4083-9222-7DCE1C4F59A2,7B83628D-93DF-432B-B054-87FB1B40CB1F,ABE18D86-4747-4B1A-B5D2-1A0886FBB88C";
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(skuvalues);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, true);
            gzip.Write(bt,0,bt.Length);
            gzip.Close();
            byte[] bt2 = ms.ToArray();
            string minvalues = Convert.ToBase64String(bt2);
            byte[] bt3 = new Guid("07736FC6-CE1A-431D-9412-79E8785FEFE1").ToByteArray();
            string minvalues2 = Convert.ToBase64String(bt3);
            string strguid = "07736FC6-CE1A-431D-9412-79E8785FEFE1".Replace("-","");
            ulong minvalues2_1 = UInt64.Parse(strguid.Substring(0, 16), System.Globalization.NumberStyles.AllowHexSpecifier);
            ulong minvalues2_2 = UInt64.Parse(strguid.Substring(16), System.Globalization.NumberStyles.AllowHexSpecifier);
            for (int i = 0; i < 100000; i++)
            {
                Guid g = Guid.NewGuid();
                string minvalues3_1 = GUIDHelper.GetGUIDNo(g);
                string minvalues3_2 = GUIDHelper.GetGUID(minvalues3_1).ToString();
                if (minvalues3_2 != g.ToString())
                {
                    throw new Exception("不等於");
                }
            }
            return View();
        }

        public ActionResult Hello(string msg)
        {
            string retStr = "";
            retStr += String.Format("Hello,{0}", msg);
            return Content(retStr);
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

        public ActionResult GetJSON(int? page, int? rows, string TSEQ)
        {
            Dictionary<string, object> jsonObj = new Dictionary<string, object>();
            List<Models.TUSERInfo> tmpListTUSER = new List<Models.TUSERInfo>();
            System.Data.Common.DbDataReader dr = null;
            try
            {
                string whereStr = "";
                if (!TSEQ.IsVoid())
                {
                    whereStr += " and \"TSEQ\"=" + TSEQ + " ";
                }
                dr = bllPG.GetDataReader("select * from \"TUSER\" where 1=1 " + whereStr + " limit " + rows + " offset " + (page - 1) * rows, null);
                while (dr.Read())
                {
                    Models.TUSERInfo tmpTUSER = new Models.TUSERInfo();
                    tmpTUSER.TSEQ = Convert.ToInt32(dr["TSEQ"]);
                    tmpTUSER.TNAME = dr["TNAME"].ToString();
                    tmpListTUSER.Add(tmpTUSER);
                }
                int rowCount = Convert.ToInt32(bllPG.GetObject("select count(0) from \"TUSER\" where 1=1 " + whereStr, null));
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

        public ActionResult GetAttributes()
        {
            Dictionary<string,int> attributes=new Dictionary<string,int>();
            attributes.Add("n1", 0);
            attributes.Add("n2", 1);
            attributes.Add("n3", 2);
            return View(new MVC5test.Models.Attribute() { data=attributes });
        }

        public ActionResult AttributesSubmit()
        {
            string vals = "";
            vals += Request["PA_n1"] + ",";
            vals += Request["PA_n2"] + ",";
            vals += Request["PA_n3"] + ",";
            vals += Request["SV_規格1"] + ",";
            List<SqlParameter> paras = new List<SqlParameter>();
            string test = string.Join(",", ValuesToWhere("p1", new string[] { "aa", "ss", "dd" }, paras));
            return Content(vals);
        }

        //public string ValuesToWhere(string pname,string [] values, List<SqlParameter> paras)
        //{
        //    string retstr = "";
        //    for(int i=0;i<values.Length;i++)
        //    {
        //        SqlParameter p = new SqlParameter("@"+pname+"_"+i, System.Data.SqlDbType.VarChar, 30) { Value = values[i] };
        //        paras.Add(p);
        //        retstr += "@" + pname + "_" + i + ",";
        //    }
        //    if (retstr.Length > 0)
        //    {
        //        retstr = retstr.Substring(0, retstr.Length - 1);
        //    }
        //    return retstr;
        //}

        public string[] ValuesToWhere(string pname, string[] values, List<SqlParameter> paras)
        {
            List<string> retstr = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                SqlParameter p = new SqlParameter("@" + pname + "_" + i, System.Data.SqlDbType.VarChar, 30) { Value = values[i] };
                paras.Add(p);
                retstr.Add("@" + pname + "_" + i);
            }
            return retstr.ToArray();
        }
    }
}