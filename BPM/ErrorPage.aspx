<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="jumbotron">
                <p><%--<a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a>--%></p>
              
                 <div class="form-group" id="divError" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="lblErrorMessage" runat="server" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>


            </div>
        </div>
    </form>
</body>
</html>
