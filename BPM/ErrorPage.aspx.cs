using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPM.BusinessEntities;

public partial class ErrorPage : System.Web.UI.Page
{
    public static string MessageArchivePath = "";
    public static string OagisArchivePath = "";
    public static string BODArchivePath = "";
    public static string EDIXMLArchivePath = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["type"].ToString() == "ContactUS")
        {
            string ErrorMessage = Session["ContactUs"].ToString();

            lblErrorMessage.Text = ErrorMessage;
            divError.Visible = true;
        }
    }
}
