using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Configuration;

namespace GRM.UTL
{
    public class XMLUtil
    {
        public static string ConvertDataTableToXML(DataTable dt)
        {

            string strResult = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                strResult += "<row>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strResult += string.Format("<{0}>{1}</{0}>", dt.Columns[i].ColumnName.Replace(" ", "")
                        , dr[i].ToString().Replace(">", "&gt;").Replace("<", "&lt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("&", "&amp;"));
                }
                strResult += "</row>";
            }

            return strResult;
            /*
DECLARE @xml XML
SET @xml = '
<row>
    <IdInvernadero>8</IdInvernadero>
    <IdProducto>3</IdProducto>
    <IdCaracteristica1>8</IdCaracteristica1>
    <IdCaracteristica2>8</IdCaracteristica2>
    <Cantidad>25</Cantidad>
    <Folio>4568457</Folio>
</row>
<row>
    <IdInvernadero>3</IdInvernadero>
    <IdProducto>3</IdProducto>
    <IdCaracteristica1>1</IdCaracteristica1>
    <IdCaracteristica2>2</IdCaracteristica2>
    <Cantidad>72</Cantidad>
    <Folio>4568457</Folio>
</row>
'
SELECT  
       Tbl.Col.value('IdInvernadero[1]', 'smallint'),  
       Tbl.Col.value('IdProducto[1]', 'smallint'),  
       Tbl.Col.value('IdCaracteristica1[1]', 'smallint'),
       Tbl.Col.value('IdCaracteristica2[1]', 'smallint'),
       Tbl.Col.value('Cantidad[1]', 'int'),
       Tbl.Col.value('Folio[1]', 'varchar(7)')
FROM   @xml.nodes('//row') Tbl(Col)  
             
             */
        }
        public static string GetConneectionString(string strConnectionNode, string strXMLPath, string strNode)
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings[strConnectionNode].ConnectionString;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(System.IO.File.ReadAllText(strXMLPath)); // suppose that myXmlString contains "<Names>...</Names>"
            XmlNodeList xnList = xml.SelectNodes(strNode);
            //foreach (XmlNode xn in xnList)
            //{
            //    string firstName = xn["FirstName"].InnerText;
            //    string lastName = xn["LastName"].InnerText;
            //    Console.WriteLine("Name: {0} {1}", firstName, lastName);
            //}
            XmlNode xn = xnList[0];
            strConnectionString = strConnectionString.Replace("[server]", xn["server"].InnerText);
            strConnectionString = strConnectionString.Replace("[username]", xn["username"].InnerText);
            strConnectionString = strConnectionString.Replace("[password]", Crypto.DecryptStringAES(xn["password"].InnerText, "Groupm#01"));
            return strConnectionString;
        }
    }
}