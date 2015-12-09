using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;
using System.Xml;

namespace MyXmlDocument
{
    public abstract class objXmlDoc
    {
        public XDocument objXDoc;

        public objXmlDoc(String strFilename)
        {
            this.objXDoc = XDocument.Load(strFilename);
        }
    }
}
