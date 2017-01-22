using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace InterOpReflector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //very simple api reflection
            //we extract only essential interfaces, 
            Assembly asm = typeof(AngleSharp.BrowsingContext).Assembly;
            Type[] allTypes = asm.GetTypes();
            //group by its namespace
            int j = allTypes.Length;
            Dictionary<string, Namespace> namespaces = new Dictionary<string, Namespace>();
            for (int i = 0; i < j; ++i)
            {
                Type t = allTypes[i];
                if (t.Namespace == null) { continue; }
                if (t.Name.StartsWith("<")) { continue; }

                Namespace found;
                if (!namespaces.TryGetValue(t.Namespace, out found))
                {
                    found = new InterOpReflector.Form1.Namespace(t.Namespace);
                    namespaces.Add(t.Namespace, found);
                }
                found.AddTypeMember(t);
            }

            //test some namespace
            Namespace selectedNs;
            if (namespaces.TryGetValue("AngleSharp.Dom", out selectedNs))
            {
                RebuildTypes(selectedNs);
            }
        }

        static void RebuildTypes(Namespace ns)
        {
            List<TypeDefinition> typedefList = new List<TypeDefinition>();

            List<Type> typeMembers = ns.members;
            int j = typeMembers.Count;
            for (int i = 0; i < j; ++i)
            {
                TypeDefinition t = RebuildType(typeMembers[i]);
                if (t != null)
                {
                    typedefList.Add(t);
                }
            }
        }
        static TypeDefinition RebuildType(Type t)
        {
            if (!t.IsPublic)
            {

                return null;
            }
            TypeDefinition typedef = null;
            //check if this is interface or enum
            if (t.IsEnum)
            {
                typedef = new TypeDefinition(TypeKind.Enum, t.Name);
            }
            else if (t.IsInterface)
            {
                typedef = new InterOpReflector.Form1.TypeDefinition(TypeKind.Interface, t.Name);
            }
            else
            {
                //check if  this is struct
                //delegate or class
                if (t.IsValueType)
                {
                    typedef = new InterOpReflector.Form1.TypeDefinition(TypeKind.Struct, t.Name);
                }
                else if (t.BaseType.Name.StartsWith("System.MultiCast"))
                {
                    typedef = new InterOpReflector.Form1.TypeDefinition(TypeKind.Delegate, t.Name);
                }
                else
                {
                    typedef = new InterOpReflector.Form1.TypeDefinition(TypeKind.Class, t.Name);
                }
            }
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly;
            ConstructorInfo[] allCtors = t.GetConstructors(bindingFlags);
            MethodInfo[] allMethods = t.GetMethods(bindingFlags);
            PropertyInfo[] allProps = t.GetProperties(bindingFlags);
            FieldInfo[] allFields = t.GetFields(bindingFlags);
            EventInfo[] allEvents = t.GetEvents(bindingFlags);
            //--------
            //show only methods/props/events/fields 
            //this version not include ctor, subtype ***

            BuildMethods(allMethods, typedef);
            BuildProps(allProps, typedef);
            BuildEvents(allEvents, typedef);
            BuildFields(allFields, typedef);
            //--------
            return typedef;
        }

        static void BuildMethods(MethodInfo[] mbs, TypeDefinition typedef)
        {
            int j = mbs.Length;
            for (int i = 0; i < j; ++i)
            {
                MethodInfo m = mbs[i];
                MethodDefinition metdef = new InterOpReflector.Form1.MethodDefinition(m.Name, m);
                typedef.AddTypeMember(metdef);
            }
        }
        static void BuildProps(PropertyInfo[] mbs, TypeDefinition typedef)
        {
            int j = mbs.Length;
            for (int i = 0; i < j; ++i)
            {
                PropertyInfo m = mbs[i];
                PropertyDefinition propdef = new InterOpReflector.Form1.PropertyDefinition(m.Name, m);
                typedef.AddTypeMember(propdef);
            }
        }
        static void BuildEvents(EventInfo[] mbs, TypeDefinition typedef)
        {
            int j = mbs.Length;
            for (int i = 0; i < j; ++i)
            {
                EventInfo m = mbs[i];
                EventDefinition eventdef = new EventDefinition(m.Name, m);
                typedef.AddTypeMember(eventdef);
            }
        }
        static void BuildFields(FieldInfo[] mbs, TypeDefinition typedef)
        {
            int j = mbs.Length;
            for (int i = 0; i < j; ++i)
            {
                FieldInfo m = mbs[i];
                FieldDefinition fielddef = new FieldDefinition(m.Name,m);
                typedef.AddTypeMember(fielddef);
            }
        }

        class Namespace
        {
            internal List<Type> members = new List<Type>();
            public Namespace(string name)
            {
                this.Name = name;
            }
            public void AddTypeMember(Type t)
            {
                members.Add(t);
            }
            public string Name { get; set; }
        }

        enum TypeKind
        {
            Class,
            Struct,
            Interface,
            Enum,
            Delegate
        }


        class TypeReference
        {
            public TypeReference(string typename)
            {
                this.TypeName = typename;
            }
            public string TypeName { get; private set; }
            public override string ToString()
            {
                return this.TypeName;
            }
        }
        class TypeDefinition : TypeMember
        {
            List<TypeMember> membes = new List<TypeMember>();
            public TypeDefinition(TypeKind kind, string name)
            {
                this.Name = name;
                this.Kind = kind;
            }
            public void AddTypeMember(TypeMember mb)
            {
                membes.Add(mb);
            }
            public TypeKind Kind { get; private set; }
            public override string ToString()
            {
                switch (this.Kind)
                {
                    case TypeKind.Class:
                        return "class " + this.Name;
                    case TypeKind.Struct:
                        return "struct" + this.Name;
                    case TypeKind.Delegate:
                        return "delegate " + this.Name;
                    case TypeKind.Interface:
                        return "interface " + this.Name;
                    case TypeKind.Enum:
                        return "enum " + this.Name;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
        abstract class TypeMember
        {
            public string Name { get; set; }
        }
        class MethodDefinition : TypeMember
        {
            MethodInfo orgMethodInfo;
            public MethodDefinition(string name, MethodInfo orgMethodInfo)
            {
                this.Name = name;
                this.orgMethodInfo = orgMethodInfo;
            }
            public override string ToString()
            {
                return orgMethodInfo.ToString();
            }
        }
        class PropertyDefinition : TypeMember
        {
            PropertyInfo orgPropInfo;
            public PropertyDefinition(string name, PropertyInfo orgPropInfo)
            {
                this.Name = name;
                this.orgPropInfo = orgPropInfo;
            }
            public override string ToString()
            {
                return orgPropInfo.ToString();
            }
        }
        class EventDefinition : TypeMember
        {
            EventInfo orgEventInfo;
            public EventDefinition(string name, EventInfo orgEventInfo)
            {
                this.Name = name;
                this.orgEventInfo = orgEventInfo;
            }
            public override string ToString()
            {
                return orgEventInfo.ToString();
            }
        }
        class FieldDefinition : TypeMember
        {
            FieldInfo orgFieldInfo;
            public FieldDefinition(string name,FieldInfo orgFieldInfo)
            {
                this.Name = name;
                this.orgFieldInfo = orgFieldInfo;
            }
            public override string ToString()
            {
                return orgFieldInfo.ToString();
            }
        }

    }
}
