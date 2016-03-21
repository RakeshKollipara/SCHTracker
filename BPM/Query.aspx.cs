using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPM.BusinessEntities;
using BPM.BusinessComponents;
using System.Data.SqlClient;
using System.Data;

public partial class Query : System.Web.UI.Page
{
    int pos;
    PagedDataSource adsource;
    int posHistory;
    PagedDataSource adsourceHistory;
    private const string vPOList = "vPOList";
    private const string vSortDir = "sortDirection";
    private const string grdPageSize = "grdPazeSize";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Cache["QueryList"] == null)
        {
            DropDownBC objBC = new DropDownBC();
            GenericCollection<QueryBE> QueryList = new GenericCollection<QueryBE>();
            QueryList = objBC.GetQueryList();
            Cache.Insert("QueryList", QueryList, null, DateTime.Now.AddDays(30), TimeSpan.Zero);
            Session["QueryList"] = QueryList;
        }

        if (!Page.IsPostBack)
        {
            BindQueryDropDown();
        }

    }
    
    private void BindQueryDropDown()
    {
        DropDownBC objBC = new DropDownBC();
        GenericCollection<QueryBE> QueryList = new GenericCollection<QueryBE>();
        Session["QueryList"] = Cache.Get("QueryList");
        QueryList = Session["QueryList"] as GenericCollection<QueryBE>;
        drdQuery.DataTextField = "QueryText";
        drdQuery.DataValueField = "SNo";
        drdQuery.DataSource = QueryList;
        drdQuery.DataBind();
        drdQuery.Items.Insert(0, "");
    }
    protected void btnExecuteQuery_Click(object sender, EventArgs e)
    {
        bool isSelect = true;
        if (txtQuery.Text.Trim().Contains("update"))
            isSelect = false;
        if (txtQuery.Text.Trim().Contains("delete"))
            isSelect = false;
        if (txtQuery.Text.Trim().Contains("truncate"))
            isSelect = false;
        if (txtQuery.Text.Trim().Contains("drop"))
            isSelect = false;
        if (txtQuery.Text.Trim().Contains("create"))
            isSelect = false;
        if (txtQuery.Text.Trim().Contains("alter"))
            isSelect = false;
        if (txtQuery.Text.ToLower() == "please write your query here")
            isSelect = false;
        if (isSelect)
        {
            TransactionBC objBC = new TransactionBC();
            DataSet ds = new DataSet();

            ds = objBC.ExecuteQueryWindow(txtQuery.Text.Trim(), drdQuery.SelectedItem.Text.Split('!')[1].ToString(), chkCeva.Checked);
            Session["ResultList"] = ds;
            if (ds != null)
            {
                gvResults.DataSource = ds;
                gvResults.DataBind();
                lblError.Visible = false;
                rowGrid.Visible = true;
            }
            else
            {
                gvResults.DataSource = ds;
                gvResults.DataBind();
                lblError.Visible = true;
                rowGrid.Visible = false;
                lblError.Text = "No Records Found";
            }

        }
        else
            lblError.Visible = true;
    }

   
    protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {

            lblError.Text = string.Empty;
            DataSet listofBE = (DataSet)Session["ResultList"];

            if (listofBE != null)
            {
                gvResults.PageIndex = e.NewPageIndex;
                gvResults.DataSource = listofBE;
                gvResults.DataBind();
            }
            else
            {
                gvResults.DataSource = null;
                gvResults.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblError.Text = Ex.Message;
            lblError.Visible = true;
            ////logFile.ErrorLogging(Ex);
        }
    }
    protected void drdQuery_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownBC objBC = new DropDownBC();
        GenericCollection<QueryBE> QueryList = new GenericCollection<QueryBE>();
        QueryList = Session["QueryList"] as GenericCollection<QueryBE>;
        if (drdQuery.SelectedValue == "")
        {
            txtQuery.Text = "";
            gvResults.DataSource = null;
            gvResults.DataBind();
            lblError.Visible = false;
            rowGrid.Visible = false;
            return;
        }

        for (int i = 0; i < QueryList.Count; i++)
        {
            if (drdQuery.SelectedValue == QueryList[i].SNo.ToString())
            {
                txtQuery.Text = QueryList[i].Query;
                break;
            }
        }

        gvResults.DataSource = null;
        gvResults.DataBind();
        lblError.Visible = false;
        rowGrid.Visible = false;
    }
}