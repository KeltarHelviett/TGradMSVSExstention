using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradMSVSExtention
{
    enum ProjectType
    {
        Model = 0, View = 1, ViewModel = 2, Repository, DatumNodeRepository
    }

    static class MetaData
    {
        public static string[] ProjectDisplayNames = new string[] { "Model", "View", "View Model", "Repository", "DN Repository"};
        public static Dictionary<ProjectType, string> ProjectTypeToProjectDisplayName = new Dictionary<ProjectType, string>
        {
            { ProjectType.Model, "Model" },
            { ProjectType.View, "View" },
            { ProjectType.ViewModel, "View Model" },
            { ProjectType.Repository, "Repository" },
            { ProjectType.DatumNodeRepository, "DN Repository" }
        };
    }
}
