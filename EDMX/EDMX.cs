using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using MyXmlDocument;

namespace EDMX
{
    public class objEdmx : MyXmlDocument.objXmlDoc
    {
        public XDocument xDoc;
        public StorageModel objStorageModel;
        public ConceptualModel objConceptualModel;
        public Mappings objMappings;
        private XNamespace strEdmxNs;
        private XNamespace strSchemaSmNs;
        private XNamespace strSchemaCsNs;
        private XNamespace strSchemaMpNs;
       
        public objEdmx(string path) : base(path)
        {
            this.strEdmxNs = XNamespace.Get(@"http://schemas.microsoft.com/ado/2008/10/edmx");
            this.strSchemaSmNs = XNamespace.Get(@"http://schemas.microsoft.com/ado/2009/02/edm/ssdl");
            this.strSchemaCsNs = XNamespace.Get(@"http://schemas.microsoft.com/ado/2008/09/edm");
            this.strSchemaMpNs = XNamespace.Get(@"http://schemas.microsoft.com/ado/2008/09/mapping/cs");
            this.objStorageModel = CreateStorageModel();
            this.objConceptualModel = CreateConceptualModel();
            this.objMappings = CreateMappings();
        }

        private StorageModel CreateStorageModel()
        {
            var model = GetModel("StorageModels");
            return new StorageModel(model, this.strEdmxNs, this.strSchemaSmNs);
        }
        private ConceptualModel CreateConceptualModel()
        {
            var model = GetModel("ConceptualModels");
            return new ConceptualModel(model, this.strEdmxNs, this.strSchemaCsNs);
        }
        private Mappings CreateMappings()
        {
            var model = GetModel("Mappings");
            return new Mappings(model, this.strEdmxNs, this.strSchemaMpNs);
        }

        private IEnumerable<XElement> GetModel(string strModel)
        {
            var query = from edmx in this.objXDoc.Elements(this.strEdmxNs + "Edmx")
                        from content in edmx.Elements(this.strEdmxNs + "Runtime")
                        from model in content.Elements(this.strEdmxNs + strModel)
                        //from schema in model.Elements(this.strSchemaNs + "Schema")
                        select model;

            return query;
        }
    }

    public abstract class EdmxObjects
    {
        public IEnumerable<XElement> GetSchema(IEnumerable<XElement> data, XNamespace nsEdmx, XNamespace nsSchema)
        {
            var query = from schema in data.Elements(nsSchema + "Schema")
                        select schema;
            return query;
        }

        public IEnumerable<XElement> GetEntityContainer(IEnumerable<XElement> data, XNamespace nsEdmx, XNamespace nsSchema)
        {
            var query = from schema in data.Elements(nsSchema + "Schema")
                        from e in schema.Elements(nsSchema + "EntityContainer")
                        select e;
            return query;
        }

        public IEnumerable<XElement> ParseEntityContainer(string tag, IEnumerable<XElement> data, XNamespace nsSchema)
        {
            var query = from entitysets in data.Elements(nsSchema + tag)
                        select entitysets;
            return query;
        }

        public IEnumerable<XElement> ParseSchema(string tag, IEnumerable<XElement> data, XNamespace nsSchema)
        {
            var query = from e in data.Elements(nsSchema + tag)
                        select e;
            return query;
        }

        public IEnumerable<XElement> GetEntityContainerMapping(IEnumerable<XElement> data, XNamespace nsEdmx, XNamespace nsSchema)
        {
            var query = from schema in data.Elements(nsSchema + "Mapping")
                        from e in schema.Elements(nsSchema + "EntityContainerMapping")
                        select e;
            return query;
        }
    }

    public abstract class Model : EdmxObjects
    {
        public EntityContainer EntityContainer;
        public IEnumerable<XElement> EntityTypes;
        public IEnumerable<XElement> Associations;
        public IEnumerable<XElement> model;
        public XNamespace nsEdmx;
        public XNamespace nsSchema;
    }

    public class StorageModel : Model
    {        
        IEnumerable<XElement> Functions;
        
        public StorageModel(IEnumerable<XElement> model, XNamespace nsEdmx, XNamespace nsSchema)
        {
            var schema = GetSchema(model, nsEdmx, nsSchema);
            this.EntityContainer = new EntityContainer(model, nsEdmx, nsSchema);
            this.model = model;
            this.nsEdmx = nsEdmx;
            this.nsSchema = nsSchema;
            this.EntityTypes = ParseSchema("EntityType", schema, nsSchema);
            this.Associations = ParseSchema("Association", schema, nsSchema);
            this.Functions = ParseSchema("Function", schema, nsSchema);
        }
    }

    public class ConceptualModel : Model
    {

        public ConceptualModel(IEnumerable<XElement> model, XNamespace nsEdmx, XNamespace nsSchema)
        {
            var schema = GetSchema(model, nsEdmx, nsSchema);
            this.EntityContainer = new EntityContainer(model, nsEdmx, nsSchema);
            this.model = model;
            this.nsEdmx = nsEdmx;
            this.nsSchema = nsSchema;
            this.EntityTypes = ParseSchema("EntityType", schema, nsSchema);
            this.Associations = ParseSchema("Association", schema, nsSchema);
        }
    }

