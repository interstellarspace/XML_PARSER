using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using SerializeEDMX;

namespace SerializeEDMX
{
    public class EdmxWrapper
    {
        public Edmx objEdmx;

        public EdmxWrapper(string fullFileName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Edmx));
            TextReader reader = new StreamReader(fullFileName);
            object obj = deserializer.Deserialize(reader);
            this.objEdmx = (Edmx)obj;
            reader.Close();
        }

        public Object GetSchema(string model)
        {
            if (model == models.StorageModel)
            {
                return this.objEdmx.Runtime.StorageModels.Schema;
            }
            else if (model == models.ConceptualModel)
            {
                return this.objEdmx.Runtime.ConceptualModels.Schema;
            }
            else
            {
                return this.objEdmx.Runtime.Mappings.Mapping;
            }
        }

        public Object GetContainer(string model)
        {
            var temp = GetSchema(model);
            if (model == models.StorageModel)
            {
                return ((Schema)temp).EntityContainer;
            }
            else if (model == models.ConceptualModel)
            {
                return ((Schema)temp).EntityContainer;
            }
            else
            {
                return ((Mapping)temp).EntityContainerMapping;
            }
        }

        public object GetEntityType(string model)
        {
            if (model == models.Mapping)
            {
                throw new Exception("EntityContainerMapping has no EntityType");
            }

            var temp = GetSchema(model);
            if (model == models.StorageModel)
            {
                return ((Schema)temp).EntityType;
            }
            else if (model == models.ConceptualModel)
            {
                return ((Schema)temp).EntityType;
            }
            else
            {
                throw new Exception("EntityContainerMapping has no EntityType");
            }
        }

        public object GetAssociation(string model)
        {
            if (model == models.Mapping)
            {
                throw new Exception("EntityContainerMapping has no Association");
            }

            var temp = GetSchema(model);
            if (model == models.StorageModel)
            {
                return ((Schema)temp).Association;
            }
            else if (model == models.ConceptualModel)
            {
                return ((Schema)temp).Association;
            }
            else
            {
                throw new Exception("EntityContainerMapping has no Association");
            }
        }

        public object GetFunction(string model)
        {
            if (model == models.Mapping || model == models.ConceptualModel)
            {
                throw new Exception("Object has no Association");
            }

            var temp = GetSchema(model);
            if (model == models.StorageModel)
            {
                return ((Schema)temp).Function;
            }
            else
            {
                throw new Exception("Object has no Association");
            }
        }

        public MappingEntityContainerMappingEntitySetMapping[] GetEntitySetMapping()
        {
            return ((MappingEntityContainerMapping)GetContainer(models.Mapping)).EntitySetMapping;
        }

        public MappingEntityContainerMappingFunctionImportMapping[] GetFunctionImportMapping()
        {
            return ((MappingEntityContainerMapping)GetContainer(models.Mapping)).FunctionImportMapping;
        }
    }

    public struct models
    {
        public static string StorageModel = "StorageModel";
        public static string ConceptualModel = "ConceptualModel";
        public static string Mapping = "Mapping";
    }
}
