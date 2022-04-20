using COAD.EXTENSION.Controller;
using COAD.EXTENSION.Model;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace COAD.EXTENSION.UserControls
{
    /// <summary>
    /// Interação lógica para CodeGeneratorUserControll.xam
    /// </summary>
    public partial class CodeGeneratorCtrlView : DialogWindow
    {
        public bool CanUpdate { get; set; }
        public bool Reorder { get; set; }
        public bool ContinueWithouUpdate { get; set; }
        public CodeGeneratorCtrlView()
        {
            this.HasMaximizeButton = true;
            this.HasMinimizeButton = true;
            this.CanUpdate = true;
            InitializeComponent();
            
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var controller = CodeGeneratorModalController.Instance;
            controller.GerarTemplates();
            this.Close();
        }

        private void OnComboChange(object sender, RoutedEventArgs e)
        {
            try
            {
                var controller = CodeGeneratorModalController.Instance;

				if (DataContext != null && DataContext is ModelDialogDTO)
				{
					var modelDialog = DataContext as ModelDialogDTO;
                    var campos = controller.projectSRV.ListarCampos(modelDialog.Metadata);  
					var colunas = modelDialog.ColunasListagem;

					if (colunas.Count > 0)
					{
                        if (!string.IsNullOrWhiteSpace(modelDialog.ColunasListagem.Last().Campo))
                        {
                            if (campos.Count > 0)
                            {
                                modelDialog.ColunasListagem.Add(new DefinicaoColunasDTO()
                                {
									Campos = campos
                                });
                                return;
                            }
                        }
                    }
				}
                
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro no update", ex);
            }
            finally
            {
                if (ContinueWithouUpdate == false)
                {
                    CanUpdate = true;
                }
                else
                {
                    ContinueWithouUpdate = false;
                }

                if (CanUpdate == false && ContinueWithouUpdate == false)
                {
                    CanUpdate = true;
                }
            }
            
        }

        public void ReorderKeys(ModelDialogDTO modelDialog)
        {
            if (modelDialog != null) {
                var index = 0;

                foreach (var key in modelDialog.Keys)
                {
                    key.Index = index;
                    key.Keys = modelDialog.ComboKeysToUse.ToList();
                    index++;
                }
            }
        }
    }
}
