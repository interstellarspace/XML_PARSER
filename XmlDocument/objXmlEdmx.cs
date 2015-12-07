using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;

namespace MyXmlDocument
{
    public class objXmlEdmx : objXmlDoc
    {
        //public EDMX.objEdmx Edmx;
        private XNamespace strEdmxNs;
        private XNamespace strSchemaNs;

        public objXmlEdmx(String strFilename) : base(strFilename) 
        {
            this.strEdmxNs = XNamespace.Get(ConfigurationManager.AppSettings["EdmxNs"]);
            this.strSchemaNs = XNamespace.Get(ConfigurationManager.AppSettings["EdmxSchemaNs"]);
            InitRuntime();
        }

        private void InitRuntime()
        {
            CreateEdmx();
        }

        private void CreateEdmx()
        {
            
        }


        private void GetModelSchema(string strModel)
        {
            var query = from edmx in this.objXDoc.Elements(this.strEdmxNs + "Edmx")
                        from content in edmx.Elements(this.strEdmxNs + "Runtime")
                        from model in content.Elements(this.strEdmxNs + strModel)
                        from schema in model.Elements(this.strSchemaNs + "Schema")
                        select schema;

            IEnumerable<XElement> q = query.AsEnumerable();

            var q2 = from entitytype in q.Elements(this.strSchemaNs + "EntityType")
                     select entitytype.Attributes();
        }
    }
}
