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
    enum ProjectType
    {
        Model = 0, View = 1, ViewModel = 2
    }

    static class MVVMSolutionManager
    {

        static private DTE dte;

        static public void InitializeDTE(DTE dte)
        {
            MVVMSolutionManager.dte = dte;
        }

        static public void FillActiveSolution(List<ProjectType> types, string className, List<string> templateFileNames)
        {
            var ps = ((Array)dte.ActiveSolutionProjects).Cast<Project>().ToArray();
            var prefixes = FindPrefixes(types);
            var typedProjects = GetTypedProjects(prefixes, types);
            foreach (var project in typedProjects.Keys)
            {
                AddClassSet(typedProjects[project], className, templateFileNames[types.IndexOf(typedProjects[project])], project);
            }
        }

        static private List<string> FindPrefixes(List<ProjectType> types)
        {
            var ps = ((Array)dte.ActiveSolutionProjects).Cast<Project>().ToArray();
            var prefixes = new List<string>();
            foreach (var p in ps)
            {
                string prefix = p.Name.Substring(0, p.Name.LastIndexOf("."));
                if (prefix != null && prefix != "" && !prefixes.Contains(prefix))
                    prefixes.Add(prefix);
            }
            return prefixes;
        }
        
        static private Dictionary<Project, ProjectType> GetTypedProjects(List<string> prefixes, List<ProjectType> types)
        {
            var typedProjects = new Dictionary<Project, ProjectType>();
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

        static private void AddClassSet(ProjectType type, string className, string templateFileName, Project project)
        {
            string tmp = $@"{Path.GetDirectoryName(project.FullName)}\{className}s";
            if (!Directory.Exists($@"{Path.GetDirectoryName(project.FullName)}\{className}s"))
                project.ProjectItems.AddFolder(className + "s");
            switch (type)
            {
                case ProjectType.Model:
                    MVVMClassCreator.CreateModelClassSet(className, templateFileName, project);
                    break;
                case ProjectType.View:
                    MVVMClassCreator.CreateViewClassSet(className, templateFileName, project);
                    break;
                case ProjectType.ViewModel:
                    MVVMClassCreator.CreateViewModelClassSet(className, templateFileName, project);
                    break;
            }
        }

        private static class MVVMClassCreator
        {
            static public void CreateViewModelClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("ViewModel", className, templateFileName, className, project);
                CreateClass("DetailViewModel", className, templateFileName, className + "Detail", project);
                CreateClass("MasterViewModel", className, templateFileName, className + "Master", project);
            }

            static public void CreateViewClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("View", className, templateFileName, className, project);
            }

            static public void CreateModelClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("Model", className, templateFileName, className, project);
            }

            static private void CreateClass(string classType, string className, string templateFileName, string fileName, Project project)
            {
                string cs = "";
                cs = templateFileName == "Default" ? Settings.Default["Default" + classType].ToString() :
                    "";
                cs = cs.Replace("%namespace%", $"{project.Name}.{className}s");
                cs = cs.Replace("%classname%", className);
                
                string filePath = $@"{Path.GetDirectoryName(project.FullName)}\{className}s\{fileName}.cs";
                if (File.Exists(filePath))
                {
                    //TODO
                    // Add interaction with user that allows him to choose between 2 options:
                    //  Overwrite file or not.
                    //TODO
                }
                File.WriteAllText(filePath, cs);
                project.ProjectItems.AddFromFile(filePath);
            }
        }

    }
}
