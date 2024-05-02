<%@ WebHandler Language="C#" Class="PostDeleteHandler" %>

using System;
using System.IO;
using System.Web;

public class PostDeleteHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string uploadedString = "";
        string[] fileLines = new string[256];
        int index = 0;
        int lastIndex = 0;
        int postNumber = 0;  //The heading number. 1 through 255

        //read the Bulletin post data from request
        StreamReader sr = new StreamReader(context.Request.InputStream);

        uploadedString = sr.ReadToEnd();
        sr.Close();

        //the first number appearing on the uploaded string contains the post number of the comment
        postNumber = Convert.ToInt32(uploadedString);

        //lock the data file so changes would not be made to the Bulletin data file while applying changes
        FileStream dataFS = File.OpenRead(context.Server.MapPath("~/ClientBin/" + "BulletinData.txt"));
        dataFS.Lock(0, dataFS.Length);

        //read all posts from Bulletin file
        fileLines = File.ReadAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"));

        //forth field is the content
        for (int x = 0; x < 4; x++)
        {
            lastIndex = index;
            index = fileLines[postNumber - 1].IndexOf(":@", lastIndex + 1);
        }

        fileLines[postNumber - 1] = fileLines[postNumber - 1].Remove(lastIndex + 2, index - lastIndex - 2);

        File.WriteAllLines(context.Server.MapPath("~/ClientBin/" + "Bulletin.txt"), fileLines);

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
    