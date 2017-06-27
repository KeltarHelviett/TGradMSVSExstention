//------------------------------------------------------------------------------
// <copyright file="MVVMClassCreator.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace TGradMSVSExtention
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class MVVMClassCreator
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("fc766c36-b4c6-4d8f-a8b1-f8f2df0a9558");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="MVVMClassCreator"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private MVVMClassCreator(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static MVVMClassCreator Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new MVVMClassCreator(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "MVVMClassCreator";
            new MVVMClassCreatorForm().Show();
        }
        
        /// <summary>
        /// M, V, and VM templates are to simple so they can be generated via the same method. 
        /// This function is going to be splitted into 3 ( or possibly more ) functions or 
        /// it's going to take a template of class to generate. TODO
        /// </summary>
        /// <param name="className"></param>
        /// <param name="project"></param>
        private void GenerateMVVMClass(string className, Project project, string format)
        {
            string dir = Path.GetDirectoryName(project.FullName) + string.Format("\\{0}s", className);
            project.ProjectItems.AddFolder(string.Format("{0}s", className));
            string cs = string.Format(format, string.Format("{0}.{1}s", project.Name, className), className);
            string filePath = dir + "\\" + className + ".cs";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                File.WriteAllText(filePath, cs);
                project.ProjectItems.AddFromFile(filePath);
            }
        }

        public void GenerateMVVMClasses(string className)
        {
            DTE dte = (DTE)this.ServiceProvider.GetService(typeof(DTE));
            Project model = null, view = null, viewmodel = null;
            System.Collections.IEnumerator it;
            try
            {
                it = dte.Solution.Projects.GetEnumerator();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                MessageBox.Show("There is no opened solution");
                return;
            }
            try
            {
                while (it.MoveNext())
                {
                    Project p = it.Current as Project;
                    string pname = p.Name.ToLower();
                    if (pname.Contains("viewmodel"))
                        viewmodel = p;
                    else
                    {
                        model = pname.Contains("model") ? p : model;
                        view  = pname.Contains("view")  ? p : view;
                    }

                }
                if (model == null || view == null || viewmodel == null)
                    throw new Exception("Missing a project");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                MessageBox.Show("Missing a project of MVVM model");
                return;
            }
            GenerateMVVMClass(className, model, modelFormat);
            GenerateMVVMClass(className, view, viewFormat);
            GenerateMVVMClass(className, viewmodel, viewModelFormat);
            MessageBox.Show("Done");
        }

        private string modelFormat = @"
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace {0}
    {{
        public class {1} : Entity
        {{

        }}
    }}
";
 
        private string viewModelFormat = @"
namespace {0}
    {{
	    public class {1}
	    {{
		
	    }}
    }}
";

        private string viewFormat = @"
namespace {0}
    {{
	    public partial class {1}
	    {{
		    public {1}()
		    {{
			    InitializeComponent();
		    }}
	    }}
    }}
";
    }
}
