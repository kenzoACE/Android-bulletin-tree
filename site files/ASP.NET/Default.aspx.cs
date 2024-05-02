using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;

namespace ASP.NET_Bulletin
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ListBox1.SelectedIndex == -1)
            {
                String[] fileLines = new String[256];

                fileLines = File.ReadAllLines(Context.Server.MapPath("~/ClientBin/Bulletin.txt"));

                ArrayList values = new ArrayList();

				values.Add("");

                for (int x = 0; x < 256 && !String.IsNullOrEmpty(fileLines[x]); x++)
                {
                    int index2 = 0;
                    int index = 0;
                    String temp = "";

                    index2 = fileLines[x].IndexOf(":@");
                    index2 = fileLines[x].IndexOf(":@", index2 + 1);
                    index2 = fileLines[x].IndexOf(":@", index2 + 1);

                    temp = fileLines[x].Substring(0, index2);

                    index2 = fileLines[x].IndexOf(":@", index2 + 1);
                    index = index2;

                    index2 = fileLines[x].IndexOf(":@", index2 + 1);

                    values.Add(temp + fileLines[x].Substring(index, index2 - index));
                }

                ListBox1.DataSource = values;
                ListBox1.DataBind();
                //                ListBox1.SelectedIndexChanged += new EventHandler(ListBox1_SelectedIndexChanged);
            }
            else
            {
            	if(ListBox1.SelectedValue != "")
            	{
                	Context.Response.Redirect("http://kenfujioka.name/faq/Bulletin/ASP.NET/Read.aspx?=" + ListBox1.SelectedValue.Substring(0, ListBox1.SelectedValue.IndexOf(":")));
                	//Context.Response.Redirect("Read.aspx?=" + ListBox1.SelectedValue.Substring(0, ListBox1.SelectedValue.IndexOf(":")));
            	}
            }
        }
    }
}