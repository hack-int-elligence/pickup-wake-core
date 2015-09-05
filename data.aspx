<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>CSASPNETAJAXWebwakeup</title>

    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jQuery/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="scripts/wakeupPacket.js"></script>


    <style type="text/css">
        body
        {
            margin: 0px;
            background-color: black;
        }
        
        ._tlklinked
        {
            width: 250px;
            float: left;
            margin: 5px 0 5px 0;
            font-size: 12px;
            color: Orange;
            border-bottom: 1px solid gray;
            white-space: normal;
            word-break: break-all;
            overflow: auto;
        }
        ._tlklinked ._talker
        {
            font-size: 12px;
            color: Black;
        }
        
        ._tlklinked ._time
        {
            font-size: 10px;
            color: Gray;
        }
        ._tlklinked ._msg
        {
            font-size: 14px;
            line-height: 20px;
            color: Blue;
            padding-left: 5px;
        }
        
        ._tlkMe
        {
            width: 250px;
            float: left;
            margin: 5px 0 5px 0;
            font-size: 12px;
            color: Green;
            border-bottom: 1px solid gray;
            white-space: normal;
            word-break: break-all;
            overflow: auto;
        }
        ._tlkMe ._talker
        {
            font-size: 12px;
            color: Black;
        }
        
        ._tlkMe ._time
        {
            font-size: 10px;
            color: Gray;
        }
        ._tlkMe ._msg
        {
            font-size: 14px;
            line-height: 20px;
            color: Fuchsia;
            padding-left: 5px;
        }
    </style>

    <script type="text/javascript">
        function pageLoad(sender, e) {
            // Get the current Packet list
            UpdateLocalPacket();
            // Get the current client list in this wakeup network
            UpdatenetworkTalkerList();
        }
    </script>
</head>
<body>

    <form id="form1" runat="server" defaultbutton="btnSendPacket">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Services/Transition.svc" />
        </Services>
    </asp:ScriptManager>

    <div style="width: 400px; border: 1px solid black">
        <table>
            <tr>
                <td style="width: 280px;">
                    <div id="txtPacketList" style="width: 280px; height: 200px; border: 0; background-color: #ffffcc;
                        font-size: 12px; overflow: auto;">
                    </div>
                </td>
                <td valign="top" style="width: 120px;">
                    <asp:ListBox ID="lstclientList" Style="height: 200px; width: 90px; border: 0;" runat="server">
                    </asp:ListBox>
                </td>
            </tr>
            <tr>
                <td valign="middle">
                    <asp:TextBox ID="txtPacket" Style="width: 280px; height: 50px;" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSendPacket" runat="server" OnClientClick="SendPacket($get('txtPacket'));return false;"
                        Text="Send" Style="width: 100px; height: 45px;" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
