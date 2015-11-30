using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using MyXmlDocument;
using System.Xml.XPath;

namespace XML_PARSER
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MyXmlDocument.XmlEdmx xDoc;
            xDoc = new MyXmlDocument.XmlEdmx(@"D:\My Data\Github Repo\XML_PARSER\Core.edmx");
        }
    }
}
