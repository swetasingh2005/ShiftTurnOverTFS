<%@ Page Title="Change Role" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="ChangeRole.aspx.cs" Inherits="ShiftTurnover.ChangeRole" %>
<%@ MasterType  virtualPath="~/ShiftTurnover.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var sessionVal = '<%= Session["ssoname"] %>';
        //alert(sessionVal);
        //alert(location.search);

        function replaceQueryParam(param, newval, search) {
            var regex = new RegExp("([?;&])" + param + "[^&;]*[;&]?");
            var query = search.replace(regex, "$1").replace(/&$/, '');

            return (query.length > 2 ? query + "&" : "?") + (newval ? param + "=" + newval : '');
        }

        var str = window.location.search;
        var str2 = replaceQueryParam('id', sessionVal, str)
        //alert(str2);
        //window.location = window.location.pathname + str
        //alert(str);
        //location.search = str;
        if (str != str2) {
            history.pushState({}, null, "ChangeRole.aspx" + str2);
            location.reload();
        }

        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_banner" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Table ID="tblRoles" runat="server" HorizontalAlign="Center">
                        <asp:TableRow>
                            <asp:TableCell>
                                
                                <asp:RadioButtonList id="rdoRoles" runat="server" CellPadding="4" tabindex="1">
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rdoRoles" ErrorMessage="Please choose a role first."></asp:RequiredFieldValidator>
                            </asp:TableCell>
                        </asp:TableRow>
             </asp:Table>

            <asp:Table ID="tblButtons" runat="server" CssClass="wiznavbuttons" HorizontalAlign="Center" Width="450px">
                        <asp:TableRow>
                            <asp:TableCell CssClass="left">
                                
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Center">
                                <asp:Button ID="btnSubmit2" TabIndex="2" runat="server" CssClass="form-control" Text="Submit" BackColor="#b3112c" ForeColor="White" Width="120px" Font-Bold="false" OnClick="btnSubmit_Click" />
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Center">
                                <asp:Button ID="btnCancel2" TabIndex="3" runat="server" CssClass="form-control" Text="Cancel" BackColor="#b3112c" ForeColor="White" Width="100px" Font-Bold="false" OnClick="btnCancel_Click" />
                            </asp:TableCell>
                            <asp:TableCell CssClass="right">
                                
                            </asp:TableCell>
                        </asp:TableRow>
             </asp:Table>
</asp:Content>

<asp:Content ID="Content15" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>
