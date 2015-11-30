using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MyXmlDocument
{
    public class XmlEdmx : MyXmlDocument
    {
        public XmlNode ieStorageModels;
        public XmlNode ieConceptualModels;
        public XmlNode ieMappings;

        public XmlEdmx(String strFilename) : base(strFilename) 
        { 
            InitRuntime();
        }

        private void InitRuntime()
        {
            ieStorageModels = InitModels(1);
            ieConceptualModels = InitModels(3);
            ieMappings = InitModels(5);
        }

        private XmlNode InitModels(int num)
        {
            XmlNode list = this.xDoc.ChildNodes[1].ChildNodes[1].ChildNodes[num];
            return list;
        }


    }
}
