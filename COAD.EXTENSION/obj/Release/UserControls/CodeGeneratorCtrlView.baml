   M S B A M L   `   `   `  ���� i  eMicrosoft.VisualStudio.Shell.15.0, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a�Zclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0!Microsoft.VisualStudio.PlatformUI   COAD.EXTENSION7   1COAD.EXTENSION.UserControls.CodeGeneratorCtrlView   dlocalZclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     $	��534��$	��506��  � 	Resources     
LabelStyle �� 
TargetType �� Property �� Value��%�   &	        �� " ��.���  � ��$ NoWrap=���  � ��$
 None=���  �@��IsHyphenationEnabled .  ��  � ��$ 15(�.��� $��0,0,0,5q�.+��� $��Gerar Views��$
��Left=�$��	10,64,0,0q�$	��Top=� J� RenderTransformOrigin$ 0.545,0.312&� d� 	IsChecked ��  �� Path$ Metadata.GerarViews� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=��� $��Gerar Controller
 ��$
��Left=�$��	10,39,0,0q�$	��Top=�$ 0.545,0.312&�$��20�� �� $ Metadata.GerarController�$ PropertyChanged=�$ TwoWay=��� $
��Wrap=�$��{{ClasseSelecionada}}��#���   $��10,10,247,707q�$��11(�$ 	0.476,0.5&�$��1���� $
��Left=�$��24��$
��Wrap=�$��Passo 2��$	��Top=�$	��130��#���   $��12(�$��	10,10,0,0q��� $��Projeto:��$
��Left=�$��25��$��	10,95,0,0q�$	��Top=�$��90���� $
��Left=�$��25��$��
125,95,0,0q�$	��Top=�$	��391��	 �� SelectedIndex$	 0��
 �� 
IsEditable
 .  ���� *
Projetos+ �� SelectedItem �� $ SelectedProject�$ TwoWay=�$ PropertyChanged=��� $��Colunas de Listagem��$
��Left=�$��29��$��
10,151,0,0q�$	��Top=�$	��130��&   System.Windows.Controls.DataGrid  -     Name$ TbChavePrimaria��$
��Left=�$	��125��$��
10,180,0,0q�$	��Top=�$	��506��   AutoGenerateColumns .     CanUserAddRows .  ���� *ColunasListagem+ �� NotifyOnSourceUpdated . $ PropertyChanged=�$ TwoWay=�   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Campo��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 100    CellTemplate �� .R��� -   
 .  $	 0�� �� $ Campo�$ TwoWay=�$ PropertyChanged=����� *Campos+0  *System.Windows.Controls.DataGridTextColumn  $ Label��$	 250 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $ Label�$ TwoWay=�$ PropertyChanged=�  $ 	Ordenavel��$	 250  �� $ 	Ordenavel�$ TwoWay=�$ PropertyChanged=