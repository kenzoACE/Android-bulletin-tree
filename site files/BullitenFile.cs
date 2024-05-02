using System;


namespace Bulliten
{
    public class BullitenFile
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
            heading += post.Substring(lastIndex, index - lastIndex) + "Date&time:";
            
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

/*            //wrap lines
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
//                tempIndex = 0;
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

            
            return content.Replace("-@-","\r") + "\r\n------------------------------------\r\n\r\n" + comment.Replace("-@-","\r\n");
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
 //               tempIndex = 0;
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
            
            comments = BullitenFile.GetComments(headingNum, file) + ";@"; 


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
        /// Adds a new post to the bulliten file on the server by uploading the
        /// post string to the web handler.  After completing the upload, redownload
        /// the file and update the screen with the added post.
        /// </summary>
        /// <param name="title">Title of the new post</param>
        /// <param name="name">Name of the new post</param>
        /// <param name="content">Content of the new post</param>
        /// <param name="bulliten">The main class binding to the screen</param>
        /// <param name="file">The always updated file string which contains the whole bulliten file</param>
        /// <returns>true</returns>
        public static bool AddPost(string title, string name, string content, Bulliten bulliten, string file)
        {
            UriBuilder ub = new UriBuilder("http://fushigispace.com/faq/Bulliten/BullitenWriteHandler.ashx");
            WebClient c = new WebClient();

            c.UploadStringCompleted += (sender, e) =>
            {
                //re-initialize main view
                UriBuilder ub2 = new UriBuilder("http://fushigispace.com/faq/Bulliten/BullitenHandler.ashx");
                WebClient c2 = new WebClient();

                c2.UploadStringCompleted += (sender2, e2) =>
                {
                    file = String.Copy(e2.Result);
                    bulliten.Heading1 = BullitenFile.CreateHeading(1, file);
                    bulliten.Heading2 = BullitenFile.CreateHeading(2, file);
                    bulliten.Heading3 = BullitenFile.CreateHeading(3, file);
                    bulliten.Heading4 = BullitenFile.CreateHeading(4, file);
                    bulliten.Heading5 = BullitenFile.CreateHeading(5, file);
                    bulliten.Heading6 = BullitenFile.CreateHeading(6, file);
                    bulliten.Heading7 = BullitenFile.CreateHeading(7, file);
                    bulliten.Heading8 = BullitenFile.CreateHeading(8, file);
                    bulliten.Heading9 = BullitenFile.CreateHeading(9, file);
                    bulliten.Heading10 = BullitenFile.CreateHeading(10, file);
                    bulliten.Heading11 = BullitenFile.CreateHeading(11, file);
                    bulliten.Heading12 = BullitenFile.CreateHeading(12, file);
                    bulliten.Heading13 = BullitenFile.CreateHeading(13, file);
                    bulliten.Heading14 = BullitenFile.CreateHeading(14, file);
                    bulliten.Heading15 = BullitenFile.CreateHeading(15, file);
                    bulliten.Heading16 = BullitenFile.CreateHeading(16, file);
                    bulliten.Heading17 = BullitenFile.CreateHeading(17, file);
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

            //Check if there is an another comment with same timestamp.  Check all comments with the same timestamp.
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
            string postInQuestion = "";

            //Check if there is an another comment with same timestamp.  Check all comments with the same timestamp.
            while (file.Contains(time))
            {
                index = file.IndexOf(":@", index);  //skip name, title fields
                index = file.IndexOf(":@", index + 1);
                index = file.IndexOf(":@", index + 1);
                index2 = file.IndexOf(":@", index + 1);
                postInQuestion = file.Substring(index + 2, index2 - index - 2);

                //See if content (first ten characters or less) is also the same.  If same, it is an almost a complete duplicate.
                if (postInQuestion.Contains(content))
                {
                    return true;  //is a hack
                }

                file.Remove(index, 10);  //remove the timestamp so it will not find it again
            }

            return false;
        }

    }
}
