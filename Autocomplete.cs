
private static List<KeyValuePair<string, string>> Clientes;

[System.Web.Services.WebMethod]
[System.Web.Script.Services.ScriptMethod]
public static string[] CargarClientes(string prefixText, int count)
{
	List<string> Lista_Clientes = new List<string>();
	var nuevos = Clientes.Where(x => x.Key.ToUpperInvariant().Contains(prefixText.ToUpperInvariant())).Take(5);
	foreach (KeyValuePair<string, string> item in nuevos)
	{
		Lista_Clientes.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(item.Key, item.Value));
	}
	return Lista_Clientes.ToArray();
}        

protected void BtnBusqueda_Click(object sender, ImageClickEventArgs e)
{	
	string entrada = TxtValor.Text;
	if (entrada != "")
	{
		int id = Convert.ToInt32(entrada);
		// aqui tienes el id del cliente
	}
}

protected void TxtValor_TextChanged(object sender, EventArgs e)
{            
	BtnBusqueda_Click(sender, null);
}
