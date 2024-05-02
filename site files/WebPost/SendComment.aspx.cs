using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using System.Net;


public partial class SendComment : System.Web.UI.Page
{
    protected bool COMMENT_NUMBER_ERROR = false;
    bool IS_LIVE_LOGIN = false;
    string SecureName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        const int MAX_COMMENT_NUM = 64;
        string comments = "";
        string[] fileLines = new string[256];
        string[] fileLines2 = new string[256];
        string[] tempFileLines = new string[256];
        string commentToInsert = "";
        string temp = "";
        //        string fileLine = "";
        int index = 0;
        //        int index2 = 0;
        int lastIndex = 0;
        int postNumber = 0;  //The heading number. 1 through 255
        int commentNum = 0;
        int positionNum = 0;
        int NumberOfComments = 0;
        //            int length = 0;
        string[] commentArray = new string[256];  //initialized to contain null strings
        ushort numberOfPosts = 0;
        byte[] buffer = new byte[1];  //number of posts represented in one byte


//        WebPost.ServiceReference1.WebServiceLiveSoapClient webserviceMgrSoapClient = new WebPost.ServiceReference1.WebServiceLiveSoapClient();
//        webserviceMgrSoapClient.HelloWorldCompleted += new EventHandler<WebPost.ServiceReference1.HelloWorldCompletedEventArgs>(webserviceMgrSoapClient_HelloWorldCompleted);
//        webserviceMgrSoapClient.HelloWorldAsync();

//        while (!IS_LIVE_LOGIN && String.IsNullOrEmpty(SecureName) && index > 50)
//        {
//            Thread.Sleep(100);
//            index++;
//        }

        string temp1 = Request.Form.Get("TextBox1");
        string temp2 = Request.Form.Get("TextBox2");
        string temp3 = Request.Form.Get("TextBox3");
        string temp4 = Request.Form.Get("TextBox4");


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

        if (IS_LIVE_LOGIN)
        {
            temp1 = SecureName;
        }

        try
        {
            //the first number appearing on the uploaded string contains the post number of the comment
            postNumber = Convert.ToInt32(temp3);

            //the second number contains the position which the comment will be inserted
            positionNum = Convert.ToInt32(temp4);
        }
        catch
        {
            COMMENT_NUMBER_ERROR = true;
            return;
        }

        //parse the uploaded comment string
        commentToInsert = " Name:" + temp1 + " " + DateTime.Now.ToString() + " Date&time: " + temp2;

        //open file which contains the data about how many posts there are
        Stream dataSR = File.Open(Server.MapPath("~/ClientBin/" + "BulletinData.txt"), FileMode.Open, FileAccess.ReadWrite);

        //read all posts from Bulletin file
        fileLines = File.ReadAllLines(Server.MapPath("~/ClientBin/" + "Bulletin.txt"));

		int numPosts = Convert.ToInt32(dataSR.ReadByte());

		postNumber = numPosts - postNumber + 1;

        try
        {
            //after fifth field is where comments section begin
            for (int x = 0; x < 5; x++)
            {
                lastIndex = index;
                index = fileLines[postNumber - 1].IndexOf(":@", lastIndex + 1);
            }

            //retrieve the comments section
            comments = fileLines[postNumber - 1].Substring(index + 2, fileLines[postNumber - 1].Length - index - 2);
        }
        catch
        {
            COMMENT_NUMBER_ERROR = true;
            dataSR.Close();
            return;
        }


        //go through the comments and extract each comment and put in array
        for (int x = 0; comments.IndexOf(";@") != 0 && x < 255; x++)
        {
            commentArray[x] = comments.Substring(0, comments.IndexOf(":@"));
            comments = comments.Remove(0, comments.IndexOf(":@") + 2);

            commentNum = Convert.ToInt32(commentArray[x].Substring(0, commentArray[x].IndexOf(":")));  //each comment will have a unique number 1-255

            NumberOfComments++;
        }

