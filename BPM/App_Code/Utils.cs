using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Web.SessionState;
using BPM.BusinessEntities;
public class Utils
{
   
    public Utils()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #region ValidateRegularExpression
    /// <summary>
    /// Compares the passed String with the "Regular expression Pattern"
    /// </summary>
    /// <param name="regExPattern">"Regular expression Pattern"</param>
    /// <param name="passedString">String to be compared</param>
    /// <returns>Returns true/false based on the matching between "string to compare" and "Regular expression Pattern"</returns>
    public static bool ValidateRegularExpression(string regExPattern, string passedString)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(passedString, regExPattern);
    }

    /// <summary>
    /// Validates State Information before Addition or Updation.
    /// </summary>
    /// <returns>returns True/false accordingly</returns>
    public bool ValidateTextBoxes(TextBox textBox)
    {
        bool validState = true;

        if (textBox.Text.Trim().Length > 50)
        {
            validState = false;
        }
        else if (!Utils.ValidateRegularExpression("[A-Za-z0-9 ]*$", textBox.Text.ToString().Trim()))
        {
            validState = false;
        }
        return validState;
    }

    public bool ValidateValues(string strtextBox)
    {
        bool validState = true;

        if (strtextBox.Trim().Length > 50)
        {
            validState = false;
        }
        else if (!Utils.ValidateRegularExpression("[A-Za-z0-9 ]*$", strtextBox.ToString().Trim()))
        {
            validState = false;
        }
        return validState;
    }

    public bool IsEmptyString(string strInput)
    {
        bool bRet = false;
        if (IsNullReference(strInput))
        {
            bRet = true;
        }
        if (strInput.Length == 0)
        {
            bRet = true;
        }
        return bRet;
    }

    /// <summary>
    /// <para>Check if the <paramref name="strInput"/> is null.</para>
    /// </summary>
    /// <param name="strInput"></param>
    /// <returns>true if given input is null</returns>
    public bool IsNullReference(string strInput)
    {
        bool bRet = false;
        if (strInput == null)
        {
            bRet = true;
        }
        return bRet;
    }

    /// <summary>
    /// <para> Check if <paramref name="strInput" matches the defined pattern/></para>
    /// </summary>
    /// <param name="strInput"></param>
    /// <param name="strPattern"></param>
    /// <returns></returns>
    public bool IsPatternMatch(string strInput, string strPattern)
    {
        bool bRet = true;
        if (!Regex.IsMatch(strInput, strPattern))
        {
            bRet = false;
        }
        return bRet;
    }
    public string IsCardID(string cardID)
    {
        return cardID = cardID.Substring(6, 8);
    }
    public static DataControlField GetColumnByText(GridView gv, String strHeader)
    {
        DataControlField dc = null;

        if (gv != null)
        {
            for (int col = 0; col < gv.Columns.Count; col++)
            {
                if (gv.Columns[col].HeaderText.ToLower() == strHeader.ToLower())
                {
                    dc = gv.Columns[col];
                    break;
                }
            }
        }
        return dc;
    }
    public static void ColsVisible(GridView gv, String strColHeaders, bool IsVisible)
    {
        DataControlField dc;
        string[] strCols = strColHeaders.Split(',');

        for (int cols = 0; cols < strCols.Length; cols++)
        {
            dc = GetColumnByText(gv, strCols[cols]);
            if (dc != null)
            {
                dc.Visible = IsVisible;
            }
        }
    }
    public string GenerateFileName(string fileDirectory, string strFileName)
    {
        int counter = 0;
        string strFile = strFileName;
        string fileExt = "";
        while (File.Exists(fileDirectory + strFile))
        {
            int extPosition = strFile.LastIndexOf('.');
            if (extPosition > 0)
            {
                fileExt = strFile.Substring(extPosition + 1);
                strFile = strFile.Substring(0, extPosition);
            }
            int sepPos = strFile.LastIndexOf("-");
            if (sepPos > 0)
            {
                int tempCount = counter;
                try
                {
                    counter = Convert.ToInt32(strFile.Substring(sepPos + 1));
                }
                catch (Exception)
                {
                    counter = tempCount;
                    sepPos = 0;
                    // do nothing
                }
            }
            if (counter == 0)
            {
                strFile += "-";
            }
            counter++;
            if (sepPos > 0)
            {
                strFile = strFile.Substring(0, sepPos) + "-" + counter;
            }
            else
            {
                strFile += counter;
            }
            if (fileExt.Length > 0)
            {
                strFile = strFile + "." + fileExt;
            }
        }
        return strFile;
    }

    #endregion

   

   

    private static string FindAppDataPath(string strConfigSection)
    {
        string listPath = ConfigurationManager.AppSettings[strConfigSection].ToString();
        string Filepath = HttpContext.Current.Server.MapPath(listPath);
        if (File.Exists(Filepath))
        {
            return Filepath;
        }
        else
        {
            listPath = @"~\" + listPath;
            listPath = HttpContext.Current.Server.MapPath(listPath);
            return listPath;
        }
    }

    //
    /// <summary>
    /// To get the temporary file path for 
    /// creating a temp file to store the xml doc
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private string GetTempFileName(string fileName)
    {
        try
        {
            string path = System.IO.Path.GetTempPath();

            if (path.Substring(path.Length - 1, 1) != "\\")
                path += char.ToString('\\');

            return path + fileName;
        }
        catch
        {
            throw;
        }
    }
    //Filling Zones

  

    /// <summary>
    /// sorts the grid based on sort expression and sort direction
    /// </summary>
    /// <typeparam name="T"> Type</typeparam>
    /// <param name="gvObject">Gridview</param>
    /// <param name="sDir">sortDirection</param>
    /// <param name="sortExpr">sortExpression</param>
    /// <param name="collection">List of Items with genericcollection</param>
    public static void GridSort<T>(GridView gvObject, SortDirection sDir, string sortExpr, GenericCollection<T> collection)
    {
        GenericCollection<T>.GenericComparer comparer = new GenericCollection<T>.GenericComparer(sortExpr, sDir);
        collection.Sort(comparer);
        gvObject.DataSource = collection;
        gvObject.DataBind();
    }
    /// <summary>
    /// sorts the grid based on sort expression and sort direction
    /// </summary>
    /// <typeparam name="T"> Type</typeparam>
    /// <param name="gvObject">Gridview</param>
    /// <param name="sDir">sortDirection</param>
    /// <param name="sortExpr">sortExpression</param>
    /// <param name="collection">List of Items</param>
    public static void GridSort<T>(GridView gvObject, SortDirection sDir, string sortExpr, List<T> collection)
    {
        GenericComparer<T> comparer = new GenericComparer<T>(sortExpr, sDir);//Initializes new comparer

        collection.Sort(comparer);//sorts
        //databinding
        gvObject.DataSource = collection;
        gvObject.DataBind();
    }

    /// <summary>
    /// sorts the grid based on sort expression and sort direction
    /// </summary>
    /// <typeparam name="T"> Type</typeparam>
    /// <param name="gvObject">Gridview</param>
    /// <param name="sDir">sortDirection</param>
    /// <param name="sortExpr">sortExpression</param>
    /// <param name="collection">List of Items with genericcollection</param>
    public static GenericCollection<T> GridSorting<T>(GridView gvObject, SortDirection sDir, string sortExpr, GenericCollection<T> collection)
    {
        GenericCollection<T>.GenericComparer comparer = new GenericCollection<T>.GenericComparer(sortExpr, sDir);
        collection.Sort(comparer);
        gvObject.DataSource = collection;
        gvObject.DataBind();
        return collection;
    }
    /// <summary>
    /// sorts the grid based on sort expression and sort direction
    /// </summary>
    /// <typeparam name="T"> Type</typeparam>
    /// <param name="gvObject">Gridview</param>
    /// <param name="sDir">sortDirection</param>
    /// <param name="sortExpr">sortExpression</param>
    /// <param name="collection">List of Items</param>
    public static List<T> GridSorting<T>(GridView gvObject, SortDirection sDir, string sortExpr, List<T> collection)
    {
        GenericComparer<T> comparer = new GenericComparer<T>(sortExpr, sDir);//Initializes new comparer

        collection.Sort(comparer);//sorts
        //databinding
        gvObject.DataSource = collection;
        gvObject.DataBind();
        return collection;
    }
    public static void GridPaging<T>(GridView gvObject, Int32 PageSize, GenericCollection<T> listObj)
    {
        gvObject.PageSize = PageSize;
        gvObject.DataSource = listObj;
        gvObject.DataBind();
    }
    public static void GridPaging<T>(GridView gvObject, Int32 PageSize, List<T> listObj)
    {
        gvObject.PageSize = PageSize;
        gvObject.DataSource = listObj;
        gvObject.DataBind();
    }
    /// <summary>
    /// Uploads the files to the specified directory. 
    /// Throws FileNotFoundException when there is no file at the location or the file size is 0. 
    /// </summary>
    /// <param name="dirPath"></param>
    public static void uploadFiles(string dirPath)
    {
        bool filecheck = false;
        HttpFileCollection uploads = HttpContext.Current.Request.Files;//receives the current context Files
        List<string> fileNames = new List<string>();
        long size = 0;
        if (uploads.Count > 25)
        {
            throw new Exception("Maximum of 25 Attachments only allowed");
        }
        for (int i = 0; i < uploads.Count; i++)
        {
            HttpPostedFile upload = uploads[i];//file;
            if (upload.FileName.Trim() == string.Empty)
                continue;
            //implementing GUI rule for given file formats validation
            FileInfo certInfo = new FileInfo(upload.FileName.Trim());
            string extention = certInfo.Extension.ToLower();
            if (extention == ".doc" || extention == ".docx" || extention == ".xls" || extention == ".xlsx" || extention == ".pdf" || extention == ".txt" || extention == ".ppt" || extention == ".pptx" || extention == ".jpeg" || extention == ".jpg" || extention == ".bmp" || extention == ".csv" || extention == ".xps" || extention == ".vsd")
                filecheck = true;
            else
                filecheck = false;
            if (filecheck == false)
                throw new Exception("Invalid Attachment .Valid Attachments : doc, docx, xls, xlsx, pdf, txt, ppt, pptx, jpeg, jpg, bmp, csv, xps, vsd");
            //end 
            if (upload.ContentLength == 0)
                throw new FileNotFoundException("The specified file " + upload.FileName + " doesn't exists or invalid.");
            //continue;

            string c = System.IO.Path.GetFileName(upload.FileName.Trim()); // we don't need the path, just the name.
            size = (upload.ContentLength);
            if (size > 4096000)
            {
                throw new Exception("Attachments size should not exceed 4MB");
            }

            try
            {
                //saves the file by appending GUID if the another fileExists with same name
                if (File.Exists(dirPath + c))
                {
                    c = Guid.NewGuid().ToString() + c;
                }
                upload.SaveAs(dirPath + c); //saves at the directory
                fileNames.Add(dirPath + c);//add the file name to the list
            }
            //Throws Exception if the file is invalid or file doesn't exists
            catch (FileNotFoundException)
            {
                DeleteFiles(fileNames);//Delete the uploaded files if an exception occurs
                throw;
            }
            catch (Exception exp)
            {                
                DeleteFiles(fileNames);//Delete the uploaded files if an exception occurs
                LogHelper logfile = new LogHelper();
                logfile.ErrorLogging(exp);
            }
        }
    }
    public static void uploadDocFiles(string dirPath)
    {
        bool filecheck = false;
        HttpFileCollection uploads = HttpContext.Current.Request.Files;//receives the current context Files
        List<string> fileNames = new List<string>();
        long size = 0;
        if (uploads.Count > 25)
        {
            throw new Exception("Maximum of 25 Attachments only allowed");
        }
        for (int i = 0; i < uploads.Count; i++)
        {
            HttpPostedFile upload = uploads[i];//file;
            if (upload.FileName.Trim() == string.Empty)
                continue;
            //implementing GUI rule for given file formats validation
            FileInfo certInfo = new FileInfo(upload.FileName.Trim());
            string extention = certInfo.Extension.ToLower();
            if (extention == ".doc" || extention == ".docx" || extention == ".pdf" || extention == ".chm")
                filecheck = true;
            else
                filecheck = false;
            if (filecheck == false)
                throw new Exception("Invalid Attachment .Valid Attachments : doc, docx, pdf,chm");
            //end 
            if (upload.ContentLength == 0)
                throw new FileNotFoundException("The specified file " + upload.FileName + " doesn't exists or invalid.");
            //continue;

            string c = System.IO.Path.GetFileName(upload.FileName.Trim()); // we don't need the path, just the name.
            size = (upload.ContentLength);
            if (size > 10240000)
            {
                throw new Exception("Attachments size should not exceed 10MB");
            }

            try
            {
                //saves the file by appending GUID if the another fileExists with same name
                if (File.Exists(dirPath + c))
                {
                    c = Guid.NewGuid().ToString() + c;
                }
                upload.SaveAs(dirPath + c); //saves at the directory
                fileNames.Add(dirPath + c);//add the file name to the list
            }
            //Throws Exception if the file is invalid or file doesn't exists
            catch (FileNotFoundException)
            {
                DeleteFiles(fileNames);//Delete the uploaded files if an exception occurs
                throw;
            }
            catch (Exception exp)
            {
                //span1.innerhtml = "upload(s) failed.";
                DeleteFiles(fileNames);//Delete the uploaded files if an exception occurs
                LogHelper logfile = new LogHelper();
                logfile.ErrorLogging(exp);
            }
        }
    }
    /// <summary>
    /// Gets the content type of the downloading file....
    /// </summary>
    /// <param name="Filename"></param>
    /// <returns></returns>
    public static string MimeType(string Filename)
    {
        string mime = "application/octetstream";
        try
        {
            string ext = System.IO.Path.GetExtension(Filename).ToLower();
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (rk != null && rk.GetValue("Content Type") != null)
                mime = rk.GetValue("Content Type").ToString();
        }
        catch (Exception ex)
        {
            LogHelper logfile = new LogHelper();
            logfile.ErrorLogging(ex);
        }
        return mime;
    }
    /// <summary>
    /// Deletes the files in list of filenames
    /// </summary>
    /// <param name="fileNames"></param>
    public static void DeleteFiles(List<string> fileNames)
    {
        try
        {
            foreach (string s in fileNames)
            {
                File.Delete(s);
            }
        }
        catch (Exception ex)
        {
            LogHelper logfile = new LogHelper();
            logfile.ErrorLogging(ex);
        }
    }
    public static void BindToolTip(DropDownList ddl)
    {
        ddl.Attributes.Add("onmouseover", "showTip(" + ddl.ClientID + ",this.options[this.selectedIndex].text)");
        ddl.Attributes.Add("onmouseout", "hideText()");
    }

    public static int bindRepeater(Repeater RepGvPageSize)
    {
        int defltPagesize = 10;
        XmlDocument xmldoc = new XmlDocument();
        DataTable dt = new DataTable();
        dt.Columns.Add("PageSize");
        string path = FindAppDataPath("ConfigXMLPath");
        xmldoc.Load(path);
        XmlNodeList sectionNodes = xmldoc.SelectNodes("//NoOfRecordsPerPage/Type");
        try
        {
            for (int iLoop = 0; iLoop < sectionNodes.Count; iLoop++)
            {
                dt.Rows.Add(sectionNodes[iLoop].Attributes["Name"].Value);
            }
            defltPagesize = Convert.ToInt16(sectionNodes[0].Attributes["Name"].Value); //Set default page size here
            RepGvPageSize.DataSource = dt;
            RepGvPageSize.DataBind();
        }
        catch (Exception)
        {
        }
        return defltPagesize;
    }

    public static void SetRptDefaultPage(Repeater RepGvPaging, int defPageSize)
    {
        try
        {
            foreach (RepeaterItem item in RepGvPaging.Items)
            {
                LinkButton lbtSize = (LinkButton)item.FindControl("lbtnSize");
                Label lblSize = (Label)item.FindControl("lblPgSize");
                if (lbtSize.Text.Trim() == defPageSize.ToString())
                {
                    lbtSize.Visible = false;
                    lblSize.Visible = true;
                }
                else
                {
                    lbtSize.Visible = true;
                    lblSize.Visible = false;
                }
            }
        }
        catch (Exception)
        {
        }
    }

    public static string GridRecDispMsg(int pageindex, int PageSize, int Count)
    {
        string msg;
        if (Count > ((pageindex + 1) * PageSize))
        {
            msg = "Displaying " + (((pageindex) * PageSize) + 1) + "-" + (pageindex + 1) * PageSize + " of " + Count + " Records";
        }
        else
        {
            msg = "Displaying " + (((pageindex) * PageSize) + 1) + "-" + Count + " of " + Count + " Records";
        }
        return msg;
    }
    public DataTable createPages(int si, int ei)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PgNo");
        for (int i = si; i <= ei; i++)
        {
            dt.Rows.Add(i);
        }
        return dt;
    }
    public string DoRequiredCalulculationsForPaging(int totalNoOfRecords, int NoOfRecordsPerPage)
    {
        int totalpages = (int)(Math.Ceiling((double)totalNoOfRecords / NoOfRecordsPerPage));
        int startRecordId = 0;
        int endRecordId = NoOfRecordsPerPage - 1;
        string PageInfo = totalpages + ":" + startRecordId + ":" + endRecordId + ":" + "false";
        return PageInfo;

    }
    /// <summary>
    /// Date Validator
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDate(string value)
    {
        if (value == string.Empty)
        {
            return true;
        }
        else
        {
            DateTime.Parse(value);
            return true;
        }
    }
   

    public static void CatchException(Exception ExMessage, string CodeBlockName, Label ErrorLabel, bool Visibility, string PageName)
    {
        LogHelper logfile = new LogHelper();
        logfile.ErrorLogging(ExMessage);
        ErrorLabel.Visible = Visibility;
        ErrorLabel.Text = ExMessage.Message;
    }
    #region CardValidation
    /// <summary>
    /// Validating card((Checksum is valid or not)
    /// </summary>
    /// <param name="cardIssuerID"></param>
    /// <param name="cardNumber"></param>
    /// <returns></returns>
    public static bool isValidCard(string cardNumber)
    {
        bool result = false;
        int number = 0;
        int sumAll = 0;
        int CheckSum = 0;

        if (!string.IsNullOrEmpty(cardNumber) && cardNumber.Length == 15)
        {
            string strCardIssuerID = System.Configuration.ConfigurationManager.AppSettings["CardIssuerID"];
            string issuerID = cardNumber.Substring(0, 6);//first 6 digits
            string cardPANID = cardNumber.Substring(6, 8);//PANID
            string checkSum = cardNumber.Substring(14, 1);//last digit for checksum

            if (strCardIssuerID != issuerID)
            {
                result = false;
            }
            else
            {
                string cardPan = cardNumber.Substring(0, 14);
                for (int loop = 1; loop <= 14; loop++)
                {
                    number = int.Parse(cardPan.Substring(loop - 1, 1));
                    if (loop % 2 == 0)
                    {
                        sumAll += CheckMoreThanTen(number * 2);
                    }
                    else
                    {
                        sumAll += number;
                    }
                }

                if (sumAll % 10 == 0)
                {
                    CheckSum = 0;
                }
                else
                {
                    int bit = sumAll / 10;
                    CheckSum = (bit + 1) * 10 - sumAll;
                }
                if (CheckSum.ToString() == checkSum)
                {
                    result = true;
                }

            }
        }
        else
        {
            result = false;
        }

        return result;

    }

    public static int CheckMoreThanTen(int Digit)
    {
        int returndigit;
        returndigit = Digit / 10;

        if (Digit < 10)
        {
            return Digit;
        }
        else
        {
            return (returndigit + (Digit % 10));
        }
    }
    #endregion CardValidation

    public static void FormatNumberText(TextBox _txtBox, Double _amt, bool _currency)
    {
        _txtBox.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
        if (_currency)
        {
            _txtBox.Text = "$" + string.Format("{0:0.00}", _amt);
        }
        else
        {
            _txtBox.Text = Convert.ToInt32(_amt).ToString(); ;
        }
    }

  


    public static bool DateValidations(string FromDate, string ToDate, Label lblDateValidation, string dateFrom, string dateTo)
    {

        //GUI rule 1
        if ((FromDate != String.Empty) && (FromDate == String.Empty))
        {
            ToDate = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
        }
        //GUI rule 2
        if ((ToDate != String.Empty) && (FromDate != String.Empty))
        {
            if (DateTime.Parse(FromDate) > DateTime.Now)
            {
                lblDateValidation.Text = "Please correct the following and try again:<br/>" + dateFrom + " cannot be greater than " + dateTo + ".";
                lblDateValidation.Visible = true;
                return false;
            }
            if (DateTime.Parse(ToDate) < DateTime.Parse(FromDate))
            {
                lblDateValidation.Text = "Please correct the following and try again:<br/>" + dateFrom + " cannot be greater than " + dateTo + ".";
                lblDateValidation.Visible = true;
                return false;
            }
            else
            {
                lblDateValidation.Text = string.Empty;
            }
        }
        //GUI rule 3
        if ((FromDate == String.Empty) && (ToDate != String.Empty))
        {
            lblDateValidation.Text = "Please correct the following and try again:<br/>" + dateFrom + " is required.";
            lblDateValidation.Visible = true;
            return false;
        }
        return true;
    }

    //Added Generic Method for Grid Top Paging Repeater
    public void SetRepeaterDefaultPage(Repeater RepGvPaging, int defPageSize, string strButtonSize, string strPageSize)
    {
        try
        {
            foreach (RepeaterItem item in RepGvPaging.Items)
            {
                LinkButton lbtSize = (LinkButton)item.FindControl(strButtonSize);
                Label lblSize = (Label)item.FindControl(strPageSize);
                if (lbtSize.Text.Trim() == defPageSize.ToString())
                {
                    lbtSize.Visible = false;
                    lblSize.Visible = true;
                }
                else
                {
                    lbtSize.Visible = true;
                    lblSize.Visible = false;
                }
            }
        }
        catch (Exception)
        {
        }
    }
}

