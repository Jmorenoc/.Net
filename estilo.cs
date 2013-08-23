/*          */
/* Basico   */
/*          */
body   
{
    background: #b6b7bc;
    font-size: .80em;
    font-family: Arial;
    margin: 0px;
    padding: 0px;
    color: #007DC3; 
    display:block;    
}

a:link, a:visited
{
    color: #A9A9A9; 
}

a:hover
{
    color: #1d60ff;
    text-decoration: none;
}

a:active
{
    color: #034af3;
}

hr
{
    border-color: #0075bc;
    border: 0.3px;
    border-style:solid;
    width: 90%;
}
/*                      */
/* Forma de la Pagina   */
/*                      */
.page
{
    width: 850px;
    height: 549px;
    background-color: #DEDEDE;
    margin-left: auto;
    margin-right: auto;    
}

.header
{
    height: 108px;
    position: relative;
    margin: 0px;
    padding: 0px;    
    width: 100%;
    top: 0px;
    left: 0px;
}

.footer
{
    width: 849px;
    height: 32px;    /*height: 46px;    */
    /*position: absolute; /*position: relative;*/      
    clear:both;    
    background-color: #013b60;    
    Border: 1px solid #000000;    
    margin-left: auto;
    margin-right: auto;
}
/*          */
/* Capas    */
/*          */
div.Base
{
    height: 400px;
    width: 800px;
    border: 1px solid #1A78BE;
    margin-left: auto;
    margin-right:auto;
    margin-top: 20px;
    margin-bottom: 20px;
    background-color: White;    
    max-height: 400px;
}

div.Mapa
{   
    margin-top:10px;
    margin-left: 10px;
    height: 25px;    
    padding: 3px 0px 3px 3px;    
}   

div.Principal
{   
    clear: both;    
}

div.Inferior
{    
    height: 25px;           
    margin: 3px 0px 3px 0px;
    height: 32px;    /*height: 46px;    */        
    vertical-align: bottom;
    margin-bottom: 0;
}

div.Izquierda
{
    float: left;
    text-align: left;
}

div.Derecha
{    
    text-align:right;
    float: right;
}

div.Centrado
{
    margin-left:auto;
    margin-right:auto;    
}

div.Fixed
{
    position:fixed;    
}

div.Fill
{
    display:inline-block;    
}

div.Flotante
{    
    position: absolute;
    margin-top: 20px;
    width:530px;
    float:right;
}

div.Mover
{
    position:relative;
    height:35px;
    width:150px; 
    border: 1px dotted red;   
}

div.Despejar    {   height: 10px;   }
div.Separador   {   height: 25px;   }
.clear          {   clear: both;    }

/* Margen */

div.MargenEstandar
{
    margin: 25px 25px 25px 25px;            
}

div.MargenBase
{
    margin: 25px 25px 25px 25px;
}

div.MargenMenor
{
    margin: 15px 15px 15px 15px;
}

div.MargenMinimo
{
    margin: 5px 5px 5px 5px;    
}

div.MargenDerecho
{
    margin-right: 50px;    
}

div.MargenIzquierdo
{
    margin-left:50px;    
}

div.MargenSuperior
{
    margin-Top: 15px;    
}

div.MargenInferior
{
    margin-bottom: 25px;    
}

div.MargenLateral
{
    margin-right: 25px;
    margin-left: 25px;
}

div.MargenInterior
{
    padding: 15px 15px 15px 15px;    
    height: 100%;
}


/* login */

div.ghostLogin
{
    background-color:#DEDEDE;
    margin-left: -10px;
    margin-top: -10px;
    width: 102%;
    /*height: 104%;    */
    height: 430px;
    position:relative;
    overflow: hidden;
    display: table;
}

div.login
{    
    height:233px;
    width:402px;
    margin-left:auto;
    margin-right:auto;    
    margin-top:80px;
    border:1px solid #0075bc;
    background-color:White;
}

div.login div
{
    margin-top: 30px;
}

