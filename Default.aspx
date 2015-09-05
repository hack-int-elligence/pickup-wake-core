<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Webwakeup.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CSASPNETAJAXWebwakeup</title>

    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jQuery/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jquery.ui/1.8.5/jquery-ui.min.js"></script>
    <script type="text/javascript" src="scripts/wakeupnetwork.js"></script>

    <link rel="Stylesheet" type="text/css" href="http://ajax.microsoft.com/ajax/jquery.ui/1.8.5/themes/dark-hive/jquery-ui.css" />


    <script type="text/javascript">
        function pageLoad(sender, e) {
            // Get the network list when page loaded.
            fuGetnetworkList();
        }
    </script>

    <style type="text/css">
        body
        {
            font-size: 12px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/Services/Transition.svc" />
            </Services>
            <Scripts>
                <asp:ScriptReference Path="~/Scripts/wakeupbox.js" />
            </Scripts>
        </asp:ScriptManager>


        <fieldset>
            <legend>Control Panel</legend>
            <asp:Panel runat="server" ID="PnlControlPanel" Style="padding: 10px;">
                <asp:Label ID="lblAlias" runat="server" Text="your alias:"></asp:Label><asp:TextBox
                    ID="txtAlias" runat="server" Width="80px"></asp:TextBox>
                <asp:Button ID="btnShowwakeupnetworkForm" runat="server" OnClientClick="fnShowwakeupnetworkForm();return false;"
                    Text="Create wakeup network" />
            </asp:Panel>
        </fieldset>
        <fieldset>
            <legend>wakeup network List</legend>
            <asp:Panel runat="server" ID="PnlwakeupnetworkList" Style="padding: 10px;">
                <table id="tblnetworkList" cellpadding="3" cellspacing="0" border="1" style="text-align: center">
                </table>
            </asp:Panel>
        </fieldset>
    </div>
    <div id="divCreatewakeupnetworkForm" style="display: none;">
        <table width="100%">
            <tr>
                <td style="width: 150px; text-align: right;">
                    network Name:
                </td>
                <td style="width: 350px;">
                    <asp:TextBox ID="txtnetworkName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr style="display: none;">
                <td style="text-align: right;">
                    network Password:
                </td>
                <td>
                    <asp:CheckBox ID="chkNeedPassword" Checked="false" runat="server" />
                    <asp:TextBox ID="txtPassword" Enabled="false" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    Max client:
                </td>
                <td>
                    <asp:DropDownList ID="ddlMaxclient" runat="server">
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                        <asp:ListItem Value="3">4</asp:ListItem>
                        <asp:ListItem Value="3">5</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right;">
                    <asp:Button ID="btnCreatewakeupnetwork" runat="server" OnClientClick="fuCreatewakeupnetwork();return false;"
                        Text="Create" />
                </td>
            </tr>
        </table>
    </div>
    <div id="DivPacket" style="display: none;">
    </div>
    </form>
</body>
</html>
