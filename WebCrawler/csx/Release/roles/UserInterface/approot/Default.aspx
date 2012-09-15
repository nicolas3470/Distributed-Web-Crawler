<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="UserInterface._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Nick&#39;s 5412 web crawler</h2>
    <h3>
        Search:</h3>
    <p>
        <asp:TextBox ID="searchBox" runat="server" MaxLength="25" Width="244px"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;
        <asp:Button ID="searchButton" runat="server" Text="Go" />
    </p>
    <p>
        <asp:Label ID="errorMsg" runat="server" Font-Size="XX-Large" 
            ForeColor="#FF3300" Text="Error: Database Error" Visible="False"></asp:Label>
    </p>
    </asp:Content>
