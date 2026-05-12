<%@ Page Title=" " Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="NewTurnover.aspx.cs" Inherits="ShiftTurnover.NewTurnover" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>

<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
    <div class="container">

        <div class="row" style="margin-top: 35px;">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 text-left">
                <asp:Table ID="tblTop" runat="server" CssClass="wiznavbuttons" BackColor="White" CellPadding="5" CellSpacing="5" BorderWidth="0" Width="100%" HorizontalAlign="right">
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="center" ColumnSpan="3">
                            <h1>
                                <asp:Label ID="lblHeader" runat="server" Text="Shift Workers"></asp:Label><br />
                                <asp:Label runat="server" ID="lblMShift" Font-Size="Medium" ForeColor="Black"></asp:Label></h1>

                        </asp:TableCell>
                    </asp:TableRow>
                     <asp:TableRow>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Shift Supervisor (s) <img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlShiftSuper" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlShiftSuper" ErrorMessage="Shift Supervisor is Required" ID="RequiredFieldValidator8" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>


                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnShiftSuper" runat="server" ImageUrl="~/Images/Add.png" CommandName="Add" Width="35px" Height="35px"
                                CausesValidation="false" OnClick="btnShiftSuper_Click" />
                        </asp:TableCell>

                    </asp:TableRow>
                    <asp:TableRow ID="trSecShiftSuper" runat="server" Visible="false" BackColor="#99ccff">
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Secondary Shift Supervisor </label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlSecShiftSuper" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"
                                ControlToValidate="ddlSecShiftSuper" ErrorMessage=" Secondary Shift Supervisor is Required" ID="rfvSecShiftSuper" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnSecShiftSuperDel" runat="server" ImageUrl="~/Images/Delete.png"
                                CommandName="Add" Width="35px" Height="35px" CausesValidation="false" 
                                OnClick="btnSecShiftSuperDel_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Crew Leader(s) <img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlCrewChief" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlCrewChief" ErrorMessage="Crew Leader is Required" ID="RequiredFieldValidator5" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>


                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnCrewChief" runat="server" ImageUrl="~/Images/Add.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnCrewChief_Click" />
                        </asp:TableCell>

                    </asp:TableRow>
                    <asp:TableRow ID="trSecCrewChief" runat="server" Visible="false" BackColor="#99ccff">
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Secondary Crew Leader </label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlSecCrewChief" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlSecCrewChief" ErrorMessage=" Secondary Crew Leader is Required" ID="rfvSecCrewChief" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnSecCrewChiefDel" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnSecCrewChiefDel_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Chiller  Operator(s) <img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlChillerOperator" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlChillerOperator" ErrorMessage="Chiller Operator is Required" ID="RequiredFieldValidator4" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>


                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnChillerOperator" runat="server" ImageUrl="~/Images/Add.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnChillerOperator_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trSecChillerOperator" runat="server" Visible="false" BackColor="#99ccff">
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Secondary Chiller  Operator(s) </label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlSecChillerOperator" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlSecChillerOperator" ErrorMessage=" Secondary Chiller Operator is Required" ID="rfvSecChillerOperator" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnChillerOperatorDel" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnChillerOperatorDel_Click" />
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Boiler Operator(s) <img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlBoilerOperator" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlBoilerOperator" ErrorMessage="Boiler Operator is Required" ID="RequiredFieldValidator1" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>


                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnBoilerOperator" runat="server" ImageUrl="~/Images/Add.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnBoilerOperator_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trSecBoilerOperator" runat="server" Visible="false" BackColor="#99ccff">
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Secondary Boiler  Operator(s) </label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlSecBoilerOperator" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlSecBoilerOperator" ErrorMessage=" Secondary Boiler Operator is Required" ID="rfvSecBoilerOperator" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">

                            <asp:ImageButton ID="btnBoilerOperatorDel" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnBoilerOperatorDel_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Auxiliary Operator(s)<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /> </label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlAuxOperator" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlAuxOperator" ErrorMessage="Auxiliary Operator is Required" ID="RequiredFieldValidator2" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>


                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnAuxOperator" runat="server" ImageUrl="~/Images/Add.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnAuxOperator_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trSecAuxOperator" runat="server" Visible="false" BackColor="#99ccff">
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Secondary Aux Operator(s) </label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlSecAuxOperator" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlSecAuxOperator" ErrorMessage=" Secondary Aux Operator is Required" ID="rfvSecAuxOperator" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnAuxOperatorDel" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnAuxOperatorDel_Click" />
                        </asp:TableCell>
                    </asp:TableRow>


                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Cogen Operator(s) <img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlCogenOperator" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlCogenOperator" ErrorMessage="Cogen Operator is Required" ID="RequiredFieldValidator7" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>


                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnCogenOperator" runat="server" ImageUrl="~/Images/Add.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnCogenOperator_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trSecCogenOperator" runat="server" Visible="false" BackColor="#99ccff">
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Secondary Cogen Operator(s) </label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlSecCogenOperator" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlSecCogenOperator" ErrorMessage=" Secondary Cogen Operator is Required" ID="rfvSecCogenOperator" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnCogenOperatorDel" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnCogenOperatorDel_Click" />
                        </asp:TableCell>
                    </asp:TableRow>







                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Shift Electrician(s)<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /> </label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlElectrician" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlElectrician" ErrorMessage="Shift Electrician(s) is Required" ID="RequiredFieldValidator3" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>


                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnElectrician" runat="server" ImageUrl="~/Images/Add.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnElectrician_Click" />
                        </asp:TableCell>

                    </asp:TableRow>
                    <asp:TableRow ID="trSecElectrician" runat="server" Visible="false" BackColor="#99ccff">
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                <label>Select Secondary Electrician(s) </label>

                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="60%">
                            <asp:DropDownList ID="ddlSecElectrician" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="ddlSecElectrician" ErrorMessage=" Secondary Electrician is Required" ID="rfvSecElectrician" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left" Width="20%">
                            <asp:ImageButton ID="btnElectricianDel" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Add" Width="35px" Height="35px" CausesValidation="false" OnClick="btnElectricianDel_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="left" ColumnSpan="2">
                            Please enter any other information about shift workers, such as callouts.
                <asp:TextBox ID="txtShiftWorkerInfo" TabIndex="11" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="txtShiftWorkerInfo" ErrorMessage="Required" ID="RequiredFieldValidator6" runat="server"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="left">
          
              
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br />
                <br />
                <div class="list-group list-group-horizontal flex-lg-row bhoechie-tab-menu" style="padding-left: 5px; padding-right: 5px; padding-top: 35px;">
                    <asp:Button ID="btnBack" runat="server" Text=" Save And Previous" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Save And Go Back" OnClick="btnSave_Click" />
                    &nbsp;&nbsp;  
                    <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Save And Go Back" OnClick="btnSave_Click" />
                    &nbsp;&nbsp;     
                    <asp:Button ID="btnNext" runat="server" Text=" Save And Next" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Save And Go to Ground" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>

    </div>
</asp:Content>


<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
        <div id="footer" style="display:flex; justify-content:center; align-items:center; gap:12px; padding:10px 0; background:#333; color:#fff; min-height:60px; box-sizing:border-box;">
        <strong style="white-space:nowrap;">
            For anonymous Safety reporting, please scan the QR code:
        </strong>

        <a href="https://dtrdata.orf.od.nih.gov/sto/NearMissSecurity.aspx" target="_blank">
            <img src="Images/Barcode.png"
                 alt="QR Code"
                 style="width:60px; height:60px; display:block;" />
        </a>
    </div>
</asp:Content>