div.Labels
{   
    font-weight:bold; 
    float:left;
    margin-left:50px;
    width: 120px;    
}

div.Label
{    
    height: 20px;    
    line-height: 20px;
    vertical-align: middle;    
}

div.Headers
{
    display:block;
    width: 182px;
    height: 25px;
    border: 1px solid #0075bc;
    background-color: #DFF0FA;    
    text-align: center;
}

.Usuario
{
    background-color: #0075bc;
    color: White;
    width: 70px;
    font-size: 8pt;
    Border: none;
}

.MenuUsuario
{
    vertical-align: middle;    
}

.MenuUsuario ul li
{    
    background-color: #0075bc;
    width: 70px;
    Border: none;
    vertical-align:middle;
}

.MenuUsuario ul li a
{
    color:White;
    font-size: 8pt;
    vertical-align: middle;
}

.MenuUsuario ul li ul li
{
    background-color: black;
    width: 70px;
    Border: none;
    vertical-align:middle;    
}

.MenuUsuario ul li ul li a
{
    color: white;
    font-size: 10pt;
    vertical-align: middle;
}

.UsrIcon
{
    /*background-image: url(/Graficos/Icons/User.png);*/
    background-color: #0075bc;
    width: 23px;
    height:23px;
}

div.Fields
{    
    float:right;
    margin-right:50px;        
}

.loginInput, .Watemark 
{
    color: #0075bc;
    width: 150px;    
    border: 1px solid #0075bc;
}

.loginButton 
{    
    background-image: url(/Graficos/Imagenes/boton.png);
    float:right;
    color: White;
    margin-right: 7px;
    border: 0px;
    width:87px;
    height:26px;        
}

/*              */ 
/* Componentes  */ 
/*              */ 

.Guardar
{
	background-image: url(/Graficos/Imagenes/boton.png);
    float:right;
    color: White;
    /*margin-right: 150px;
    margin-top: 10px;*/
    border: 0px;
    width:87px;
    height:26px;
}

.Accion
{    
    background-image: url(/Graficos/Imagenes/boton.png);    
    color: White;        
    border: 0px;
    width:87px;
    height:22px;        
}

.BotonFlotante
{
    Width: 25px;
    height:25px;
}


.Negativo
{
    height:20px;
    width: 80px;
    background-color: #B80C0C;
    color: #FFFFFF; 
    border: none;       
}

.Positivo
{
    height:20px;
    width: 80px;
    background-color: #0F8F27;
    color: #FFFFFF;
    border: none;
}

input.busqueda[type="text"]
{
    width: 250px;
}

.TextBox /*:focus*/
{    
    border:1px solid #0075bc;
    font-family:Verdana, Geneva, Arial, Helvetica, sans-serif;
    font-size:1em; /* Cambiar a gusto*/
    color:#0075bc;
    width : 200px; /* Cambiar a gusto*/
    height:17px;    
}

.TextBox[disabled='disabled']
{
    border:1px solid #0075bc;
    background-color: #B2B2B2;
    font-family:Verdana, Geneva, Arial, Helvetica, sans-serif;
    font-size:1em; /* Cambiar a gusto*/
    color:#0075bc;
    width : 200px; /* Cambiar a gusto*/
    height:17px;        
}

.TextBoxMenor /*:focus*/
{    
    border:1px solid #0075bc;
    font-family:Verdana, Geneva, Arial, Helvetica, sans-serif;
    font-size:1em; 
    color:#0075bc;
    width : 115px; 
    height:17px;
}

.TextBoxMenor[disabled='disabled']
{
    background-color: #B2B2B2;
    border-color: White;
}

.TextBoxBusqueda
{    
    border: 1px solid #E3E9EF;
    border-top-color: #ABADB3;
    height: 19px;
    width: 255px;
    vertical-align:middle;
    color: #1A78BE;
    margin-right: 10px;
    margin-left: 10px;        
}

