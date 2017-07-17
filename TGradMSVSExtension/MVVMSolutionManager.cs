using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TGradMSVSExtension.Properties;
using System.IO;
using EnvDTE;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace TGradMSVSExtension
{
    
    static class MVVMSolutionManager
    {

        static private DTE dte;
        static private Dictionary<ClassType, Action<string, string, Project>> classCreatingFucntions = new Dictionary<ClassType, Action<string, string, Project>>
        {
            { ClassType.Model, MVVMClassCreator.CreateModelClassSet },
            { ClassType.View, MVVMClassCreator.CreateViewClassSet },
            { ClassType.DetailView, MVVMClassCreator.CreateDetailViewClassSet },
            { ClassType.MasterView, MVVMClassCreator.CreateMasterViewClassSet },
            { ClassType.ViewModel, MVVMClassCreator.CreateViewModelClassSet },
            { ClassType.DetailViewModel, MVVMClassCreator.CreateDetailViewModelClassSet },
            { ClassType.MasterViewModel, MVVMClassCreator.CreateMasterViewModelClassSet },
            { ClassType.Repository, MVVMClassCreator.CreateRepositoryClassSet },
            { ClassType.DatumNodeRepository, MVVMClassCreator.CreateDatumNodeRepositoryClassSet },

        };

        static public void InitializeDTE(DTE dte)
        {
            MVVMSolutionManager.dte = dte;
        }

        static public void FillActiveSolution(List<ClassType> types, string className, List<string> templateFileNames)
        {
            var ps = ((Array)dte.ActiveSolutionProjects).Cast<Project>().ToArray();
            var prefixes = FindPrefixes(types);
            var typedProjects = GetTypedProjects(prefixes, types);
            foreach (var project in typedProjects.Keys)
            {
                foreach (var type in typedProjects[project])
                {
                    AddClassSet(type, className, templateFileNames[types.IndexOf(type)], project);
                }
            }
        }

        static private List<string> FindPrefixes(List<ClassType> types)
        {
            var ps = ((Array)dte.ActiveSolutionProjects).Cast<Project>().ToArray();
            var prefixes = new List<string>();
            foreach (var p in ps)
            {
                string prefix = p.Name.Substring(0, p.Name.LastIndexOf(".") + 1);
                if (prefix != null && prefix != "" && !prefixes.Contains(prefix))
                    prefixes.Add(prefix);
            }
            return prefixes;
        }
        
        static private Dictionary<Project, List<ClassType>> GetTypedProjects(List<string> prefixes, List<ClassType> types)
        {
            var typedProjects = new Dictionary<Project, List<ClassType>>();
            var it = dte.Solution.Projects.GetEnumerator();
            while (it.MoveNext())
            {
                var p = it.Current as Project;
                foreach (var prefix in prefixes)
                {
                    if (p.Name.StartsWith(prefix))
                    {
                        string suffix = p.Name.ToLower().Substring(prefix.Length);
                        foreach (var t in types)
                        {
                            var pt = MetaData.ClassTypeToProjectType[t];
                            string lcptname = pt.ToString().ToLower();
                            if (suffix == lcptname || suffix == lcptname + "s")
                            {
                                if (!typedProjects.ContainsKey(p))
                                {
                                    typedProjects.Add(p, new List<ClassType> { t });
                                }
                                else
                                {
                                    typedProjects[p].Add(t);
                                }
                                
                            }
                        }
                    }
                }
            }
            return typedProjects;
        }

        static private void AddClassSet(ClassType type, string className, string templateFileName, Project project)
        {
            string dirname = $@"{Path.GetDirectoryName(project.FullName)}\{className}s";
            if (!Directory.Exists(dirname))
                project.ProjectItems.AddFolder(className + "s");
            string templateFileFullName = templateFileName == "Default" ? "Default" : 
                $@"{SettingsModel.Default[type.ToString() + "Folder"].ToString()}\{templateFileName}";
            classCreatingFucntions[type](className, templateFileFullName, project);
        }

        private static class MVVMClassCreator
        {
            static public void CreateViewModelClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("ViewModel", className, templateFileName, $"{className}sViewModel.cs", project);
            }

            static public void CreateViewClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("View", className, templateFileName, $"{className}View.xaml.cs", project);
                CreateClass("ViewXaml", className, templateFileName, $"{className}View.xaml", project);
            }

            static public void CreateModelClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("Model", className, templateFileName, $"{className}.cs", project);
                CreateClass("ModelFilter", className, templateFileName, $"{className}Filter.cs", project);
            }
            
            static public void CreateRepositoryClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("Repository", className, templateFileName, $"I{className}Repository.cs", project);
            }

            static public void CreateDatumNodeRepositoryClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("DatumNodeRepository", className, templateFileName, $"{className}Repository.cs", project);
            }

            static public void CreateDetailViewClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("DetailView", className, templateFileName, $"{className}DetailView.xaml.cs", project);
                CreateClass("DetailViewXaml", className, templateFileName, $"{className}DetailView.xaml", project);
            }

            static public void CreateDetailViewModelClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("DetailViewModel", className, templateFileName, $"{className}DetailViewModel.cs", project);
            }

            static public void CreateMasterViewClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("MasterView", className, templateFileName, $"{className}sMasterView.xaml.cs", project);
                CreateClass("MasterViewXaml", className, templateFileName, $"{className}sMasterView.xaml", project);
            }

            static public void CreateMasterViewModelClassSet(string className, string templateFileName, Project project)
            {
                CreateClass("MasterViewModel", className, templateFileName, $"{className}sMasterViewModel.cs", project);
            }

            static private void CreateClass(string classType, string className, string templateFileFullName, string fileName, Project project)
            {
                string cs = templateFileFullName == "Default" ? SettingsModel.Default["Default" + classType].ToString() :
                    GetTemplateFromFile(templateFileFullName, classType);
                string prefix = project.Name.Substring(0, project.Name.LastIndexOf(".") + 1);
                cs = cs.Replace("%namespace%", $"{project.Name}.{className}s");
                cs = cs.Replace("%classname%", className);
                cs = cs.Replace("%prefix%", prefix);
                
                string filePath = $@"{Path.GetDirectoryName(project.FullName)}\{className}s\{fileName}";
                MessageBoxResult res = MessageBoxResult.Yes;
                if (File.Exists(filePath))
                {
                    res = MessageBox.Show($@"Would you like to overwrite existing file: {filePath}",
                        "", MessageBoxButton.YesNo);
                }
                if (res == MessageBoxResult.Yes)
                {
                    File.WriteAllText(filePath, cs);
                    project.ProjectItems.AddFromFile(filePath);
                }
            }

            static private string GetTemplateFromFile(string fileName, string classType)
            {
                string file = File.ReadAllText(fileName);
                JObject jo = JObject.Parse(file);
                return string.Join("\n", jo[classType].Select(t => (string)t).ToArray());
            }
        }

    }
}