    public class Mappings : Model
    {
        public EntityContainerMapping EntityContainerMapping;
        public Mappings(IEnumerable<XElement> model, XNamespace nsEdmx, XNamespace nsSchema)
        {
            this.model = model;
            this.nsEdmx = nsEdmx;
            this.nsSchema = nsSchema;
            var schema = GetEntityContainerMapping(model, nsEdmx, nsSchema);
            this.EntityContainerMapping = new EntityContainerMapping(schema, nsEdmx, nsSchema);
        }
    }

    public class EntityContainer : EdmxObjects
    {
        string Name;
        IEnumerable<XElement> EntitySets;
        IEnumerable<XElement> AssociationSets;

        public EntityContainer(IEnumerable<XElement> model, XNamespace nsEdmx, XNamespace nsSchema)
        {
            var container = this.GetEntityContainer(model, nsEdmx, nsSchema);
            var n = from schema in model.Elements(nsSchema + "Schema")
                    from e in schema.Elements(nsSchema + "EntityContainer")
                    select e.Attribute("Name");
            this.Name = n.ToArray()[0].Value;

            this.EntitySets = ParseEntityContainer("EntitySet", container, nsSchema);
            this.AssociationSets = ParseEntityContainer("AssociationSet", container, nsSchema);
        }
    }

    public class EntityContainerMapping : EdmxObjects
    {
        string StorageEntityContainer; //TODO
        string CdmEntityContainer; //TODO
        EntitySetMapping EntitySetMappings;
        IEnumerable<XElement> FunctionImportMapping;

        public EntityContainerMapping(IEnumerable<XElement> schema, XNamespace nsEdmx, XNamespace nsSchema)
        {
            this.EntitySetMappings = new EntitySetMapping(schema, nsEdmx, nsSchema);
            this.FunctionImportMapping = ParseEntityContainer("FunctionImportMapping", schema, nsSchema);
        }
    }

    public class EntitySetMapping
    {
        string Name; //TODO
        EntityTypeMapping objEntityContainerMappings;
        private IEnumerable<XElement> schema;
        private XNamespace nsEdmx;
        private XNamespace nsSchema;

        public EntitySetMapping(IEnumerable<XElement> schema, XNamespace nsEdmx, XNamespace nsSchema)
        {
            // TODO: Complete member initialization
            this.schema = schema;
            this.nsEdmx = nsEdmx;
            this.nsSchema = nsSchema;
        }
    }

    public class EntityTypeMapping
    {
        string TypeName; //TODO
        MappingFragment MappingFragment;
        ModificationFunctionMapping ModificationFunctionMapping;
    }

    public class MappingFragment
    {
        string StoreEntitySet; //TODO
        IEnumerable<XElement> ScalarProperty;
    }

    public class ModificationFunctionMapping //TODO
    {
        UpdateFunction UpdateFunction;
        InsertFunction InsertFunction;
    }

    public class UpdateFunction //TODO
    {
        string FunctionName;
        string RowsAffectedParameter;
        IEnumerable<XElement> ScalarProperty;
    }

    public class InsertFunction //TODO
    {
        string FunctionName;
        string RowsAffectedParameter;
        IEnumerable<XElement> ScalarProperty;
    }


    /*
    public class EntitySet
    {
        string Name;
        string EntityType;
        string Type;
        string Schema;
    }

    public class EntityType
    {
        string Key;
        Property[] objProperties;
        NavigationProperty[] objNavigationProperties;
    }

    public class NavigationProperty
    {
        string Name;
        string Relationship;
        string FromRole;
        string ToRole;
    }

    public class Property
    {
        string Name;
        string Type;
        bool Nullable;
        int MaxLength;
    }

    public class Association
    {
        End From;
        End To;
    }

    public class End
    {
        string Role;
        string Type;
        string Multiplicity;
    }

    public class Function
    {
        string Name;
        bool Aggregate;
        bool BuiltIn;
        bool NiladicFunction;
        bool IsCompasable;
        string ParameterTypeSemantics;
        string Schema;
        Parameter[] objParameters;
    }

    public class Parameter
    {
        string Name;
        string Mode;
        string Type;
    }

    public class Mapping
    {
        EntityContainerMapping objEntityContainerMapping;
    }

    public class EntityContainerMapping
    {
        string StorageEntityContainer;
        string CdmEntityContainer;
        EntitySetMapping[] objEntitySetMapping;
        FunctionImportMapping[] objFunctionImportMapping;
    }

    public class EntitySetMapping
    {
        string Name;
        EntityTypeMapping[] objEntityContainerMappings;
    }

    public class EntityTypeMapping
    {
        string TypeName;
        MappingFragment[] objMappingFragments;
        ModificationFunctionMapping[] objMappingFragment;
    }

    public class MappingFragment
    {
        string StoreEntitySet;
        ScalarProperty[] objScalarProperties;
    }

    public class ScalarProperty
    {
        string Name;
        string ColumnName;
        string ParameterName;
        string Version;
    }

    public class ModificationFunctionMapping
    {
        UpdateFunction objUpdateFunction;
        InsertFunction objInsertFunction;
    }

    public class UpdateFunction
    {
        string FunctionName;
        string RowsAffectedParameter;
        ScalarProperty[] objScalarProperty;
    }

    public class InsertFunction
    {
        string FunctionName;
        string RowsAffectedParameter;
        ScalarProperty[] objScalarProperty;
    }

    public class FunctionImportMapping
    {
        string FunctionImportName;
        string FunctionName;
    }
    */
}

