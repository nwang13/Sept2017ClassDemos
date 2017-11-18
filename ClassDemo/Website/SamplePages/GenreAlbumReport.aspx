<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GenreAlbumReport.aspx.cs" Inherits="SamplePages_GenreAlbumReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <rsweb:ReportViewer ID="GenreAlbumReportViewer" 
        runat="server" Font-Names="Verdana" 
        Font-Size="8pt" WaitMessageFont-Names="Verdana" 
        WaitMessageFont-Size="14pt" Width="100%">
        <LocalReport ReportPath="Reports\GenreAlbumReport.rdlc">
            <DataSources>
                <rsweb:ReportDataSource Name="GenreAlbumReportDS" 
                    DataSourceId="GenreAlbumReportODS">
                </rsweb:ReportDataSource>
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>


    <asp:ObjectDataSource ID="GenreAlbumReportODS" 
        runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GenreAlbumReport_Get" 
        TypeName="ChinookSystem.BLL.TrackController" >
    </asp:ObjectDataSource>

</asp:Content>

