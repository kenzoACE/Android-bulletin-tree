<%@ WebHandler Language="C#" Class="BulletinHandler" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

public class BulletinHandler : IHttpHandler
{

        public void ProcessRequest(HttpContext context)
        {
            StreamReader sr2 = File.OpenText(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"));

            //send back the Bulletin file
            context.Response.Write(sr2.ReadToEnd());
            sr2.Close();
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

