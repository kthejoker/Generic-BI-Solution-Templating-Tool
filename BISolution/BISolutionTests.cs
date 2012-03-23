/*
 * Created by SharpDevelop.
 * User: Kyle
 * Date: 03/20/2012
 * Time: 16:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;


namespace WpfApplication1
{
	/// <summary>
	/// NUnit testing class for BI Solutions project.
	/// </summary>
	///
	[TestFixture]
	public class CONFIGURATIONTests
	{
		
		[Test] public void MyTest() {
		
		Configuration.CONFIGURATION Configuration = new Configuration.CONFIGURATION();
		Assert.IsNull(Configuration.TIERS);
		
		}
		
		//TODO create folder for Tests
		//TODO figure out how to mock in snippets of XML file for testing
		//TODO Get Sandcastle Help Builder thing
		

	}
}
