/*
 * Created by SharpDevelop.
 * User: Kyle
 * Date: 03/31/2012
 * Time: 19:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WpfApplication1.Mappings
{
	/// <summary>
	/// Description of LOOKUP.
	/// </summary>
	public class LOOKUP
	{
		
		//TODO get dimension object by name? (why?)
		
		[System.Xml.Serialization.XmlAttribute("NAME")]
        public string NAME { get; set; }

        [System.Xml.Serialization.XmlAttribute("DIMENSION")]
        public string DIMENSION { get; set; }
        
          [System.Xml.Serialization.XmlArrayItemAttribute("LOOKUPCOLUMN", typeof(LOOKUPCOLUMN), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public LOOKUPCOLUMN[] LOOKUPCOLUMNS
        {
		
		public LOOKUP()
		{
		}
	}
}
