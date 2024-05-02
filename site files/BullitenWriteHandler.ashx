<%@ WebHandler Language="C#" Class="BulletinWriteHandler" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

    public class BulletinWriteHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string post = "";
            ushort numberOfPosts = 0;
            byte[] buffer = new byte[1];  //number of posts represented in one byte
            string[] fileLines = new string[256];
            string[] tempFileLines = new string[256];
            string temp = "";
            int index = 0;
            
            //read the Bulletin post data from request
            StreamReader sr = new StreamReader(context.Request.InputStream);

            post = sr.ReadToEnd();
            sr.Close();

            //open file which contains the data about how many posts there are
            Stream dataSR = File.Open(context.Server.MapPath("~/ClientBin/" + "BulletinData.txt"), FileMode.Open, FileAccess.ReadWrite);

            if (dataSR.Length == 0)  //create a byte file
            {
                buffer[0] = 0;
                dataSR.Write(buffer, 0, 1);
                dataSR.Seek(0, SeekOrigin.Begin);
            }

            dataSR.Read(buffer, 0, 1);
            dataSR.Seek(0, SeekOrigin.Begin);
            numberOfPosts = Convert.ToUInt16(buffer[0] + 1);
            
            //write the new post
            fileLines = File.ReadAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"));

            if (numberOfPosts == 256)   //split the file in half number of posts and save to a unique file as log
            {
                temp = DateTime.Now.ToString();
                temp = temp.Replace('/', '_');
                temp = temp.Replace(' ', '_');
                temp = temp.Replace(':', '_');
                
                File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + temp + "_BulletinLog.txt"), fileLines);

                tempFileLines[0] = "1" + ":@" + post;

                for (int x = 1; x < 128; x++)
                {
                    tempFileLines[x] = fileLines[x - 1];
                }

                File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"), tempFileLines);

                //update the data file and close
                buffer[0] = Convert.ToByte(128);
            }
            else
            {
                if (numberOfPosts != 1)  //already incrimented for handling the post
                {
                    //read previouse line number and incriment by one
                    index = fileLines[0].IndexOf(":@");
                    temp = fileLines[0].Substring(0, index);
                    tempFileLines[0] = Convert.ToString(Convert.ToInt16(temp) + 1) + ":@" + post;
                }
                else
                {
                    tempFileLines[0] = "1" + ":@" + post;
                }
                
                for (int x = 1; x < numberOfPosts; x++)
                {
                    tempFileLines[x] = fileLines[x - 1];
                }

                File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"), tempFileLines);

                //update the data file and close
                buffer[0] = Convert.ToByte(numberOfPosts);
            }
                
            dataSR.Write(buffer, 0, 1);
            dataSR.Close();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


