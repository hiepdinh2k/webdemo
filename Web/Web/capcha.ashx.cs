using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.SessionState;
namespace Web
{
    /// <summary>
    /// Summary description for capcha
    /// </summary>
    public class capcha : IHttpHandler, IRequiresSessionState
    {
        string gen_random()
        {
            return string.Format("{0}", r.Next(100000, 999999));
        }
        Random r = new Random(DateTime.Now.Second * 123);
        int get_random_int(int d, int t)
        {

            return r.Next(d, t);
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/png";
            string dap_an = gen_random();       //sinh ngẫu nhiên
            context.Session["dap_an"] = dap_an;
            Bitmap myBitmap = new Bitmap(@"E:\CODE\visual studio\vs 2013\Web\Web\captcha.png");
            Graphics g = Graphics.FromImage(myBitmap);   //ảnh nền
            Font font = new Font("tahoma", 12, FontStyle.Regular);
            Brush brush = new SolidBrush(Color.Red);
            g.DrawString(dap_an, font, brush, 50, 25);
            Pen myPen = new Pen(Color.Red);
            //biến dạng ảnh: làm khó khẩu xử lý nhận dạng text trong ảnh

            //làm nhiễu đơn giản
            for (int i = 1; i <= 8; i++)
            {
                g.DrawLine(myPen, get_random_int(1, 100), get_random_int(1, 50), get_random_int(100, 200), get_random_int(50, 96));
            }
            //đẩy về client là cái ảnh
            using (MemoryStream ms = new MemoryStream())
            {
                myBitmap.Save(ms, ImageFormat.Png);
                byte[] result = ms.ToArray();
                context.Response.BinaryWrite(result);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}