.TextBoxDato
{
    width: 120px;   
}

.Choice label
{   
    line-height: 25px;
}

.Borde
{
    border: 1px solid #1A78BE;        
}

.Texto
{
    color: #1A78BE;    
}

.TextoCentrado
{
    text-align: center;
}

.Combo
{
    border: 1px solid #1A78BE;        
    width: 204px;    
    font-size: 10;
    color: #1A78BE;
    font: arial;     
}

.Combo[disabled="disabled"]
{
    border: 1px solid #1A78BE;        
    background-color: #B2B2B2; 
    width: 204px;    
    font-size: 10;
    color: #1A78BE;
    font: arial;     
}

.ComboMenor
{
    border: 1px solid #1A78BE;        
    width: 120px;    
    font-size: 10;
    color: #1A78BE;
    font: arial;     
}

.ComboMenor[disabled="disabled"]
{
    background-color: #B2B2B2;
    border-color: White;
}

.Combo:disabled
{
    border: 1px solid #1A78BE;        
    width: 204px;    
    font-size: 10;
    background-color:Red;
    color: #1A78BE;
    font: arial;
}

.Campo
{
    width: 94%;    
}

.Label
{
    margin-right: 5px;    
}

.ComboFiltro
{
    border: 1px solid #1A78BE;        
    width: 150px;    
    font-size: 10;
    color: #1A78BE;
    font: arial;
    vertical-align: top;    
}

.TextBoxFiltro
{
    border: 1px solid #1A78BE;
    height: 15px;
    width: 100px;
    vertical-align:middle;    
    color: #1A78BE;    
}

.ListBox
{
    border-style: solid;
    border-width: 1px;
    border-color: #0075BC;
    height : 75px;
    max-height: 500px;
    width: 149px;
    font-size: 0.7em;
    color: #0075BC;
}

.SubLista
{
    width:100%;    
    height: 90px;
    color: #0075BC;
    Border: 1px solid #0075BC;
}

.Lista
{
    margin-top: 4px;
    width: 140px;
    margin-left: 3px;
    height: 90px;   
    border: 1px solid #0075BC;    
}

.ListaConceptos
{
    text-transform:lowercase;
    color: #0075BC;    
    width:100%;
    height:100%;
}

.ListaConceptos:first-letter
{
    text-transform:capitalize;
}

.ListaItems li
{
    padding: 5px 5px 5px 5px;
    margin: 5px 0px 5px 0px;        
}

.GrillaMenor
{
    width:100%;
    font-size: 10pt;    
    border: 1px solid #0075bc;
}

.GrillaMenor th
{
    color: #FFFFFF;
    background-Color:#0075bc;    
}

.GrillaMenor td
{
    background-color: #FFFFFF;    
}

.GrillaMenorAlternada
{
    background-color: #EFEFEF;
}

/* Grilla */
.Grilla {        
    width: 100%;    
    width: 555px;	
	margin-right: auto;
	margin-left: auto;	
	border: 1px solid #A0A0A0;	
}

.Grilla th
{    
    Background-Color: #0075bc;
    color: #FFFFFF;
    font-size: 11pt;
    height: 35px;
    border: 1px solid #A0A0A0;
}

.Grilla td
{
    font-size: 10pt;
    text-transform:capitalize;
    height: 11%;
}

.GrillaAlternada
{
    border: 1px solid #A0A0A0;
    font-size: 10pt;
    height: 11%;
    color: #0075bc;
    background-color: #EFEFEF;
}

.GrillaBase
{
    width: 100%;    
    height: 100%;
	margin-right: auto;
	margin-left: auto;	
	border: 1px solid #A0A0A0;	     
}

.GrillaBase th
{    
    Background-Color: #0075bc;
    color: #FFFFFF;
    font-size: 9pt;    
    height: 10px;
    border: 1px solid #A0A0A0;
}

.GrillaBaseAlternada
{    
    border: 1px solid #A0A0A0;
    font-size: 9pt;    
    color: #0075bc;
    background-color: #EFEFEF;
}

