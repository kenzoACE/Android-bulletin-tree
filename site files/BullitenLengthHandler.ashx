<%@ WebHandler Language="C#" Class="BulletinLengthHandler" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

public class BulletinLengthHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ushort numberOfPosts = 0;
            byte[] buffer = new byte[1];  //number of posts represented in one byte

            //open file which contains the data about how many posts there are
            Stream dataSR = File.Open(context.Server.MapPath("~/ClientBin/" + "BulletinData.txt"), FileMode.Open, FileAccess.Read);

            if (dataSR.Length == 0)  //create a byte file
            {
                numberOfPosts = 0;
            }
            else
            {
                dataSR.Read(buffer, 0, 1);
                numberOfPosts = Convert.ToUInt16(buffer[0]);
            }
            dataSR.Close();

            context.Response.ContentType = "text/plain";
            context.Response.Write(Convert.ToString(numberOfPosts));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }