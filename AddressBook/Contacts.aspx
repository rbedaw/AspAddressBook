<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="AddressBook.Contacts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .myGrid {
            border: 1px solid #ddd;
            margin: 15px;
            -webkit-border-radius: 3px 3px 0 0;
            -moz-border-radius: 3px 3px 0 0;
            border-radius: 3px 3px 0 0;
        }
        .myGrid td {
            vertical-align:top;
        }
        .header {
            overflow: hidden;
            position: relative;
            border-bottom: 1px solid #ddd;
            height: 30px;
        }
 
            .header th {
                color: #222;
                font-weight: normal;
                line-height: 40px;
                text-align: left;
                /* text-shadow: 0 1px #FFFFFF; */
                white-space: nowrap;
                border-right: 1px solid #ddd;
                border-bottom: 2px solid #ddd;
                padding: 0px 15px 0px 15px;
                -webkit-border-radius: 1px;
                -moz-border-radius: 1px;
            }
 
        .trow1 {
            background: #f9f9f9;
        }
 
        .trow2 {
            background: #fff;
        }
 
            .trow1 td, .trow2 td {
                color: #555;
                line-height: 18px;
                padding: 9px 5px;
                text-align: left;
                border-right: 1px solid #ddd;
                border-bottom: 1px solid #ddd;
                text-align: left;
                
            }
 
        input[type='text'],select {
            border: 1px solid #b8b8b8;
            border-radius: 3px;
            color: #999999;
            float: left;
            height: 22px;
            padding: 0 5px;
            position: relative;
            width: 185px;            
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="myGridView" runat="server" AutoGenerateColumns="false"
                DataKeyNames="ContactId,ContactPhoneType" CellPadding="10" CellSpacing="0"
                ShowFooter="true" CssClass="myGrid" HeaderStyle-CssClass="header" RowStyle-CssClass="trow1"
                AlternatingRowStyle-CssClass="trow2" OnRowCommand="myGridView_RowCommand">

                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>First Name</HeaderTemplate>
                        <ItemTemplate><%#Eval("ContactFirstName") %></ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtContactFirst" runat="server"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="rfF" runat="server" ErrorMessage="*"
                                ForeColor="Red" Display="Dynamic" ValidationGroup="Add" ControlToValidate="txtContactFirst">Required</asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Last Name</HeaderTemplate>
                        <ItemTemplate><%#Eval("ContactLastName") %></ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtContactLast" runat="server"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="rfL" runat="server" ErrorMessage="*"
                                ForeColor="Red" Display="Dynamic" ValidationGroup="Add" ControlToValidate="txtContactLast">Required</asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Contact #</HeaderTemplate>
                        <ItemTemplate><%#Eval("ContactPhone") %></ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtContactNo" runat="server"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="rfCN" runat="server" ErrorMessage="*"
                                ForeColor="Red" Display="Dynamic" ValidationGroup="Add" ControlToValidate="txtContactNo">Required</asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>Phone Type</HeaderTemplate>
                        <ItemTemplate><%#Eval("PhoneTypeName") %></ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddPhoneType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddPhoneType_SelectedIndexChanged">
                                <asp:ListItem Text="Type of Phone" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <asp:RequiredFieldValidator ID="rfPT" runat="server" ErrorMessage="*"
                                ForeColor="Red" Display="Dynamic" ValidationGroup="Add" ControlToValidate="ddPhoneType" InitialValue="0">Required</asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" runat="server" CommandName="Edit">Edit</asp:LinkButton>
                            &nbsp; | &nbsp; <%--non-braking space--%>
                            <asp:LinkButton ID="lbDelete" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnInsert" runat="server" Text="Save" CommandName="Insert" ValidationGroup="Add" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </div>
    </form>
</body>
</html>
