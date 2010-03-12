<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="Footer" %>
<div id="footer">
    <div id="footer_wrapper">
        <p class="copyright">&copy; 
            <a href="http://www.IndexIt.co.il"> Web-Master: Haim.Azar@gmail.com</a>. All rights reserved. <br /> 
        </p>		
    </div>
</div>

<% if( !Request.IsLocal ) { %>
<script defer="defer" type="text/javascript" charset="utf-8" src="http://search.live.com/bootstrap.js?market=en-US&ServId=SearchBox&ServId=SearchBoxWeb&Callback=WLSearchBoxScriptReady"></script>
<% } %>