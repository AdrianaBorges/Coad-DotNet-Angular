using COAD.EXTENSION.Model;
using COAD.EXTENSION.Service;
using COAD.EXTENSION.TextTemplate.Model;
using COAD.EXTENSION.UserControls;
using EnvDTE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace COAD.EXTENSION.Controller
{
    public class CodeGeneratorModalController
    {
        private static CodeGeneratorModalController Controller { get; set; }
        public ProjectSRV projectSRV { get; set; }
        public ModelDialogDTO modelDialogDTO { get; set; }
        public T4TemplateObjectMetadataModel metadata { get; set; }
        public CodeGeneratorUserControll ModalControl { get; set; }
        public CodeGeneratorCtrlView ModelCtrView { get; set; }


        private CodeGeneratorModalController()
        {
            projectSRV = new ProjectSRV();
        }

        private CodeGeneratorModalController(DTE dte)
        {
            projectSRV = new ProjectSRV();
            projectSRV.dte = dte;
        }

        public static CodeGeneratorModalController Instance { get {

                if (Controller == null)
                    Controller = new CodeGeneratorModalController();
                return Controller;
            }
        }

        private void IncluirNameSpacesSugeridos(T4TemplateObjectMetadataModel metadata)
        {
            if(metadata != null)
            {
                var defaultNamespace = metadata.DefaultNameSpace;
                metadata.ModelNamespace = string.Format("{0}.Model.DTO", defaultNamespace);
                metadata.DAONamespace = string.Format("{0}.DAO", defaultNamespace);
                metadata.ServiceNamespace = string.Format("{0}.Service", defaultNamespace);
            }
        }

        public CodeGeneratorUserControll AbrirModalGerarCodigos()
        {
            ModelDialogDTO modelDialogDTO = new ModelDialogDTO();

            var lstMetadatas = projectSRV.CreateMetatadaFromSelectedItems();
            this.metadata = lstMetadatas.FirstOrDefault();

            IncluirNameSpacesSugeridos(metadata);
            modelDialogDTO.Metadata = metadata;

            CodeGeneratorUserControll testDialog = new CodeGeneratorUserControll();

            var table = testDialog.TbChavePrimaria;
            var properties = this.metadata.NormalProperties;


            var keys = properties.Select(x => x.Name ).ToList();
            keys = new List<string>() {""}.Concat(keys).ToList();
            ICollection<DataGridSelectKeyRow> rows = new HashSet<DataGridSelectKeyRow>();
            modelDialogDTO.ComboKeys = keys;

            rows.Add(new DataGridSelectKeyRow()
            {
                Index = 0,
                Keys = new ObservableCollection<string>(keys),
            });

            //foreach (var row in rows)
            //{
            //    table.Items.Add(row);
            //}

            modelDialogDTO.Keys = new ObservableCollection<DataGridSelectKeyRow>(rows);
            this.modelDialogDTO = modelDialogDTO;
            this.ModalControl = testDialog;
            testDialog.DataContext = modelDialogDTO;

            return testDialog;
        }

        public void GerarTemplates()
        {
            var modelDialogDTO = this.ModalControl.DataContext as ModelDialogDTO;
            if (modelDialogDTO != null && modelDialogDTO.Keys != null)
            {

                var selectedKeys = modelDialogDTO.SelectedKey;
                if(selectedKeys != null && selectedKeys.Count() > 0 && modelDialogDTO.Metadata != null)
                {
                    modelDialogDTO.Metadata.PrimaryKeys = selectedKeys.ToList();
                    this.projectSRV.CreateClassFromMetadata(modelDialogDTO.Metadata);
                }
            }
        }
    }
}
