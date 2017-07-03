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
            Array tmp = (Array)dte.ActiveSolutionProjects;
            var ps = tmp.Cast<Project>().ToArray();
            List<string> prefixes = FindPrefixes(types);
            List<Project> projects;
            List<MVVMClassType> projectTypes;
            GetProjectsAndTypes(prefixes, types, out projects, out projectTypes);
            for (int i = 0; i < projects.Count; ++i)
            {
                CreateClass(projectTypes[i], className, templateFileNames[types.IndexOf(projectTypes[i])], projects[i]);
            }

        }

        static private List<string> FindPrefixes(List<MVVMClassType> types)
        {
            Array tmp = (Array)dte.ActiveSolutionProjects;
            var ps = tmp.Cast<Project>().ToArray();
            List<string> prefixes = new List<string>();
            foreach (var p in ps)
            {
                string lcpname = p.Name.ToLower();
                foreach (var t in types)
                {
                    string lctname = t.ToString().ToLower();
                    if (lcpname.Contains(lctname))
                    {
                        if (lctname != "viewmodel")
                        {
                            if (!lcpname.Contains("viewmodel"))
                            {
                                string prefix = p.Name.Substring(0, lcpname.IndexOf(lctname));
                                if (!prefixes.Contains(prefix))
                                    prefixes.Add(prefix);
                                break;
                            }
                        }
                        else
                        {
                            string prefix = p.Name.Substring(0, lcpname.IndexOf(lctname));
                            if (!prefixes.Contains(prefix))
                                prefixes.Add(prefix);
                            break;
                        }
                    }
                }
            }
            return prefixes;
        }
        
        static private void GetProjectsAndTypes(List<string> prefixes, List<MVVMClassType> types, out List<Project> projects,
            out List<MVVMClassType> projectTypes)
        {
            projects = new List<Project>();
            projectTypes = new List<MVVMClassType>();
            Projects allprojects = dte.Solution.Projects;
            System.Collections.IEnumerator it;
            it = allprojects.GetEnumerator();
            while (it.MoveNext())
            {
                Project p = it.Current as Project;
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
                                        projects.Add(p);
                                        projectTypes.Add(t);
                                        break;
                                    }
                                }
                                else
                                {
                                    projects.Add(p);
                                    projectTypes.Add(t);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
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
                string folder = Settings.Default[typename + "Folder"].ToString() + "\\" + templateFileName;
                cs = File.ReadAllText(Settings.Default[typename + "Folder"].ToString() + "\\" + templateFileName);
            }
            cs = cs.Replace("%namespace%", project.Name + "." + className + "s");
            cs = cs.Replace("%classname%", className);
            project.ProjectItems.AddFolder(className + "s");
            string fileName = Path.GetDirectoryName(project.FullName) + "\\" + className + "s.cs";
            File.WriteAllText(fileName, cs);
            project.ProjectItems.AddFromFile(fileName);
        }

        static public void InitializeDTE(DTE dte)
        {
            MVVMClassCreator.dte = dte;
        }

    }
}
