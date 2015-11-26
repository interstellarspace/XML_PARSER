using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlDocument
{
    public class XmlEdmx : XmlDocument
    {
        public IEnumerable<XElement> ieStorageModels;
        public IEnumerable<XElement> ieConceptualModels;
        public IEnumerable<XElement> ieMappings;

        public XmlEdmx(String strFilename) : base(strFilename) 
        { 
            InitRuntime();
        }

        public XmlEdmx(String strFilename, String strNameSpace) : base(strFilename, strNameSpace) 
        { 
            InitRuntime();
        }

        private void InitRuntime()
        {
            ieStorageModels = InitModels("StorageModels");
            ieConceptualModels = InitModels("ConceptualModels");
            ieMappings = InitModels("Mappings");
        }

        private IEnumerable<XElement> InitModels(String model)
        {
            var result = GetNodes(model).Descendants();
            return result;
        }


    }
}
