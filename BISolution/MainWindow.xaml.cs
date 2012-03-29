using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WpfApplication1.Tier;
using WpfApplication1.DataSource;
using WpfApplication1.Package;
using WpfApplication1.Cube;
using WpfApplication1.Static;
using WpfApplication1.Mappings;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public class BISolution
    {
        public string solutionFile;
        public XmlReader solutionXML;
        public SOLUTION SOLUTION;

        public int deployTier(string tierToDeploy) {
            //foreach object
                       // insert dataset if new
            //insert all meta table if new

            TIER t = this.SOLUTION.getTier(tierToDeploy);
            //foreach (DATASOURCE ds in this.SOLUTION.DATASOURCES)
            //{
            //    foreach (DATAOBJECT d in ds.DATAOBJECTS)
            //    {

            //        //TODO some sort of compare to see if we even need to re-run this
            //        d.createMatchTable(true);
            //        d.createPSATable(true);
            //        d.createStageTable(true);

            //        PACKAGE_STAGE Package_Stage = new PACKAGE_STAGE(d);
            //        PACKAGE_PSA Package_PSA = new PACKAGE_PSA(d);
            //    }

            //}

            foreach (DIMENSION dim in this.SOLUTION.CUBE.DIMENSIONS)
            {
               dim.createTable(dim.DATABASELAYER, true);
               PACKAGE_DIM Package_DIM = new PACKAGE_DIM(dim);
               //TODO add package to project
               
            }
            
            //TODO create fact table if does not exist
            MAPPING[] factMappings = Array.FindAll(this.SOLUTION.MAPPINGS, delegate(MAPPING tempMapping) {
                                            return tempMapping.TYPE == "FACT";
                                                });
         
            foreach (MAPPING factMapping in factMappings) {
            
            
            PACKAGE_FACT Package_FACT = new PACKAGE_FACT(factMapping);
            }

        return 0;
        }

        public BISolution(string solutionFile)
        {
            this.solutionFile = solutionFile;
            //TODO try catch valid XML file
            this.solutionXML = XmlReader.Create(solutionFile);

            XmlSerializer ser = new XmlSerializer(typeof(SOLUTION));
           
                this.SOLUTION = (SOLUTION)ser.Deserialize(this.solutionXML);
               
          

        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            BISolution solution = new BISolution(@"BISolution.xml");
            solution.deployTier("DEV");   
           
        }
    }


   

   

}