/* TABLA */

.PanelProducto
{
    width: 700px;
    height: 200px;
    margin-left: auto;
    margin-right: auto;    
}

.PanelBordes
{
    border: 1px solid #0075bc;
}

.Tabla
{
    /*width: 600px;*/
    height: 250px;
    max-height: 350px;
    margin-left: auto;
    margin-right: auto;    
    clear:none;
}

.Tabla .Cabecera td
{
     background-color: #0075bc;  
     font-size: 12px;
     height: 15px;
     color: White;
     text-align: center;
     padding-top: 7px;
}

.Tabla td 
{
    border: 1px solid #0075bc;
    text-align: center;    
    font-size: 10px;
    vertical-align:top;
    min-width: 160px; /*min-width: 105px; */
}

.Tabla td td
{
    border: none;
    text-align: left;
    border-top: 0px;    
}
/* Tab */
.MyTabs
{
    margin-left: auto;
    margin-right: auto;
}

.MyTabs .ajax__tab_body 
{
    padding-top: 5px;    
    border:1px solid #0075bc;    
    padding-Bottom: 15px;
}
/* Cabecera */
.MyTabs .ajax__tab_header 
{   
    width: 300px; 
    margin-left: 5px;    
}

#ContenidoDetalle input
{
    float:right;
}

.ajax__tab_default .ajax__tab_outer {display:-moz-inline-box;display:inline-block}
.ajax__tab_default .ajax__tab_inner {display:-moz-inline-box;display:inline-block}
.ajax__tab_default .ajax__tab_tab {margin-right:4px;overflow:hidden;text-align:center;cursor:pointer;display:-moz-inline-box;display:inline-block;}

/* Cabecera pestaña */
.MyTabs .ajax__tab_outer 
{
    padding-right:3px;    
    height: 0px;    
    
}
/* Pestaña inactiva */
.MyTabs .ajax__tab_inner 
{     
    border: 1px solid #0075bc;
    background-image: url(/Graficos/Imagenes/tab_inactivo.png); 
    color: Black;   
}

.MyTabs .ajax__tab_inner a
{
    color:Black;
}

.MyTabs .ajax__tab_tab 
{
    height:18px;
    width: 124px;    
    margin:0;    
    border-bottom: none;        
}

.MyTabs .ajax__tab_active .ajax__tab_inner 
{    
    background-image: none;
    border-Bottom-color: White;    
}

.MyTabs .ajax__tab_active .ajax__tab_tab 
{   
    border-Bottom-color: White;
    color:#0075bc;
}

/* Alternativo */

.TabContainer
{
    margin-left: auto;
    margin-right: auto;
    height: 50px;   
}

.TabContainer .ajax__tab_body 
{
    padding-top: 5px;    
    border:1px solid #0075bc;    
    padding-Bottom: 15px;
}
/* Cabecera */
.TabContainer .ajax__tab_header 
{   
    width: 600px; 
    margin-left: 5px;    
}

.TabContainer .ajax__tab_inner 
{     
    border: 1px solid #0075bc;
    background-image: url(/Graficos/Imagenes/tab_inactivo.png); 
    color: Black;   
}

.TabContainer .ajax__tab_inner a
{
    color:Black;
}

.TabContainer .ajax__tab_tab 
{
    height:14px;
    width: 80px; 
    margin:0;    
    border-bottom: none;        
}
.TabContainer .ajax__tab_active .ajax__tab_inner 
{    
    background-image: none;
    border-Bottom-color: White;    
}
.TabContainer .ajax__tab_active .ajax__tab_tab 
{   
    border-Bottom-color: White;
    color:#0075bc;
}

.modal
{
    background-color: Gray;
    -ms-filter:"progid:DXImageTransform.Microsoft.Alpha(opacity=50)";
}        
.panelMensaje
{
    background-color: White;
    border: 1px solid #0075bc;
}
