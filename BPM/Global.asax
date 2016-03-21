<%@ Application Language="C#" %>
<%@ Import Namespace="WebSite1" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.DirectoryServices" %>
<%@ Import Namespace="System.DirectoryServices.AccountManagement" %>
<%@ Import Namespace="BPM.BusinessEntities" %>
<%@ Import Namespace="BPM.BusinessComponents" %>
<%@ Import Namespace="System.Web" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }

    void Session_Start(object sender, EventArgs e)
    {
        if (System.Web.HttpContext.Current.Cache["QueryList"] == null)
        {
            DropDownBC objBC = new DropDownBC();
            GenericCollection<QueryBE> QueryList = new GenericCollection<QueryBE>();
            QueryList = objBC.GetQueryList();
            System.Web.HttpContext.Current.Cache.Insert("QueryList", QueryList, null, DateTime.Now.AddDays(30), TimeSpan.Zero);
            Session["QueryList"] = QueryList;
        }
        Session["ContactUs"] = "Please find the contacts for Providing feedback or Report any Issues : ' + '\n' + '  Primary Contact : Gladys Esther (gladyses)' + '\n' + ' Secondary Contact : Rakesh Kollipara (v-rakek). Thank you for your valuable feedback or Suggestion";
        if (System.Web.HttpContext.Current.Cache["PartnerList"] == null)
        {
            TransactionBC objBC = new TransactionBC();
            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();
            PartnerList = objBC.GetPartnerList();
            System.Web.HttpContext.Current.Cache.Insert("PartnerList", PartnerList, null, DateTime.Now.AddDays(30), TimeSpan.Zero);
            Session["PartnerList"] = PartnerList;
        }
        else
        {
            Session["PartnerList"] = (GenericCollection<PartnerBE>)System.Web.HttpContext.Current.Cache["PartnerList"];
        }
        if (System.Web.HttpContext.Current.Cache["ExtranetPartnerList"] == null)
        {
            TransactionBC objBC = new TransactionBC();
            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();
            PartnerList = objBC.GetExtraPartnerList();
            System.Web.HttpContext.Current.Cache.Insert("ExtranetPartnerList", PartnerList, null, DateTime.Now.AddDays(30), TimeSpan.Zero);
            Session["ExtranetPartnerList"] = PartnerList;
        }
        else
        {
            Session["ExtranetPartnerList"] = (GenericCollection<PartnerBE>)System.Web.HttpContext.Current.Cache["ExtranetPartnerList"];
        }

        if (System.Web.HttpContext.Current.Cache["CorpnetPartnerList"] == null)
        {
            TransactionBC objBC = new TransactionBC();
            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();
            PartnerList = objBC.GetCorpPartnerList();
            System.Web.HttpContext.Current.Cache.Insert("CorpnetPartnerList", PartnerList, null, DateTime.Now.AddDays(30), TimeSpan.Zero);
            Session["CorpnetPartnerList"] = PartnerList;
        }
        else
        {
            Session["CorpnetPartnerList"] = (GenericCollection<PartnerBE>)System.Web.HttpContext.Current.Cache["CorpnetPartnerList"];
        }
        
        //string myself = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        string myself = HttpContext.Current.User.Identity.Name;
        bool IsAuthenticated = false;

        PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

        //find the group in question
        GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, "MSCITBPM");

        //if found....
        if (group != null)
        {
            //iterate over members
            foreach (Principal p in group.GetMembers())
            {
                if (p.StructuralObjectClass.ToLower() == "user")
                {
                    if (myself.Split('\\')[1] == p.SamAccountName)
                    {
                        IsAuthenticated = true;
                        break;
                    }
                }
                else
                {
                    if (p.ContextType == ContextType.Domain)
                    {
                        string DomainName = "";
                        if(p.Context.ConnectedServer.ToLower().Contains("fareast"))
                            DomainName = "fareast.corp.microsoft.com";
                        if(p.Context.ConnectedServer.ToLower().Contains("redmond"))
                            DomainName = "redmond.corp.microsoft.com";
                        if(p.Context.ConnectedServer.ToLower().Contains("Extranettest"))
                            DomainName = "Parttest.Extranettest.Microsoft.com";
                        if(p.Context.ConnectedServer.ToLower().Contains("partners"))
                            DomainName = "partners.extranet.microsoft.com";
                        PrincipalContext ctx1 = new PrincipalContext(ContextType.Domain, DomainName);
                        GroupPrincipal group1 = GroupPrincipal.FindByIdentity(ctx1, p.SamAccountName);
                        foreach (Principal p1 in group1.GetMembers())
                        {

                            if (myself.Split('\\')[1] == p1.SamAccountName)
                            {
                                IsAuthenticated = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (!IsAuthenticated)
            Response.Redirect("~/Denied.html");
    }

</script>

