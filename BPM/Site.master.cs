using System;
using System.Collections.Generic;
//using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPM.BusinessEntities;
using BPM.BusinessComponents;

public partial class SiteMaster : MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    protected void Page_Init(object sender, EventArgs e)
    {
        // The code below helps to protect against XSRF attacks
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        String activepage = Request.RawUrl;
        if (activepage.Contains("Search"))            
            pageSearch.Attributes.Add("class", "active");
        //else if (activepage.Contains("Main"))
        //    pageTxnSearch.Attributes.Add("class", "active");
        else if (activepage.Contains("PO"))
            pagePO.Attributes.Add("class", "active");
        else if (activepage.Contains("ASN"))
            pageASN.Attributes.Add("class", "active");
        else if (activepage.Contains("GR"))
            pageGR.Attributes.Add("class", "active");
        else if (activepage.Contains("Transformations"))
            page841.Attributes.Add("class", "active");
        //else if (activepage.Contains("ShowShipDESADV"))
        //    pagessdesadv.Attributes.Add("class", "active");
        else if (activepage.Contains("ShowShipment"))
            pageShowShip.Attributes.Add("class", "active");
        else if (activepage.Contains("DO"))
            pageDO.Attributes.Add("class", "active");
        else if (activepage.Contains("Query"))
            pageQuery.Attributes.Add("class", "active");
        else
            pageSearch.Attributes.Add("class", "active");
        if (!IsPostBack)
        {
            drdEnvironment.SelectedIndex = 1;
            Session["Environment"] = "PROD";
        }


    }

    protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Context.GetOwinContext().Authentication.SignOut();
    }
    protected void drdEnvironment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(drdEnvironment.SelectedIndex == 0)
            Session["Environment"] = "UAT";
        else
            Session["Environment"] = "PROD";
    }
}