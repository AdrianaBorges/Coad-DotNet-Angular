   M S B A M L   `   `   `  ����i  eMicrosoft.VisualStudio.Shell.15.0, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a�Zclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0!Microsoft.VisualStudio.PlatformUI  5       COAD.EXTENSION;   5COAD.EXTENSION.UserControls.CodeGeneratorUserControll   dlocalZclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      $	��534��5      $��665.991��6+     � 	Resources  5	         
LabelStyle �� 
TargetType �� Property �� Value��%�   &	        �� 5
   
   " ��6#   .�5      ��  � ��$ NoWrap=�6-   �� 5       � ��$
 None=�6-   �� 5       �@��IsHyphenationEnabled .  65   �� 5       � ��$ 15(�6)   5      5      .��5      � $��0,0,0,5q�6    � ColumnDefinitions 5   
   �� 5      $
��145* �6   �� 5      $
��381* �6   .+�5   
   �� $��Chaves Primárias:
��6   $
��Left=�61   $��29��6L   $��
20,124,0,0q�6X   $	��Top=�6l   $	��135��6�   $��2��6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ TbChavePrimaria��5      $
��Left=�6,   $	��125��6G   $��
10,124,0,0q�6T   $	��Top=�6h   $	��179��6�   $��1��6�      AutoGenerateColumns .     CanUserAddRows .  ��6�   �� *Keys+ �� NotifyOnSourceUpdated . 	 �� UpdateSourceTrigger$	 PropertyChanged=�
 �� Mode$
 TwoWay=�   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Key Nº��6%   -   5        ColumnKeyNumber��   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 5   N   ��  �� Path$ Index�$
 TwoWay=�$	 PropertyChanged=�6�   4  .System.Windows.Controls.DataGridTemplateColumn  5      -   5        ColumnCombox��$ Chave Primária��5   ?      CellTemplate 5      �� 5      .R�5      �� -   5         �� 
IsEditable .   �� SelectedIndex$ 0��5   :    �� SelectedItem 6L   �� $	 Key�$
 TwoWay=�$	 PropertyChanged=���6�   �� *Keys+6�   5      5!      5#      �� 5$   
   -   5        $��Criar��5$      $
��Left=�6!   $��239,581,0,0q�6<   $	��Top=�6Q   $	��132��6i   $��40��6u   $��1��6�   {�6�   �� *Metadata.PodeGerar+�� 5%   
   -   5         ��Name$ TbClasseSelecionada��5%      $
��Wrap=�61   $��{{ClasseSelecionada}}��6E   #���   6d   $��10,10,247,707q�6�   $��11(�6�    J� RenderTransformOrigin$ 	0.476,0.5&�6�   $��1��6�   �� 5&   
   $
��Left=�6   $��24��6/   $
��Wrap=�6;   $��Classe Selecionada:��6O   $	��Top=�6j   $	��130��6�   #���   6�   $��12(�6�   $��	20,10,0,0q�6�   $��2��6�   �� 5'   
   $��Model NameSpace:��6   $
��Left=�6+   $��25��6F   $��
10,278,0,0q�6R   $	��Top=�6f   $	��130��6~   {�6�   �� *Metadata.GerarModel+�� 5(   
   -   5         ��Name$ TxbModelNaspace��5(      $��1��6+   $
��Left=�6;   $��25��6V   $��
10,280,0,0q�6b    �� TextWrapping$
 Wrap=�6v   $	��Top=�6�   $	��355��6�   ��6�   �� *Metadata.ModelNamespace+�� 5)   
   $��Service NameSpace:��6   $
��Left=�6-   $��25��6H   $��
10,338,0,0q�6T   $	��Top=�6h   $	��130��6�   {�6�   �� *Metadata.GerarService+�� 5*   
   -   5        $ TxbDAONamespace��5*      $��1��6+   $
��Left=�6;   $��25��6V   $��
10,310,0,0q�6b   $
 Wrap=�6v   $	��Top=�6�   $	��355��6�   ��6�   �� *Metadata.DAONamespace+{�6�   �� *Metadata.GerarDAO+�� 5+   
   $��DAO NameSpace:��6   $
��Left=�6)   $��25��6D   $��
10,308,0,0q�6P   $	��Top=�6d   $	��130��6|   $ 	0.154,1.2&�6�   {�6�   �� *Metadata.GerarDAO+�� 5,   
   -	   5        $ TxbServiceNameSpace��5,      $��1��6/   $
��Left=�6?   $��25��6Z   $��
10,340,0,0q�6f   $
 Wrap=�6z   $	��Top=�6�   $	��355��6�   ��6�   �� *Metadata.ServiceNamespace+{�6�   �� *Metadata.GerarService+�� 5-   
   $��Gerar Model��6   $
��Left=�6)   $��	20,49,0,0q�6D   $	��Top=�6W   $ 0.545,0.312&�6o    d� 	IsChecked 6�   �� $ Metadata.GerarModel�$	 PropertyChanged=�$
 TwoWay=��� 5.   
   $��Gerar DAO
 ��6   $
��Left=�62   $��	20,70,0,0q�6M   $	��Top=�6`   $ 0.545,0.312&�6x   $��20��6�    6�   �� $ Metadata.GerarDAO�$	 PropertyChanged=�$
 TwoWay=��� 5/   
   $��Gerar Serviço��6   $
��Left=�6+   $��	20,90,0,0q�6F   $	��Top=�6Y   $ 0.545,0.312&�6q   $��20��6�    6�   �� $ Metadata.GerarService�$	 PropertyChanged=�$
 TwoWay=��� 50   
   $��1��6   $
��Left=�6$   $��19��6?   $��	10,10,0,0q�6K   $
��Wrap=�6^   $	��Top=�6r   $	��115��6�   ��6�   �� *Metadata.RefClassName+�� 51   
   -
   5        $��Concluir��51      $
��Left=�6$   $��239,581,0,0q�6?   $	��Top=�6T   $	��132��6l   $��40��6x   $��1��6�   {�6�   �� *Metadata.PodeGerar+�� 52   
   -   5        $��Próximo��52      $
��Left=�6#   $��
75,581,0,0q�6>   $	��Top=�6R   $	��132��6j   $��40��6v   $��1��6�   {�6�   �� *Metadata.PodeGerar+53      54      