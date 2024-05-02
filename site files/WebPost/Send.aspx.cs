using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Threading;


public partial class Send : System.Web.UI.Page
{

    bool IS_LIVE_LOGIN = false;
    string SecureName = ""; 

    protected void Page_Load(object sender, EventArgs e)
    {
        string post = "";
        ushort numberOfPosts = 0;
        byte[] buffer = new byte[1];  //number of posts represented in one byte
        string[] fileLines = new string[256];
        string[] tempFileLines = new string[256];
        string temp = "";
        int index = 0;
        string temp1 = Request.Form.Get("TextBox1");
        string temp2 = Request.Form.Get("TextBox2");
        string temp3 = Request.Form.Get("TextBox3");

        //replace all delimiters and create a newline delimiter (do not change the order of replacement)
        temp1 = temp1.Replace(":@", "");
        temp1 = temp1.Replace(";@", "");
        temp1 = temp1.Replace(";@;", "");
        temp1 = temp1.Replace("\r\n", "");
        temp1 = temp1.Replace(" ", "_");
        temp1 = temp1.Replace("  ", "_");
        temp1 = temp1.Replace("!", "");
        temp2 = temp2.Replace(":@", "");
        temp2 = temp2.Replace(";@", "");
        temp2 = temp2.Replace(";@;", "");
        temp2 = temp2.Replace("-@-", "");
        temp2 = temp2.Replace("\r\n", "");
        temp2 = temp2.Replace("\r", "-@-");  //newline delimiter
        temp3 = temp3.Replace(":@", "");
        temp3 = temp3.Replace(";@", "");
        temp3 = temp3.Replace(";@;", "");
        temp3 = temp3.Replace("\r\n", "");

//        WebPost.ServiceReference1.WebServiceLiveSoapClient webserviceMgrSoapClient = new WebPost.ServiceReference1.WebServiceLiveSoapClient();
//        webserviceMgrSoapClient.HelloWorldCompleted += new EventHandler<WebPost.ServiceReference1.HelloWorldCompletedEventArgs>(webserviceMgrSoapClient_HelloWorldCompleted);
//        webserviceMgrSoapClient.HelloWorldAsync();


//        while (!IS_LIVE_LOGIN && String.IsNullOrEmpty(SecureName) && index > 50)
//        {
//            Thread.Sleep(100);
//            index++;
//        }

        if (IS_LIVE_LOGIN)
        {
            temp1 = SecureName;
        }

        //retrieve text submitted by form
        post += temp3 + ":@";
        post += temp1 + ":@";
        post += temp2 + ":@" + DateTime.Now.ToString() + ":@" + ";@";

        //open file which contains the data about how many posts there are
        Stream dataSR = File.Open(Server.MapPath("~/ClientBin/" + "BulletinData.txt"), FileMode.Open, FileAccess.ReadWrite);

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
        fileLines = File.ReadAllLines(Server.MapPath("~/ClientBin/" + "Bulletin.txt"));

        if (numberOfPosts == 256)   //split the file in half number of posts and save to a unique file as log
        {
            temp = DateTime.Now.ToString();
            temp = temp.Replace('/', '_');
            temp = temp.Replace(' ', '_');
            temp = temp.Replace(':', '_');

            File.WriteAllLines(Server.MapPath("~/ClientBin/" + temp + "_BulletinLog.txt"), fileLines);

            tempFileLines[0] = "1" + ":@" + post;

            for (int x = 1; x < 128; x++)
            {
                tempFileLines[x] = fileLines[x - 1];
            }

            File.WriteAllLines(Server.MapPath("~/ClientBin/" + "Bulletin.txt"), tempFileLines);

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

            File.WriteAllLines(Server.MapPath("~/ClientBin/" + "Bulletin.txt"), tempFileLines);

            //update the data file and close
            buffer[0] = Convert.ToByte(numberOfPosts);
        }

        dataSR.Write(buffer, 0, 1);
        dataSR.Close();
    }
/*
    void webserviceMgrSoapClient_HelloWorldCompleted(object sender, WebPost.ServiceReference1.HelloWorldCompletedEventArgs e)
    {
        if (e.Error == null && e.Result != "" && e.Result != null)
        {

            UriBuilder ub = new UriBuilder("http://kenfujioka.name/faq/Bulletin/LiveGetName.ashx");
            WebClient wc = new WebClient();
            UriBuilder ub2 = new UriBuilder("http://kenfujioka.name/faq/Bulletin/LiveGetID.ashx");
            WebClient wc2 = new WebClient();

            wc2.UploadStringCompleted += (sender3, e3) =>
            {
                if (e3.Result != null && e3.Error == null && e3.Result != "")
                {
                    ID = String.Copy(e3.Result);

                    wc.UploadStringCompleted += (sender2, e2) =>
                    {
                        if (e2.Result != null && e2.Error == null && e2.Result != "")
                        {

                            SecureName = "!" + String.Copy(e2.Result) + "(" + ID.Substring(28, 4) + ")";     //last four chars of live id for this site

                            IS_LIVE_LOGIN = true;
                        }
                    };
                    wc.UploadStringAsync(ub.Uri, e.Result);
                }
            };
            wc2.UploadStringAsync(ub2.Uri, e.Result);
        }
        else
        {
            //                name = "";
        }
    }*/
}
