/*
 * Created by SharpDevelop.
 * User: Kyle
 * Date: 3/27/2012
 * Time: 12:15 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using WpfApplication1.Cube;


namespace WpfApplication1.Tests
{
	/// <summary>
	/// Test fixture for DIMENSION class.
	/// </summary>

	using System;
	using NUnit.Framework;
	using System.Xml;
	using System.Xml.Serialization;
	
	

	[TestFixture]
	public class DIMENSIONTests
	{
		public SOLUTION s;
		public DIMENSION dimension;
		
		[TestFixtureSetUp] public void DimensionCreation() {
			string solutionFile = @"BISolution.xml";
			BISolution solution = new BISolution(solutionFile);
            XmlReader solutionXML = XmlReader.Create(solutionFile);
            XmlSerializer ser = new XmlSerializer(typeof(SOLUTION));
           	this.s = (SOLUTION)ser.Deserialize(solutionXML);
           	this.dimension = this.s.CUBE.getDimensionByName("USER");
			
		}
		
		[Test] public void CheckName() {
			Assert.AreEqual(this.dimension.NAME, "USER");
		}
		
		//TODO Dimension tests
		
		[SetUp] public void DimensionSetup() {
			
		}

	}
}
