using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;  


namespace Web
{
    public partial class xl : System.Web.UI.Page
    {
        const string cn_str = @"Server=ASUS-PROMAX\HIEP_SQLEXPRESS;Database=SVKMT;User Id=sa;Password=123;";
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            switch (action)
            {
                case "login":
                    login();
                    break;
                case "check_login":
                    check_login();
                    break;
                case "dangxuat":
                    logout();
                    break;
                case "Doi_mk":
                    doimk();
                    break;
                case "lichsu":
                    lich_su();
                    break;
            }
        }
        class SV
        {
            public string msv;
            private string pw;
            public void setPW(string pw) { this.pw = pw; }
            private string get_field_string(DataRow r, string field)
            {
                try
                {
                    return (string)r[field];
                }
                catch
                {
                    return "";
                }
            }
        }
          
        
        
        class DataLogin
        {
            public bool ok;
            public string msg;
            public SV sv;
            public List<lichsu> lichsu;
        }
        class lichsu
        {
            public string noidung;
        }
        string get(DataRow r, string field)
        {
            try
            {
                return (string)r[field];
            }
            catch
            {
                return "";
            }

        }
        void login()
        {

            DataLogin L = new DataLogin();
            SqlConnection cn = new SqlConnection(cn_str);
                cn.Open(); 
            try 
        {
            string msv = Request["msv"];
            string pw = Request["pw"];
            string captcha = Request["cap"];
            string dapan = (string)Session["dap_an"];
            if (captcha == dapan)
            {
             
                SqlCommand cm = new SqlCommand("SP_login", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.Add("@action", SqlDbType.NVarChar, 50).Value = "login";
                cm.Parameters.Add("@msv", SqlDbType.NVarChar, 50).Value = msv;
                cm.Parameters.Add("@pw", SqlDbType.NVarChar, 50).Value = pw;
                SqlDataReader dr = cm.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                if (dt != null && dt.Rows.Count == 1)
                {

                    SV sv = new SV();
                    DataRow r = dt.Rows[0];
                    sv.msv = get(r, "msv");
                    L.sv = sv;

                    L.ok = true;
                    L.msg = "Đăng nhập thành công";

                }
                else
                {
                    L.ok = false;
                    L.msg = "sai thông tin đăng nhập hoặc mật khẩu";

                }
            }
            else
            {
                L.msg = "sai capcha";
            }
                   
                }
                catch(Exception ex)
                {
                    L.ok = false;
                    L.msg = ex.Message;
                }
            SqlCommand cmd = new SqlCommand("INSERT INTO lich_su (NoiDung)VALUES (@noidung);", cn);
            cmd.Parameters.Add("@noidung", SqlDbType.NVarChar, 50).Value = "đã đăng nhập ";
            cmd.ExecuteNonQuery();
            Session["ok"] = L.ok;
                string json = JsonConvert.SerializeObject(L);
                Session["json_ok"] = json;
                Response.Write(json);
        }
        void check_login()
        {
            DataLogin L = new DataLogin();
            string json;
            try
            {
                L.ok = (bool)Session["ok"];
                if (L.ok)
                {
                    json = (string)Session["json_ok"];
                    Response.Write(json);
                    return;
                }
                else
                {
                    L.msg = "thất bại";
                }
            }
            catch
            {
                L.ok = false;
                L.msg = "chưa login";
            }
            json = JsonConvert.SerializeObject(L);
            Response.Write(json);
        }

        void logout()
        {
            DataLogin L = new DataLogin();
            if (Session["ok"] != null)
            {
                Session["ok"] = null;
                Session["json_ok"] = null;
                L.ok = true;
                L.msg = "Đã đăng xuất thành công ! ";
            }
            else
            {

                L.ok = false;
                L.msg = "Lỗi rồi ";
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(L);
            Response.Write(json);

        }
        void doimk()
        {

            string pwc = Request.Form["pwc"];
            string pw = Request.Form["pw"];
            SqlConnection cn = new SqlConnection(cn_str);
            cn.Open();
            DataLogin L = new DataLogin();
            try
            {
                SqlCommand cm = new SqlCommand("UPDATE SV SET pw =@pw  WHERE pw=@pwc", cn);
                cm.Parameters.Add("@pwc", SqlDbType.NVarChar, 50).Value = pwc;
                cm.Parameters.Add("@pw", SqlDbType.NVarChar, 50).Value = pw;
                cm.ExecuteNonQuery();
                L.ok = true;
                L.msg = "Đã đổi mật khẩu thành công !";

            }
            catch
            {
                L.ok = false;
                L.msg = " Đổi mật khẩu không thành công !";
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO LichSu (NoiDung)VALUES (@noidung);", cn);
            cmd.Parameters.Add("@noidung", SqlDbType.NVarChar, 50).Value = "đã đổi mật khẩu ";
            cmd.ExecuteNonQuery();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(L);
            Session["json_ok"] = json; 
            Response.Write(json);

        }
        void lich_su(){
        DataLogin L = new DataLogin();
            SqlConnection cn = new SqlConnection(cn_str);
            cn.Open();
            SqlCommand cm = new SqlCommand("select * from lich_su ", cn);     
            SqlDataReader dr = cm.ExecuteReader();
            DataTable dt = new DataTable();
          
            dt.Load(dr);
            if (dt.Rows.Count > 0)
            {
                
                L.ok = true;
              
                L.lichsu = new List<lichsu>();
                foreach (DataRow r in dt.Rows)
                {
                    lichsu ls = new lichsu();
                    ls.noidung = r["NoiDung"].ToString();
                    L.lichsu.Add(ls);
                }
            }
            else
            {
                L.ok = false;

            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(L);
            Response.Write(json);

        }
    }
}