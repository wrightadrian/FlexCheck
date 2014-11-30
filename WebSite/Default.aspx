<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WSDL Checker for Flex 3 & 4</title>
    <link rel="Stylesheet" type="text/css" href="StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label ID="lblXmlUri" runat="server" Text="WSDL URL"/>
    <br />
    <%-- the value displayed in the text box can be overridden using <appSettings> in web.config --%>
    <asp:TextBox ID="tbXmlUri" runat="server" Width="865px">http://s3.amazonaws.com/doc/2006-03-01/AmazonS3.xsd</asp:TextBox>
    <br />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="tbXmlUri" Text="The XML URL is required."></asp:RequiredFieldValidator><br />
    <p align="center">
    <asp:Button ID="btnSubmit" runat="server" Text="Check Compatability" PostBackUrl="~/ShowResults.aspx" />
    </p>
    <div>
    <h1>What does this tool do?</h1>
    <p>This tool checks if a WSDL presents any compatibility issues with Flex 3 & 4.</p>
    <p>The 
    <asp:HyperLink ID="urlFlex3Doc" Target="_blank" NavigateUrl="http://livedocs.adobe.com/flex/3/html/data_access_3.html#202409" runat="server">Flex 3 documentation</asp:HyperLink> and
    <asp:HyperLink ID="urlFlex4Doc" Target="_blank" NavigateUrl="http://livedocs.adobe.com/flex/3/html/data_access_3.html#202409" runat="server">Flex 4 documentation</asp:HyperLink> 
    state that -</p>
        
    <p><q>The following XML schema structures or structure attributes are only partially implemented in Flex 3/4</q></p>
    <pre>
&lt;choice>
&lt;all>
&lt;union></pre>
    <q>The following XML Schema structures or structure attributes are ignored and are not supported in Flex 3/4</q>
    <pre>
&lt;attribute use="required"/>

&lt;element
    substitutionGroup="..."
    unique="..."
    key="..."
    keyref="..."
    field="..."
    selector="..."/>

&lt;simpleType>
    &lt;restriction>
        &lt;minExclusive>
        &lt;minInclusive>
        &lt;maxExclusiv>
        &lt;maxInclusive>
        &lt;totalDigits>
        &lt;fractionDigits>
        &lt;length>
        &lt;minLength>
        &lt;maxLength>
        &lt;enumeration>
        &lt;whiteSpace>
        &lt;pattern>
    &lt;/restriction>
&lt;/simpleType>

&lt;complexType
    final="..."
    block="..."
    mixed="..."
    abstract="..."/>

&lt;any 
processContents="..."/>

&lt;annotation></pre>
    </div>
    </form>
</body>
</html>