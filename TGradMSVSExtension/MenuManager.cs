//------------------------------------------------------------------------------
// <copyright file="MVVMClassCreator.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Windows;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace TGradMSVSExtension
{

    internal sealed class MenuManager
    {

        public const int CreateClassesCmdId = 0x1023;
        public const int SettingsCmdId = 0x1024;
        public const int CreateClassesProjectNodeItemCmdId = 0x1026;

        public static readonly Guid CommandSet = new Guid("fc766c36-b4c6-4d8f-a8b1-f8f2df0a9558");

        private readonly Package package;

        private MenuManager(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                CommandID createClassesCmdId = new CommandID(CommandSet, CreateClassesCmdId);
                CommandID settingsCmdId = new CommandID(CommandSet, SettingsCmdId);
                CommandID createClassesProjectNodeItemCmdId = new CommandID(CommandSet, CreateClassesProjectNodeItemCmdId);
                MenuCommand createClasses = new MenuCommand(this.CreateClassesMenuItemCallback, createClassesCmdId);
                MenuCommand settings = new MenuCommand(this.SettingsMenuItemCallback, settingsCmdId);
                MenuCommand createClassesProjectItemNode = new MenuCommand(this.CreateClassesProjectNodeMenuItemCallback, createClassesProjectNodeItemCmdId);
                commandService.AddCommand(createClasses);
                commandService.AddCommand(settings);
                commandService.AddCommand(createClassesProjectItemNode);
            }
            SettingsViewModel.Load();
            DTE dte = (DTE)this.ServiceProvider.GetService(typeof(DTE));
            MVVMSolutionManager.InitializeDTE(dte);
        }

        public static MenuManager Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static void Initialize(Package package)
        {
            Instance = new MenuManager(package);
        }

        private void CreateClassesMenuItemCallback(object sender, EventArgs e)
        {
            new MVVMClassCreatorWindow().Show();
        }
        
        private void SettingsMenuItemCallback(object sender, EventArgs e)
        {
            new SettingsView().Show(); 
        }

        private void CreateClassesProjectNodeMenuItemCallback(object sender, EventArgs e)
        {
            DTE dte = (DTE)this.ServiceProvider.GetService(typeof(DTE));
            if (dte.SelectedItems.Count != 1)
                return;
            string className = dte.SelectedItems.Item(1).ProjectItem.Name;
            int index = className.IndexOf('.');
            if (index != -1)
            {
                className = className.Substring(0, index);
                new MVVMClassCreatorWindow(className).Show();
            }
        }
    }
}