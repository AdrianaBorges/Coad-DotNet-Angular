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
    public partial class CodeGeneratorUserControll : DialogWindow
    {
        public bool CanUpdate { get; set; }
        public bool Reorder { get; set; }
        public bool ContinueWithouUpdate { get; set; }
        public CodeGeneratorUserControll()
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

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void proximoClick(object sender, RoutedEventArgs e)
        {
            var controller = CodeGeneratorModalController.Instance;
            var dto = controller.modelDialogDTO;
            this.Hide();
            CodeGeneratorCtrlView codeGeneratorCtrlView = new CodeGeneratorCtrlView();
            dto.Projetos = controller.projectSRV.GetProjectsName();
            var lstCampos = controller.projectSRV.ListarCampos(dto.Metadata);
            dto.ColunasListagem.Add(new DefinicaoColunasDTO() {

                Campos = lstCampos
            });
            codeGeneratorCtrlView.DataContext = dto;
            codeGeneratorCtrlView.ShowModal();
        }

        private void OnComboChange(object sender, RoutedEventArgs e)
        {

            try
            {
                if (CanUpdate && ContinueWithouUpdate == false)
                {

                    CanUpdate = false;
                    if (DataContext != null && DataContext is ModelDialogDTO)
                    {
                        var modelDialog = DataContext as ModelDialogDTO;
                        var keys = modelDialog.Keys;

                        if (modelDialog.Keys.Count > 0)
                        {
                            var lastKey = modelDialog.Keys.Last();
                            int index = 0;

                            foreach (var key in new List<DataGridSelectKeyRow>(modelDialog.Keys))
                            {
                                if (key.Index != lastKey.Index && string.IsNullOrWhiteSpace(key.Key))
                                {
                                    var index2 = 0;
                                    var startRemoveIndex = index + 1;
                                    foreach(var keyRemove in new List<DataGridSelectKeyRow>(modelDialog.Keys)) {

                                        if (index2 >= startRemoveIndex)
                                        {
                                            modelDialog.Keys.Remove(keyRemove);
                                        }

                                        index2++;
                                    }

                                    //ReorderKeys(modelDialog);
                                    return;                                    
                                    
                                }
                                index++;
                            }


                            if (!string.IsNullOrWhiteSpace(modelDialog.Keys.Last().Key))
                            {
                                var comboElements = modelDialog.ComboKeysToUse;
                                var countComboElements = comboElements.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                                if (countComboElements.Count > 0)
                                {
                                    modelDialog.Keys.Add(new DataGridSelectKeyRow()
                                    {
                                        Index = keys.Count,
                                        Keys = comboElements.ToList()
                                    });
                                    return;
                                }
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