        //If there are too many comments, clone the post and begin a new post.  Max number of comments is limited to 64.
        if (NumberOfComments == MAX_COMMENT_NUM)
        {
            if (dataSR.Length == 0)  //should never be true
            {
                buffer[0] = 0;
                dataSR.Write(buffer, 0, 1);
                dataSR.Seek(0, SeekOrigin.Begin);
            }

            dataSR.Read(buffer, 0, 1);
            dataSR.Seek(0, SeekOrigin.Begin);
            numberOfPosts = Convert.ToUInt16(buffer[0] + 1);

            //write the new post
            fileLines2 = File.ReadAllLines(Server.MapPath("~/ClientBin/" + "Bulletin.txt"));

            if (numberOfPosts == 256)   //split the file in half number of posts and save to a unique file as log
            {
                temp = DateTime.Now.ToString();
                temp = temp.Replace('/', '_');
                temp = temp.Replace(' ', '_');
                temp = temp.Replace(':', '_');

                File.WriteAllLines(Server.MapPath("~/ClientBin/" + temp + "_BulletinLog.txt"), fileLines);

                tempFileLines[0] = fileLines[postNumber - 1].Substring(0, index + 2) + ";@";  //post without comments 

                for (int x = 1; x < 128; x++)
                {
                    tempFileLines[x] = fileLines[x - 1];
                }

                //                if (postNumber >= 128)
                //                {
                //tag the cloned post as cloned
                //                    tempFileLines[postNumber] = tempFileLines[postNumber].Replace(";@", ";@;");
                //                }

                File.WriteAllLines(Server.MapPath("~/ClientBin/" + "Bulletin.txt"), tempFileLines);

                //update the data file
                buffer[0] = Convert.ToByte(128);
            }
            else
            {
                //                if (numberOfPosts != 1)  //already incrimented for handling the post.
                //                {
                //read previouse line number and incriment by one
                //                    index2 = fileLines2[0].IndexOf(":@");
                //                    temp = fileLines2[0].Substring(0, index2);
                //                    tempFileLines[0] = fileLines[postNumber - 1].Substring(0, index + 2) + ";@";  //post without comments
                //                }
                //                else
                //                {
                tempFileLines[0] = fileLines[postNumber - 1].Substring(0, index + 2) + ";@";  //post without comments
                //                }

                for (int x = 1; x < numberOfPosts; x++)
                {
                    tempFileLines[x] = fileLines2[x - 1];
                }

                //tag the cloned post as cloned
                tempFileLines[postNumber] = tempFileLines[postNumber].Replace(";@", ";@;");

                File.WriteAllLines(Server.MapPath("~/ClientBin/" + "Bulletin.txt"), tempFileLines);

                //update the data file and close
                buffer[0] = Convert.ToByte(numberOfPosts);
            }


            NumberOfComments = 0;       //since there are no comments on the new post
            commentArray[0] = null;     //set to null since the post is cloned and starting on a new post without any previouse comments

            fileLines = File.ReadAllLines(Server.MapPath("~/ClientBin/" + "Bulletin.txt"));  //re-read the updated file with cloned post

            postNumber = 1;             //the post with the new comment will be at the very top, so update
            //            cloned = true;              //to add the cloned delimiter ";@;"

            dataSR.Write(buffer, 0, 1);
        }

        try
        {
            //find insertion point
            for (int x = 0; commentArray[x] != null && x < 255 && positionNum > 0; x++)
            {
                if (Convert.ToInt32(commentArray[x].Substring(0, commentArray[x].IndexOf(":"))) == positionNum)  //found the comment
                {
                    index += commentArray[x].Length + 2;
                    break;
                }

                index += commentArray[x].Length + 2;
            }

            NumberOfComments++;  //update for new comment

            //insert the comment in the appropriate position
            fileLines[postNumber - 1] = fileLines[postNumber - 1].Insert(index, ":@" + NumberOfComments.ToString() + ":" + commentToInsert);
        }
        catch
        {
            COMMENT_NUMBER_ERROR = true;
            dataSR.Close();
            return;
        }

        File.WriteAllLines(Server.MapPath("~/ClientBin/" + "Bulletin.txt"), fileLines);

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
