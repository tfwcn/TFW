﻿using System;
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
            List<string> retstr = new List<string>(); ;
            for (int i = 0; i < values.Length; i++)
            {
                SqlParameter p = new SqlParameter("@" + pname + "_" + i, System.Data.SqlDbType.VarChar, 30) { Value = values[i] };
                paras.Add(p);
                retstr.Add("@" + pname + "_" + i);
            }
            return retstr.ToArray();
        }

        public void GetA()
        {
            
        }
        /*
         產品分類屬性-sku值設置
         產品屬性
         產品sku屬性
產品圖片	tpicture							
序号	名称	数据类型	长度	小数位	允许空值	主键	默认值	说明
1	fid 	char(36)	36	0	N	Y		主鍵id
2	fproductid	char(36)	36					產品id
3	fname	nvarchar(100)	100					原始文件名稱
4	fisthumbnail	bit						是否縮略圖
5	findex	int						圖片順序
6	fismain	bit						是否主圖
產品屬性	tproductattributes							
序号	名称	数据类型	长度	小数位	允许空值	主键	默认值	说明
1	fid 	char(36)	36	0	N	Y		主鍵id
2	fproductid	char(36)	36					產品id
3	fattributesid	char(36)	36					屬性id
4	fvalues	nvarchar（100）	100					屬性值

產品sku屬性	tproductsku							
序号	名称	数据类型	长度	小数位	允许空值	主键	默认值	说明
1	fid 	char(36)	36	0	N	Y		主鍵id
2	fproductid	char(36)	36					產品id
3	tproductskuvalueid	char(36)	36					sku值id

產品sku值表	tproductskuvalue							
序号	名称	数据类型	长度	小数位	允许空值	主键	默认值	说明
1	fid 	char(36)	36	0	N	Y		主鍵id
2	fattributesid	char(36)	36					屬性id
3	fvalue	nvarchar(100)	100					屬性值
4	ftext	nvarchar(100)	100					屬性值顯示名稱
10	faccount	char(36)	36					賬套

         
         */
    }
}