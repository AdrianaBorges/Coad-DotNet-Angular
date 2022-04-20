using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.PlatformUI;
using COAD.EXTENSION.Controller;
using System.Collections.Generic;
using EnvDTE;
using System.Text;
using System.Linq;
using COAD.EXTENSION.TextTemplate;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace COAD.EXTENSION
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class CommandCriarTemplate
    {
        public CodeGeneratorModalController Controller { get; set; }
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("262e0989-0773-4275-8481-825319d81172");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCriarTemplate"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private CommandCriarTemplate(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;
            var dte = this.ServiceProvider.GetService(typeof(SDTE)) as DTE;
            ITextTemplating t4 = this.ServiceProvider.GetService(typeof(STextTemplating)) as ITextTemplating;

            this.Controller = CodeGeneratorModalController.Instance;
            this.Controller.projectSRV.dte = dte;
            this.Controller.projectSRV.compiler = new T4TemplateCompiler(t4);

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
        public static CommandCriarTemplate Instance
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
            Instance = new CommandCriarTemplate(package);
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
            string title = "CommandCriarTemplate";
            
            var modal = Controller.AbrirModalGerarCodigos();
            
            modal.ShowModal();
        }
    }
}
