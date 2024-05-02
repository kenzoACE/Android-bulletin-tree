<%@ WebHandler Language="C#" Class="CommentDeleteHandler" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


public class CommentDeleteHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string comment = "";
        string comments = "";
        string[] fileLines = new string[256];
        string temp = "";
        int temp2 = 0;
        int index = 0;
        int lastIndex = 0;
        int postNumber = 0;  //The heading number. 1 through 255
        int commentNum = 0;
        int length = 0;
        string[] commentArray = new string[256];  //initialized to contain null strings

        //read the Bulletin post data from request
        StreamReader sr = new StreamReader(context.Request.InputStream);

        comment = sr.ReadToEnd();
        sr.Close();

        //the first number appearing on the uploaded string contains the post number of the comment
        postNumber = Convert.ToInt32(comment.Substring(0, comment.IndexOf(":@")));

        //the second number contains the number of the comment to be deleted
        length = comment.IndexOf(";@") - comment.IndexOf(":@") - 2;
        temp = comment.Substring(comment.IndexOf(":@") + 2, length);
        commentNum = Convert.ToInt32(temp);

        //lock the data file so changes would not be made to the Bulletin data file while applying changes
        FileStream dataFS = File.OpenRead(context.Server.MapPath("~/ClientBin/" + "BulletinData.txt"));
        dataFS.Lock(0, dataFS.Length);
        
        //read all posts from Bulletin file
        fileLines = File.ReadAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"));

        //after fifth field is where comments section begin
        for (int x = 0; x < 5; x++)
        {
            lastIndex = index;
            index = fileLines[postNumber - 1].IndexOf(":@", lastIndex + 1);
        }

        //retrieve the comments section
        comments = fileLines[postNumber - 1].Substring(index + 2, fileLines[postNumber - 1].Length - index - 2);

        //go through the comments and extract each comment and put in array
        for (int x = 0; comments.IndexOf(";@") != 0 && x < 255; x++)
        {
            commentArray[x] = comments.Substring(0, comments.IndexOf(":@"));
            comments = comments.Remove(0, comments.IndexOf(":@") + 2);

//            commentNum = Convert.ToInt32(commentArray[x].Substring(0, commentArray[x].IndexOf(":")));  //each comment will have a unique number 1-255
        }

        //find the comment
        for (int x = 0; commentArray[x] != null && x < 255 && commentNum > 0; x++)
        {
            if (Convert.ToInt32(commentArray[x].Substring(0, commentArray[x].IndexOf(":"))) == commentNum)  //found the comment
            {
                temp2 = commentArray[x].IndexOf(" ---- ") + 6;
                //remove the comment
                fileLines[postNumber - 1] = fileLines[postNumber - 1].Remove(index + temp2 + 2, commentArray[x].Length - temp2);

                File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"), fileLines);
                break;
            }
            
            lastIndex = index;
            index += commentArray[x].Length + 2;
            
        }

        //remove the comment
//        fileLines[postNumber - 1] = fileLines[postNumber - 1].Remove(lastIndex, index - lastIndex);

//        File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"), fileLines);

        dataFS.Unlock(0, dataFS.Length);
        dataFS.Close();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}