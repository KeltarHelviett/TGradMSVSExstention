using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradMSVSExtension
{
    enum ClassType
    {
        Model = 0, View = 1, ViewModel = 2, Repository, DatumNodeRepository,
        DetailView, MasterView, DetailViewModel, MasterViewModel
    }

    enum ProjectType
    {
        Model = 0, View = 1, ViewModel = 2, Repository, DatumNodeRepository
    }

    static class MetaData
    {
        public static string[] ProjectDisplayNames = new string[] { "Model", "View", "View Model", "Repository", "DN Repository"};
        public static Dictionary<ClassType, string> ClassTypeToClassDisplayName = new Dictionary<ClassType, string>
        {
            { ClassType.Model, "Model" },
            { ClassType.View, "View" },
            { ClassType.ViewModel, "View Model" },
            { ClassType.Repository, "Repository" },
            { ClassType.DatumNodeRepository, "DN Repository" },
            { ClassType.DetailView, "Detail View" },
            { ClassType.DetailViewModel, "Detail VM" },
            { ClassType.MasterView, "Master View" },
            { ClassType.MasterViewModel, "Master VM" }
        };

        public static Dictionary<ClassType, ProjectType> ClassTypeToProjectType = new Dictionary<ClassType, ProjectType>
        {
            { ClassType.Model, ProjectType.Model },
            { ClassType.View, ProjectType.View },
            { ClassType.ViewModel, ProjectType.ViewModel },
            { ClassType.Repository, ProjectType.Repository },
            { ClassType.DatumNodeRepository, ProjectType.DatumNodeRepository },
            { ClassType.DetailView, ProjectType.View },
            { ClassType.DetailViewModel, ProjectType.ViewModel },
            { ClassType.MasterView, ProjectType.View },
            { ClassType.MasterViewModel, ProjectType.ViewModel }
        };
    }
}
