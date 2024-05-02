using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ASP.NET_Bulletin
{
    public partial class Read : System.Web.UI.Page
    {
        int headingNum = 0;
        String file = "";
        int baseIndex = 0;
        int globalTreeNum = 0;
        int lastTreeNum = 0;
        int globalHeaderNum = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                headingNum = Convert.ToInt32(Request.QueryString.Get(0));
            }
            catch
            {
                return;
            }

            file = File.ReadAllText(Context.Server.MapPath("~/ClientBin/Bulletin.txt"));

            PopulateNode(TreeView1, null);
            TreeView1.TreeNodeCheckChanged += new TreeNodeEventHandler(TreeView1_TreeNodeCheckChanged);
            TreeView1.ShowLines = true;
            TreeView1.NodeWrap = true;
            TreeView1.SelectedNodeChanged += new EventHandler(TreeView1_SelectedNodeChanged);
            TreeView1.CollapseAll();
        }

        void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            Context.Response.Redirect("http://kenfujioka.name/faq/Bulletin/ASP.NET/Popup.aspx?=" + ((TreeNode)sender).Text, false);
        }

        void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            Context.Response.Redirect("http://kenfujioka.name/faq/Bulletin/ASP.NET/Popup.aspx?=" + ((TreeNode)sender).Text, false);
        }

        void PopulateNode(TreeView sender, TreeNodeEventArgs e)
        {
            String comments = BulletinFile.GetComments(headingNum, file);
            int count = 0;
            int currentCommentNum = 0;
            String temp = "";
            int numberOfComments = 1;
            int nextCommentNum = 0;
            int currentPosition = 1;

            temp = BulletinFile.GetHeading(headingNum, file);

            temp = (headingNum.ToString()) + ":@" + temp;



            TreeNode newNode = new TreeNode();
            newNode.Text = temp;//.Replace("-@-", "\r\n");
            newNode.Value = "0";

            // Set the PopulateOnDemand property to true so that the child nodes can be 
            // dynamically populated.
            newNode.PopulateOnDemand = false;

            // Set additional properties for the node.
            newNode.SelectAction = TreeNodeSelectAction.Expand;


            // Add the new node to the ChildNodes collection of the parent node.
            sender.Nodes.Add(newNode);

            if (comments == "")
            {
                return;
            }

            currentCommentNum = Convert.ToInt32(comments.Substring(0, comments.IndexOf(":")));

            try
            {
                comments = ":@" + comments;

                int index = comments.IndexOf(":@" + (currentCommentNum.ToString()) + ":");

                if (comments.IndexOf(":@", index + 1) != -1)
                {
                    temp = comments.Substring(comments.IndexOf(":@:@", index + 1), comments.IndexOf(":", comments.IndexOf(":@:@", index + 1) + 4) - comments.IndexOf(":@:@", index + 1)).Replace(":", "");
                }
                else
                {
                    temp = comments.Substring(index + 1, comments.IndexOf(":", index + 1));
                }
                temp = temp.Replace("@", "");
                nextCommentNum = Convert.ToInt32(temp);
            }
            catch
            {
                nextCommentNum = 0;
            }


            temp = comments;

            while (temp.IndexOf(":@:@", 1) != -1)
            {
                numberOfComments++;

                try
                {
                    temp = temp.Substring(temp.IndexOf(":@:@", 2), temp.Length - temp.IndexOf(":@:@", 2));
                }
                catch
                {
                    break;
                }
            }

            while (baseIndex < numberOfComments)
            {
                if (baseIndex + 1 == numberOfComments)  //last comment
                {
                    nextCommentNum = 0;
                }
                else
                {
                }

                if (baseIndex < numberOfComments)
                {
                    TreeNode newNode2 = new TreeNode();
                    newNode2.Text = BulletinFile.GetComment(headingNum, file, currentCommentNum);//.Replace("-@-", "\r\n");
                    globalHeaderNum = currentCommentNum;
                    newNode2.Value = globalHeaderNum.ToString();

                    // Set the PopulateOnDemand property to true so that the child nodes can be 
                    // dynamically populated.
                    newNode2.PopulateOnDemand = false;

                    // Set additional properties for the node.
                    newNode2.SelectAction = TreeNodeSelectAction.Expand;


                    // Add the new node to the ChildNodes collection of the parent node.
                    sender.Nodes.Add(newNode2);

                    baseIndex++;

                    if (nextCommentNum > currentCommentNum)
                    {
                        globalTreeNum = nextCommentNum;
                        newItem_Expanded(newNode2, 1);
                    }
                }
                else
                {
                }

                currentCommentNum = globalTreeNum;

                try
                {
                    string comments2 = "";
                    string header = globalHeaderNum.ToString();
                    string temp2 = "";

                    comments2 = comments.Substring(comments.IndexOf(":@:@", comments.IndexOf(":@" + header + ":") + 1), comments.Length - comments.IndexOf(":@:@", comments.IndexOf(":@" + header + ":") + 1) - 1);  //ignore comments that are not relavant

                    if (comments2.Length <= 1 || !comments2.Contains(":@")) //end of comments
                    {
                        baseIndex--;
                        return;
                    }

                    comments2 = comments2.Replace("\r", " ");
                    temp = comments2.Substring(4, comments2.IndexOf(":", 1));
                    temp = temp.Replace(":", "");
                    temp = temp.Replace("@", "");

                    currentCommentNum = Convert.ToInt32(temp);

                    temp2 = temp;

                    comments2 = comments.Substring(comments.IndexOf(":@:@", comments.IndexOf(":@" + temp2 + ":") + 1), comments.Length - comments.IndexOf(":@:@", comments.IndexOf(":@" + temp2 + ":") + 1) - 1);  //ignore comments that are not relavant

                    if (comments2.Length <= 1 || !comments2.Contains(":@")) //end of comments
                    {
                        baseIndex--;
                        return;
                    }

                    comments2 = comments2.Replace("\r", " ");
                    temp = comments2.Substring(4, comments2.IndexOf(":", 1));
                    temp = temp.Replace(":", "");
                    temp = temp.Replace("@", "");

                    nextCommentNum = Convert.ToInt32(temp);
                }
                catch
                {
                    nextCommentNum = 0;
                }
            }
        }

        public int newItem_Expanded(TreeNode parentIndex, int depth)
        {
            String comments = BulletinFile.GetComments(headingNum, file);
            String comments2 = "";
            int count = 0;
            int currentCommentNum = 0;
            String temp = "";
            int numberOfComments = 1;
            int nextCommentNum = 0;
            String header = "";
            int headerNum = globalHeaderNum;
            int lastTreeNum2 = lastTreeNum;

            header = globalHeaderNum.ToString();

            comments2 = comments.Substring(comments.IndexOf(":@:@", comments.IndexOf(":@" + header + ":") + 1), comments.Length - comments.IndexOf(":@:@", comments.IndexOf(":@" + header + ":") + 1) - 1);  //ignore comments that are not relavant

            if (comments2.Length <= 1 || !comments2.Contains(":@")) //end of comments
            {
                baseIndex--;
                return headerNum;
            }

            comments2 = comments2.Replace("\r", " ");
            temp = comments2.Substring(4, comments2.IndexOf(":", 1));
            temp = temp.Replace(":", "");
            temp = temp.Replace("@", "");
            currentCommentNum = Convert.ToInt32(temp);

            try
            {
                int index = comments.IndexOf(":@" + (currentCommentNum.ToString()) + ":");

                if (comments.IndexOf(":@", index + 1) != -1)
                {
                    temp = comments.Substring(comments.IndexOf(":@:@", index + 1), comments.IndexOf(":", comments.IndexOf(":@:@", index + 1) + 4) - comments.IndexOf(":@:@", index + 1)).Replace(":", "");
                }
                else
                {
                    temp = comments.Substring(index + 1, comments.IndexOf(":", index + 1));
                }

                temp = temp.Replace("@", "");
                nextCommentNum = Convert.ToInt32(temp);
                lastTreeNum = nextCommentNum;
            }
            catch (Exception e)
            {
                nextCommentNum = 0;
            }

            temp = comments;

            while (temp.IndexOf(":@:@", 1) != -1)
            {
                numberOfComments++;

                try
                {
                    temp = temp.Substring(temp.IndexOf(":@:@", 2), temp.Length - temp.IndexOf(":@:@", 2));
                }
                catch
                {
                    break;
                }
            }

            do
            {
                if (baseIndex < numberOfComments)
                {
                    TreeNode newNode2 = new TreeNode();
                    newNode2.Text = BulletinFile.GetComment(headingNum, file, currentCommentNum);//.Replace("-@-", "\r\n");
                    globalHeaderNum = currentCommentNum;
                    newNode2.Value = globalHeaderNum.ToString();

                    // Set the PopulateOnDemand property to true so that the child nodes can be 
                    // dynamically populated.
                    newNode2.PopulateOnDemand = false;

                    // Set additional properties for the node.
                    newNode2.SelectAction = TreeNodeSelectAction.Expand;


                    // Add the new node to the ChildNodes collection of the parent node.
                    parentIndex.ChildNodes.Add(newNode2);


                    baseIndex++;
                    count++;

                    globalTreeNum = nextCommentNum;

                    if (nextCommentNum > currentCommentNum)
                    {
                        headerNum = currentCommentNum;

                        lastTreeNum2 = newItem_Expanded(newNode2, depth + 1);

                        if (count == 1)
                        {
                            return 0;
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    break;
                }

                if ((currentCommentNum > nextCommentNum && headerNum > nextCommentNum) || (currentCommentNum < nextCommentNum && nextCommentNum > lastTreeNum))
                {
                    return 0;
                }

                if (currentCommentNum > lastTreeNum && nextCommentNum > currentCommentNum)  //last comment
                {
                    currentCommentNum = lastTreeNum;
                }
                else
                {
                    currentCommentNum = nextCommentNum;
                }

                try
                {
                    header = currentCommentNum.ToString();

                    comments2 = comments.Substring(comments.IndexOf(":@:@", comments.IndexOf(":@" + header + ":") + 1), comments.Length - comments.IndexOf(":@:@", comments.IndexOf(":@" + header + ":") + 1) - 1);  //ignore comments that are not relavant

                    if (comments2.Length <= 1 || !comments2.Contains(":@")) //end of comments
                    {
                        baseIndex--;
                        return headerNum;
                    }

                    comments2 = comments2.Replace("\r", " ");
                    temp = comments2.Substring(4, comments2.IndexOf(":", 1));
                    temp = temp.Replace(":", "");
                    temp = temp.Replace("@", "");
                    nextCommentNum = Convert.ToInt32(temp);
                    lastTreeNum = nextCommentNum;
                }
                catch
                {
                    nextCommentNum = 0;
                }


            } while (true);

            return headerNum;
        }
    }

    public class BulletinFile
    {
        public static string CreateHeading(int headingNum, string file)
        {
            string post = "";
            string heading = "";
            int index = 0;
            int counter = 0;
            int lastIndex = 0;

            //check for unwanted calls
            if (file == "")
            {
                return " ";
            }

            //go through the file string which contains the whole file and find the post
            do
            {
                lastIndex = index;
                index = file.IndexOf(";@", lastIndex + 1);
                counter++;
            } while (headingNum != counter && lastIndex != -1 && index != -1);

            if (lastIndex == -1 || index == -1)  //last line of file is reached
            {
                return " ";
            }

            if (headingNum != 1)
            {
                post = file.Substring(lastIndex + 4, index - lastIndex - 4);
            }
            else
            {
                post = file.Substring(lastIndex, index - lastIndex);
            }

            index = 0;  //reset index

            //find each field and update the passed variables
            lastIndex = index;


            index = post.IndexOf(":@", lastIndex + 1);
            heading += post.Substring(lastIndex, index - lastIndex) + ": " + GetTitle(headingNum, file) + " Creator:";

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);
            heading += post.Substring(lastIndex, index - lastIndex) + " Date&time:";

            //            lastIndex = index;
            //            index = post.IndexOf(":@", lastIndex + 1);
            //content = post.Substring(lastIndex, index - lastIndex);

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);
            heading += post.Substring(lastIndex, index - lastIndex);

            heading = heading.Replace("\n", "");  //remove white sapce

            return heading;
        }

        public static string GetHeading(int headingNum, string file)
        {
            string post = "";
            string heading = "";
            int index = 0;
            int counter = 0;
            int lastIndex = 0;
            string temp2 = "";

            //check for unwanted calls
            if (file == "")
            {
                return "";
            }

            //go through the file string which contains the whole file and find the post
            do
            {
                lastIndex = index;
                index = file.IndexOf(";@", lastIndex + 1);
                counter++;

                temp2 = file.Substring(index + 4, 3).Replace(":", "").Replace("@", "");

            } while (headingNum != 1 && headingNum - 1 != Convert.ToInt32(temp2) && lastIndex != -1 && index != -1);

            if (headingNum == 1)
            {
                index = file.Length - 1;
                lastIndex = file.IndexOf(":@;@\r\n" + headingNum.ToString()) + 4;
            }

            if (lastIndex == -1 || index == -1)  //when last line of file is reached
            {
                return "";
            }

            if (headingNum == 1)
            {
                post = file.Substring(lastIndex + 4, index - lastIndex - 4);
            }
            else
            {
                post = file.Substring(lastIndex + 6, index - lastIndex - 6);
            }

            index = 0;  //reset index

            //find the heading in the found post and build the text 
            for (int x = 0; x < 3; x++)  // 
            {
                lastIndex = index;
                index = post.IndexOf(":@", lastIndex + 1);
                heading += post.Substring(lastIndex, index - lastIndex);
            }

            return heading;
        }

        public static string GetPost(int headingNum, string file)
        {
            string post = "";
            int index = 0;
            int counter = 0;
            int lastIndex = 0;

            //check for unwanted calls
            if (file == "")
            {
                return "";
            }

            //go through the file string which contains the whole file and find the post
            do
            {
                lastIndex = index;
                index = file.IndexOf(";@", lastIndex + 1);
                counter++;
            } while (headingNum != counter && lastIndex != -1 && index != -1);

            if (lastIndex == -1 || index == -1)  //when last line of file is reached
            {
                return "";
            }

            post = file.Substring(lastIndex + 4, index - lastIndex - 1);

            return post.Replace("\n", "");
        }


        public static string GetContent(int headingNum, string file)
        {
            string post = "";
            string temp = "";
            string temp2 = "";
            string content = "";
            string comment = "";
            //           string commentHeading = "";
            int index = 0;
            int counter = 0;
            int lastIndex = 0;
            //            int tempIndex = 0;

            //go through the file string which contains the whole file and find the post
            do
            {
                lastIndex = index;
                index = file.IndexOf(";@", lastIndex + 1);
                counter++;
            } while (headingNum != counter && lastIndex != -1 && index != -1);

            if (lastIndex == -1 || index == -1)  //last line of file is reached
            {
                return "";
            }

            if (headingNum != 1)  //first line will not have newline chars in front of the line unlike all other lines
            {
                post = file.Substring(lastIndex + 4, index - lastIndex - 4);
            }
            else
            {
                post = file.Substring(lastIndex, index - lastIndex);
            }

            index = 0;  //reset index

            //find the content in the found post
            for (int x = 0; x < 4; x++)  // 
            {
                lastIndex = index;
                index = post.IndexOf(":@", lastIndex + 1);
            }

            temp = post.Substring(lastIndex, index - lastIndex);
            /*
                        //wrap lines
                        while (temp.Length - 41 > tempIndex)
                        {
                            tempIndex += 41;
                            temp = temp.Insert(tempIndex, "\r\n");
                            tempIndex += 4;
                        }
            */
            content = temp;

            //skip date field
            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            //add delimiter
            post += ";@";

            while (post.IndexOf(":@;@") != index)
            {
                //               tempIndex = 0;
                lastIndex = index;
                index = post.IndexOf(":@", lastIndex + 1);

                temp2 = post.Substring(lastIndex, index - lastIndex);
                /*
                                //wrap lines
                                while (temp2.Length - 41 > tempIndex)
                                {
                                    tempIndex += 41;
                                    temp2 = temp2.Insert(tempIndex, "\r\n");
                                    tempIndex += 4;
                                }
                */
                comment += temp2 + "\r\n\r\n";

            }


            return content.Replace("-@-", "\r") + "\r\n------------------------------------\r\n\r\n" + comment.Replace("-@-", "\r");
        }

        public static string GetComments(int headingNum, string file)
        {
            string post = "";
            //            string temp = "";
            string temp2 = "";
            //            string content = "";
            string comment = "";
            //           string commentHeading = "";
            int index = 0;
            int counter = 0;
            int lastIndex = 0;
            //            int tempIndex = 0;

            //go through the file string which contains the whole file and find the post
            do
            {
                lastIndex = index;
                index = file.IndexOf(";@", lastIndex + 1);
                counter++;

                temp2 = file.Substring(index + 4, 3).Replace(":", "").Replace("@", "");

            } while (headingNum != 1 && headingNum - 1 != Convert.ToInt32(temp2) && lastIndex != -1 && index != -1);
			
            if (headingNum == 1)
            {
			try
			{
                index = file.Length - 1;
                lastIndex = file.IndexOf(":@;@\r\n" + headingNum.ToString()) + 4;
            }
			catch
			{
            	post = file.Substring(0, file.IndexOf(":@;@\r\n") + 8);
 			}
			}		
			
            if (lastIndex == -1 || index == -1)  //last line of file is reached
            {
                return "";
            }

            if (headingNum != 1)  //first line will not have newline chars in front of the line unlike all other lines
            {
                post = file.Substring(lastIndex, index - lastIndex);
            }
            else if(post == "")
            {
                post = file.Substring(lastIndex, index - lastIndex);
            }
            

            index = 0;  //reset index

            //find the content in the found post
            for (int x = 0; x < 4; x++)  // 
            {
                lastIndex = index;
                index = post.IndexOf(":@", lastIndex + 1);
            }
            /*
                        temp = post.Substring(lastIndex, index - lastIndex);

                        //wrap lines
                        while (temp.Length - 41 > tempIndex)
                        {
                            tempIndex += 41;
                            temp = temp.Insert(tempIndex, "\r\n");
                            tempIndex += 4;
                        }

                        content = temp;
            */
            //skip date field
            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            //add delimiter
            post += ";@";

            while (post.IndexOf(":@;@") != index)
            {
                //                tempIndex = 0;
                lastIndex = index;
                index = post.IndexOf(":@", lastIndex + 1);

                temp2 = post.Substring(lastIndex, index - lastIndex);
                /*
                                //wrap lines
                                while (temp2.Length - 41 > tempIndex)
                                {
                                    tempIndex += 41;
                                    //temp2 = temp2.Insert(tempIndex, "\r\n");
                                    tempIndex += 4;
                                }
                                */
                comment += temp2 + ":@";// +"\r\n\r\n";

            }

            if (comment.Length != 0)
            {
                comment = comment.Remove(0, 2);
            }

            return comment.Replace("-@-", "\r");
        }

        public static string GetComment(int headingNum, string file, int commentNum)
        {
            string comments = "";
            string[] commentArray = new string[64];

            comments = BulletinFile.GetComments(headingNum, file) + ";@";


            //go through the comments and extract each comment and put in array
            for (int x = 0; comments.IndexOf(";@") != 0 && x < 255 && comments.Length != 0; x++)
            {
                commentArray[x] = comments.Substring(0, comments.IndexOf(":@"));
                comments = comments.Remove(0, comments.IndexOf(":@") + 4);

                if (commentNum == Convert.ToInt32(commentArray[x].Substring(0, commentArray[x].IndexOf(":"))))  //each comment will have a unique number 1-64
                {
                    return commentArray[x].Replace("-@-", "\r");
                }
            }

            return "COMMENT_NOT_FOUND";
        }

        public static string GetName(int headingNum, string file)
        {
            string post = "";
            int index = 0;
            int counter = 0;
            int lastIndex = 0;

            //go through the file string which contains the whole file and find the post
            do
            {
                lastIndex = index;
                index = file.IndexOf(";@", lastIndex + 1);
                counter++;
            } while (headingNum != counter && lastIndex != -1 && index != -1);

            if (lastIndex == -1 || index == -1)  //last line of file is reached
            {
                return " ";
            }

            if (headingNum != 1)
            {
                post = file.Substring(lastIndex + 4, index - lastIndex - 4);
            }
            else
            {
                post = file.Substring(lastIndex, index - lastIndex);
            }


            index = 0;  //reset index

            //find each field and update the passed variables
            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);
            //            title = post.Substring(lastIndex, index - lastIndex);

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            return (post.Substring(lastIndex, index - lastIndex));

            //            lastIndex = index;
            //            index = post.IndexOf(":@", lastIndex + 1);
            //            content = post.Substring(lastIndex, index - lastIndex);

            //            lastIndex = index;
            //            index = post.IndexOf(":@", lastIndex + 1);
            //            time = post.Substring(lastIndex, index - lastIndex);
        }

        public static string GetTitle(int headingNum, string file)
        {
            string post = "";
            int index = 0;
            int counter = 0;
            int lastIndex = 0;

            //go through the file string which contains the whole file and find the post
            do
            {
                lastIndex = index;
                index = file.IndexOf(";@", lastIndex + 1);
                counter++;
            } while (headingNum != counter && lastIndex != -1 && index != -1);

            if (lastIndex == -1 || index == -1)  //last line of file is reached
            {
                return " ";
            }

            if (headingNum != 1)
            {
                post = file.Substring(lastIndex + 4, index - lastIndex - 4);
            }
            else
            {
                post = file.Substring(lastIndex, index - lastIndex);
            }

            index = 0;  //reset index

            //find each field and update the passed variables
            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            return (post.Substring(lastIndex, index - lastIndex));

            //            lastIndex = index;
            //            index = post.IndexOf(":@", lastIndex + 1);
            //            return (post.Substring(lastIndex, index - lastIndex));

            //            lastIndex = index;
            //            index = post.IndexOf(":@", lastIndex + 1);
            //            content = post.Substring(lastIndex, index - lastIndex);

            //            lastIndex = index;
            //            index = post.IndexOf(":@", lastIndex + 1);
            //            time = post.Substring(lastIndex, index - lastIndex);

        }

        public static string GetTime(int headingNum, string file)
        {
            string post = "";
            int index = 0;
            int counter = 0;
            int lastIndex = 0;

            //go through the file string which contains the whole file and find the post
            do
            {
                lastIndex = index;
                index = file.IndexOf(";@", lastIndex + 1);
                counter++;
            } while (headingNum != counter && lastIndex != -1 && index != -1);

            if (lastIndex == -1 || index == -1)  //last line of file is reached
            {
                return " ";
            }

            if (headingNum != 1)
            {
                post = file.Substring(lastIndex + 4, index - lastIndex - 4);
            }
            else
            {
                post = file.Substring(lastIndex, index - lastIndex);
            }


            index = 0;  //reset index

            //find each field and update the passed variables
            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);
            //            title = post.Substring(lastIndex, index - lastIndex);

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);
            //            return (post.Substring(lastIndex, index - lastIndex));

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);
            //            content = post.Substring(lastIndex, index - lastIndex);

            lastIndex = index;
            index = post.IndexOf(":@", lastIndex + 1);

            return (post.Substring(lastIndex, index - lastIndex));
        }
        /*
                /// <summary>
                /// Adds a new post to the bulletin file on the server by uploading the
                /// post string to the web handler.  After completing the upload, redownload
                /// the file and update the screen with the added post.
                /// </summary>
                /// <param name="title">Title of the new post</param>
                /// <param name="name">Name of the new post</param>
                /// <param name="content">Content of the new post</param>
                /// <param name="bulletin">The main class binding to the screen</param>
                /// <param name="file">The always updated file string which contains the whole bulletin file</param>
                /// <returns>true</returns>
                public static bool AddPost(string title, string name, string content, Bulletin bulletin, string file)
                {
                    UriBuilder ub = new UriBuilder("http://kenfujioka.name/faq/Bulletin/BulletinWriteHandler.ashx");
                    WebClient c = new WebClient();

                    c.UploadStringCompleted += (sender, e) =>
                    {
                        //re-initialize main view
                        UriBuilder ub2 = new UriBuilder("http://kenfujioka.name/faq/Bulletin/BulletinHandler.ashx");
                        WebClient c2 = new WebClient();

                        c2.UploadStringCompleted += (sender2, e2) =>
                        {
                            file = String.Copy(e2.Result);
                            bulletin.Heading1 = BulletinFile.CreateHeading(1, file);
                            bulletin.Heading2 = BulletinFile.CreateHeading(2, file);
                            bulletin.Heading3 = BulletinFile.CreateHeading(3, file);
                            bulletin.Heading4 = BulletinFile.CreateHeading(4, file);
                            bulletin.Heading5 = BulletinFile.CreateHeading(5, file);
                            bulletin.Heading6 = BulletinFile.CreateHeading(6, file);
                            bulletin.Heading7 = BulletinFile.CreateHeading(7, file);
                            bulletin.Heading8 = BulletinFile.CreateHeading(8, file);
                            bulletin.Heading9 = BulletinFile.CreateHeading(9, file);
                            bulletin.Heading10 = BulletinFile.CreateHeading(10, file);
                            bulletin.Heading11 = BulletinFile.CreateHeading(11, file);
                            bulletin.Heading12 = BulletinFile.CreateHeading(12, file);
                            bulletin.Heading13 = BulletinFile.CreateHeading(13, file);
                            bulletin.Heading14 = BulletinFile.CreateHeading(14, file);
                            bulletin.Heading15 = BulletinFile.CreateHeading(15, file);
                            bulletin.Heading16 = BulletinFile.CreateHeading(16, file);
                            bulletin.Heading17 = BulletinFile.CreateHeading(17, file);
                        };
                        c2.UploadStringAsync(ub2.Uri, "");
                    };
                    c.UploadStringAsync(ub.Uri, title + ":@" + name + ":@" + content + ":@" + DateTime.Now + ":@" + ";@");

                    return true;
                }
        */

        public static bool IsCommentHack(string time, string content, string file)
        {
            int index = 0;
            int index2 = 0;
            string commentInQuestion = "";

            //Check if there is an another comment with same timestamp.  Check all comments with same timestamp.
            while (file.Contains(time))
            {
                index = file.IndexOf(time);  //locate the post in question
                index = file.IndexOf(" ---- ", index);
                index2 = file.IndexOf(":@", index);
                commentInQuestion = file.Substring(index + 6, index2 - index - 6);

                //See if content (first ten characters or less) is also the same.  If same, it is an almost a complete duplicate.
                if (commentInQuestion.Contains(content))
                {
                    return true;  //is a hack
                }

                file.Remove(index, 10);  //remove the timestamp so it will not find it again
            }

            return false;
        }

        public static bool IsPostHack(string time, string content, string file)
        {
            int index = 0;
            int index2 = 0;
            string commentInQuestion = "";

            //Check if there is an another comment with same timestamp.  Check all comments with same timestamp.
            while (file.Contains(time))
            {
                index = file.IndexOf(":@", index);  //skip name, title fields
                index = file.IndexOf(":@", index + 1);
                index = file.IndexOf(":@", index + 1);
                index2 = file.IndexOf(":@", index + 1);
                commentInQuestion = file.Substring(index + 2, index2 - index - 2);

                //See if content (first ten characters or less) is also the same.  If same, it is an almost a complete duplicate.
                if (commentInQuestion.Contains(content))
                {
                    return true;  //is a hack
                }

                file.Remove(index, 10);  //remove the timestamp so it will not find it again
            }

            return false;
        }

    }
}