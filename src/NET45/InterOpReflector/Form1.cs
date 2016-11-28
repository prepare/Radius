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
            List<Type> typeMembers = ns.members;
            int j = typeMembers.Count;
            for (int i = 0; i < j; ++i)
            {
                RebuildType(typeMembers[i]);
            }
        }
        static void RebuildType(Type t)
        {
            if (!t.IsPublic)
            {

                return;
            }

            //check if this is interface or enum
            if (t.IsEnum)
            {

            }
            else if (t.IsInterface)
            {

            }
            else
            {
                //check if  this is struct
                //delegate or class
                if (t.IsValueType)
                {

                }
                else if (t.BaseType.Name.StartsWith("System.MultiCast"))
                {

                }
                else
                {

                }
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

    }
}
