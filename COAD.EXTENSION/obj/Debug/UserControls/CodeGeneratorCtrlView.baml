   M S B A M L   `   `   `  ����i  eMicrosoft.VisualStudio.Shell.15.0, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a�Zclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0!Microsoft.VisualStudio.PlatformUI  5       COAD.EXTENSION7   1COAD.EXTENSION.UserControls.CodeGeneratorCtrlView   dlocalZclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      $	��534��5      $	��506��6+     � 	Resources  5	         
LabelStyle �� 
TargetType �� Property �� Value��%�   &	        �� 5
   
   " ��6#   .�5      ��  � ��$ NoWrap=�6-   �� 5       � ��$
 None=�6-   �� 5       �@��IsHyphenationEnabled .  65   �� 5       � ��$ 15(�6)   5      5      .��5      � $��0,0,0,5q�6   .+�5   
   �� $��Gerar Views��6   $
��Left=�6)   $��	10,64,0,0q�6D   $	��Top=�6W    J� RenderTransformOrigin$ 0.545,0.312&�6o    d� 	IsChecked 6�   ��  �� Path$ Metadata.GerarViews� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=��� 5   
   $��Gerar Controller
 ��6   $
��Left=�69   $��	10,39,0,0q�6T   $	��Top=�6g   $ 0.545,0.312&�6   $��20��6�    6�   �� $ Metadata.GerarController�$ PropertyChanged=�$ TwoWay=��� 5   
   $
��Wrap=�6   $��{{ClasseSelecionada}}��6(   #���   6G   $��10,10,247,707q�6l   $��11(�6�   $ 	0.476,0.5&�6�   $��1��6�   �� 5   
   $
��Left=�6   $��24��6/   $
��Wrap=�6;   $��Passo 2��6O   $	��Top=�6^   $	��130��6v   #���   6�   $��12(�6�   $��	10,10,0,0q�6�   �� 5   
   $��Projeto:��6   $
��Left=�6#   $��25��6>   $��	10,95,0,0q�6J   $	��Top=�6]   $��90��6u   �� 5   
   $
��Left=�6   $��25��6.   $��
125,95,0,0q�6:   $	��Top=�6N   $	��391��6f   	 �� SelectedIndex$	 0��6r   
 �� 
IsEditable
 .  ��6�   �� *
Projetos+ �� SelectedItem 6�   �� $ SelectedProject�$ TwoWay=�$ PropertyChanged=�6  �� 5   
   $��Colunas de Listagem��6   $
��Left=�6.   $��29��6I   $��
10,151,0,0q�6U   $	��Top=�6i   $	��130��6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ TbChavePrimaria��5      $
��Left=�6,   $	��125��6G   $��
10,180,0,0q�6T   $	��Top=�6h   $	��506��6�      AutoGenerateColumns .     CanUserAddRows .  ��5      �� *ColunasListagem+ �� NotifyOnSourceUpdated . $ PropertyChanged=�$ TwoWay=�   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Campo��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 68      CellTemplate 5      �� 5       .R�5!      �� -   5        
 .  $	 0��5!   :    6L   �� $ Campo�$ TwoWay=�$ PropertyChanged=���6�   �� *Campos+6�   5"      5$      0  *System.Windows.Controls.DataGridTextColumn  5%      $ Label��6%   $	 250 6�   1  +System.Windows.Controls.DataGridBoundColumn   Binding 64   �� $ Label�$ TwoWay=�$ PropertyChanged=�6�     5&      $ 	Ordenavel��6%   $	 250 6�    68   �� $ 	Ordenavel�$ TwoWay=�$ PropertyChanged=�6�   5(      5)      5*      