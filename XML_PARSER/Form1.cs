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
using System.Xml.XPath;
using System.Xml.Serialization;
using SerializeEDMX;
using System.IO;

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
            string path;
            //path = @"D:\My Data\Github Repo\XML_PARSER\XML_PARSER\Sample.xml";
            path = @"D:\My Data\Github Repo\Temp\Core.edmx";

            //objEdmx edmx = new objEdmx(path);

            SerializeEDMX.EdmxWrapper edmx = new EdmxWrapper(path);
            var r = edmx.GetFunctionImportMapping();
        }
    }
}

/*
           //var query = from department in doc.Elements("edmx:Edmx").Elements("edmx:Runtime")
                       from item in department.Elements("edmx:StorageModels")
                       select item;
                       //select new Item
                       //{
                       //    DepartmentName = department.Attribute("Name").Value,
                       //    DepartmentNames = department.Attributes().Where(a => a.Name != "Name").ToDictionary(a => a.Name.LocalName, a => a.Value),
                       //    Name = item.Attribute("Name").Value,
                       //    Names = item.Attributes().Where(a => a.Name != "Name").ToDictionary(a => a.Name.LocalName, a => a.Value),
                       //};
            
           var items = query.ToList();
           */