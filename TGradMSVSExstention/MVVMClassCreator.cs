using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TGradMSVSExtention.Properties;
using System.IO;
using EnvDTE;
using System.Windows;

namespace TGradMSVSExtention
{
    enum MVVMClassType
    {
        Model = 0, View = 1, ViewModel = 2
    }

    static class MVVMClassCreator
    {

        static private DTE dte;

        static public void CreateClasses(List<MVVMClassType> types, string className, List<string> templateFileNames)
        {
            var ps = ((Array)dte.ActiveSolutionProjects).Cast<Project>().ToArray();
            List<string> prefixes = FindPrefixes(types);
            var typedProjects = GetTypedProjects(prefixes, types);
            foreach (var project in typedProjects.Keys)
            {
                CreateClass(typedProjects[project], className, templateFileNames[types.IndexOf(typedProjects[project])], project);
            }
        }

        static private List<string> FindPrefixes(List<MVVMClassType> types)
        {
            Array tmp = (Array)dte.ActiveSolutionProjects;
            var ps = tmp.Cast<Project>().ToArray();
            List<string> prefixes = new List<string>();
            foreach (var p in ps)
            {
                string prefix = p.Name.Substring(0, p.Name.LastIndexOf("."));
                if (prefix != null && prefix != "" && !prefixes.Contains(prefix))
                    prefixes.Add(prefix);
            }
            return prefixes;
        }
        
        static private Dictionary<Project, MVVMClassType> GetTypedProjects(List<string> prefixes, List<MVVMClassType> types)
        {
            var typedProjects = new Dictionary<Project, MVVMClassType>();
            var it = dte.Solution.Projects.GetEnumerator();
            while (it.MoveNext())
            {
                var p = it.Current as Project;
                foreach (var prefix in prefixes)
                {
                    string lcpname = p.Name.ToLower();
                    if (p.Name.StartsWith(prefix))
                    {
                        foreach (var t in types)
                        {
                            string lctname = t.ToString().ToLower();
                            if (lcpname.Contains(lctname))
                            {
                                if (lctname != "viewmodel")
                                {
                                    if (!lcpname.Contains("viewmodel"))
                                    {
                                        typedProjects.Add(p, t);
                                        break;
                                    }
                                }
                                else
                                {
                                    typedProjects.Add(p, t);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return typedProjects;
        }

        static private void CreateClass(MVVMClassType type, string className, string templateFileName, Project project)
        {
            string typename = type.ToString();
            string cs = "";
            if (templateFileName == "Default")
            {
                cs = Settings.Default[templateFileName + typename].ToString();
            }
            else
            {
                cs = File.ReadAllText(Settings.Default[typename + "Folder"].ToString() + "\\" + templateFileName);
            }
            cs = cs.Replace("%namespace%", $"{project.Name}.{className}s");
            cs = cs.Replace("%classname%", className);
            project.ProjectItems.AddFolder(className + "s");
            string fileName = $@"{Path.GetDirectoryName(project.FullName)}\{className}s\{className}.cs";
            File.WriteAllText(fileName, cs);
            project.ProjectItems.AddFromFile(fileName);
        }

        static public void InitializeDTE(DTE dte)
        {
            MVVMClassCreator.dte = dte;
        }

    }
}
