using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;
using System.Xml;

namespace XmlDocument
{
    public abstract class XmlDocument
    {
        public XNamespace strNameSpace;
        public XDocument xDoc;

        public XmlDocument(String strFilename)
        {
            this.xDoc = XDocument.Load(strFilename);
            this.strNameSpace = null;
        }

        public XmlDocument(String strFilename, String strNameSpace)
        {
            this.xDoc = XDocument.Load(strFilename);
            XNamespace xnEdmxNameSpace = "http://www.w3.org/2000/xmlns/";
            this.strNameSpace = xDoc.Root.Attribute(xnEdmxNameSpace + strNameSpace).Value;
        }

        public IEnumerable<System.Xml.Linq.XElement> GetNodes(String tag)
        {
            IEnumerable<System.Xml.Linq.XElement> objNodeList;
            objNodeList = this.xDoc.Descendants(this.strNameSpace + tag);
            return objNodeList;
        }
    }
}
