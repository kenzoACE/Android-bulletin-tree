<%@ WebHandler Language="C#" Class="BulletinCommentHandler" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

public class BulletinCommentHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
//        string post = "";
        const int MAX_COMMENT_NUM = 64;
        string comment = "";
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
        int length = 0;
        string[] commentArray = new string[256];  //initialized to contain null strings
        ushort numberOfPosts = 0;
        byte[] buffer = new byte[1];  //number of posts represented in one byte
//        bool cloned = false;
//        int clonedPostNum = 0;

        
        
        //read the Bulletin post data from request
        StreamReader sr = new StreamReader(context.Request.InputStream);

        comment = sr.ReadToEnd();
        sr.Close();

        //the first number appearing on the uploaded string contains the post number of the comment
        postNumber = Convert.ToInt32(comment.Substring(0, comment.IndexOf(":@")));

        //the second number contains the position which the comment will be inserted
        length = comment.IndexOf(";@") - comment.IndexOf(":@") - 2;
        temp = comment.Substring(comment.IndexOf(":@") + 2, length);
        positionNum = Convert.ToInt32(temp);

        //parse the uploaded comment string
        commentToInsert = comment.Substring(comment.IndexOf(";@") + 2, comment.Length - comment.IndexOf(";@") - 2);

        //open file which contains the data about how many posts there are
        Stream dataSR = File.Open(context.Server.MapPath("~/ClientBin/" + "BulletinData.txt"), FileMode.Open, FileAccess.ReadWrite);

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
        for(int x = 0; comments.IndexOf(";@") != 0 && x < 255; x++)
        {
            commentArray[x] = comments.Substring(0, comments.IndexOf(":@"));
            comments = comments.Remove(0, comments.IndexOf(":@") + 2);

            commentNum = Convert.ToInt32(commentArray[x].Substring(0, commentArray[x].IndexOf(":")));  //each comment will have a unique number 1-255

            NumberOfComments++;
        }

        //If there are too many comments, clone the post and begin a new post.  Max number of comments are limited to 64.
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
            fileLines2 = File.ReadAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"));

            if (numberOfPosts == 256)   //split the file in half number of posts and save to a unique file as log
            {
                temp = DateTime.Now.ToString();
                temp = temp.Replace('/', '_');
                temp = temp.Replace(' ', '_');
                temp = temp.Replace(':', '_');

                File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + temp + "_BulletinLog.txt"), fileLines);

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
                
                File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"), tempFileLines);
               
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
                
                File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"), tempFileLines);

                //update the data file and close
                buffer[0] = Convert.ToByte(numberOfPosts);
            }

      
            NumberOfComments = 0;       //since there are no comments on the new post
            commentArray[0] = null;     //set to null since the post is cloned and starting on a new post without any previouse comments

            fileLines = File.ReadAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"));  //re-read the updated file with cloned post
                        
            postNumber = 1;             //the post with the new comment will be at the very top, so update
//            cloned = true;              //to add the cloned delimiter ";@;"
            
            dataSR.Write(buffer, 0, 1);
        }

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
        
        File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"), fileLines);
 
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
