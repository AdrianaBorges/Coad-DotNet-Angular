   M S B A M L   `   `   `  ���� i  eMicrosoft.VisualStudio.Shell.15.0, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a�Zclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0!Microsoft.VisualStudio.PlatformUI   COAD.EXTENSION;   5COAD.EXTENSION.UserControls.CodeGeneratorUserControll   dlocalZclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     $	��534��$��665.991��  � 	Resources     
LabelStyle �� 
TargetType �� Property �� Value��%�   &	        �� " ��.���  � ��$ NoWrap=���  � ��$
 None=���  �@��IsHyphenationEnabled .  ��  � ��$ 15(�.��� $��0,0,0,5q� � ColumnDefinitions �� $
��145* ��� $
��381* �.+��� $��Chaves Primárias:
��$
��Left=�$��29��$��
20,124,0,0q�$	��Top=�$	��135��$��2��&   System.Windows.Controls.DataGrid  -     Name$ TbChavePrimaria��$
��Left=�$	��125��$��
10,124,0,0q�$	��Top=�$	��179��$��1��   AutoGenerateColumns .     CanUserAddRows .  ���� *Keys+ �� NotifyOnSourceUpdated . 	 �� UpdateSourceTrigger$	 PropertyChanged=�
 �� Mode$
 TwoWay=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Key Nº��-   ColumnKeyNumber��   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding ��  �� Path$ Index�$
 TwoWay=�$	 PropertyChanged=�4  .System.Windows.Controls.DataGridTemplateColumn  -   ColumnCombox��$ Chave Primária��   CellTemplate �� .R��� -    �� 
IsEditable .   �� SelectedIndex$ 0�� �� SelectedItem �� $	 Key�$
 TwoWay=�$	 PropertyChanged=����� *Keys+�� -   $��Criar��$
��Left=�$��239,581,0,0q�$	��Top=�$	��132��$��40��$��1��{��� *Metadata.PodeGerar+�� -    ��Name$ TbClasseSelecionada��$
��Wrap=�$��{{ClasseSelecionada}}��#���   $��10,10,247,707q�$��11(� J� RenderTransformOrigin$ 	0.476,0.5&�$��1���� $
��Left=�$��24��$
��Wrap=�$��Classe Selecionada:��$	��Top=�$	��130��#���   $��12(�$��	20,10,0,0q�$��2���� $��Model NameSpace:��$
��Left=�$��25��$��
10,278,0,0q�$	��Top=�$	��130��{��� *Metadata.GerarModel+�� -    ��Name$ TxbModelNaspace��$��1��$
��Left=�$��25��$��
10,280,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��355������ *Metadata.ModelNamespace+�� $��Service NameSpace:��$
��Left=�$��25��$��
10,338,0,0q�$	��Top=�$	��130��{��� *Metadata.GerarService+�� -   $ TxbDAONamespace��$��1��$
��Left=�$��25��$��
10,310,0,0q�$
 Wrap=�$	��Top=�$	��355������ *Metadata.DAONamespace+{��� *Metadata.GerarDAO+�� $��DAO NameSpace:��$
��Left=�$��25��$��
10,308,0,0q�$	��Top=�$	��130��$ 	0.154,1.2&�{��� *Metadata.GerarDAO+�� -	   $ TxbServiceNameSpace��$��1��$
��Left=�$��25��$��
10,340,0,0q�$
 Wrap=�$	��Top=�$	��355������ *Metadata.ServiceNamespace+{��� *Metadata.GerarService+�� $��Gerar Model��$
��Left=�$��	20,49,0,0q�$	��Top=�$ 0.545,0.312&� d� 	IsChecked �� $ Metadata.GerarModel�$	 PropertyChanged=�$
 TwoWay=��� $��Gerar DAO
 ��$
��Left=�$��	20,70,0,0q�$	��Top=�$ 0.545,0.312&�$��20�� �� $ Metadata.GerarDAO�$	 PropertyChanged=�$
 TwoWay=��� $��Gerar Serviço��$
��Left=�$��	20,90,0,0q�$	��Top=�$ 0.545,0.312&�$��20�� �� $ Metadata.GerarService�$	 PropertyChanged=�$
 TwoWay=��� $��1��$
��Left=�$��19��$��	10,10,0,0q�$
��Wrap=�$	��Top=�$	��115������ *Metadata.RefClassName+�� -
   $��Concluir��$
��Left=�$��239,581,0,0q�$	��Top=�$	��132��$��40��$��1��{��� *Metadata.PodeGerar+�� -   $��Próximo��$
��Left=�$��
75,581,0,0q�$	��Top=�$	��132��$��40��$��1��{��� *Metadata.PodeGerar+