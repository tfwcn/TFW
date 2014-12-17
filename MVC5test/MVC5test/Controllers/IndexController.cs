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
            Page();
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

        public ActionResult QueryData(string kw,string c,string skus)
        {
            List<string> listC = new List<string>();
            List<string> listSkus = new List<string>();
            Dictionary<string, string> listskuselect = new Dictionary<string, string>();
            listskuselect.Keys.ToList().Exists(m => m == "");
            listskuselect.Add("", "");
            //查詢
            //根據查詢條件獲取產品類別條件
            //根據查詢條件獲取sku
            //生成當前位置URL
            //生成產品類別URL
            //生成skuURL
            //

            return View();
        }

        public string Page()
        {
            //up 1 ... 3 4 5 6 7 ... 10 down
            //up 1 2 3 4 5 6 ... 10 down
            //up 1 ... 5 6 7 8 9 10 down
            int now = 2;
            int pagecount = 10;
            int pageshow = 5;
            int pagestart = 0;
            int pageend = 0;
            string up, down;
            string s1, s2;
            if (now > 1)
                up = (now - 1).ToString();
            else
                up = "";
            if (now < pagecount)
                down = (now + 1).ToString();
            else
                down = "";

            if (now - Math.Ceiling(pageshow / 2d) <= 1)
            {
                pagestart = 2;
            }
            else if (now + Math.Ceiling(pageshow / 2d) >= pagecount)
            {
                pagestart = pagecount - pageshow;
            }
            else
            {
                pagestart = Convert.ToInt32(now - Math.Floor(pageshow / 2d));
            }
            pageend = pagestart + pageshow;
            if (pageend > pagecount)
                pageend = pagecount;
            if (pagestart > 2)
                s1 = "...";
            else
                s1 = "";
            if (pageend < pagecount)
                s2 = "...";
            else
                s2 = "";
            string mid = "";
            for (int i = pagestart; i < pageend; i++)
            {
                mid += " " + i + " ";
            }
            string all = String.Format("{4} 1 {0}{1}{2} {3} {5}", s1, mid, s2, pagecount <= 1 ? "" : pagecount.ToString(), up, down);
            return all;
        }
    }
}