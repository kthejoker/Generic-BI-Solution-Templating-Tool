/*
 * Created by SharpDevelop.
 * User: Kyle
 * Date: 03/31/2012
 * Time: 19:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace WpfApplication1.Mappings
{
	/// <summary>
	/// Description of LOOKUPCOLUMN.
	/// </summary>
	public class LOOKUPCOLUMN
	{
		
				[System.Xml.Serialization.XmlAttribute("NATURALKEYCOLUMN")]
        public string NATURALKEYCOLUMN { get; set; }

        [System.Xml.Serialization.XmlAttribute("DATACOLUMN")]
        public string DATACOLUMN { get; set; }
		
		public LOOKUPCOLUMN()
		{
		}
	}
}
