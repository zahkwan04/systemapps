   M S B A M L   `   `   `  ����E  Asystemapps, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:systemapps
systemapps  5   	       systemapps.ErrorDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:systemapps     5   	     � Title$  ErrorDialog��5   	   $	��180��6   $	��350��6*    � WindowStartupLocation$ CenterScreen=�66    � WindowStyle$
 None=�6[    � 
ResizeMode$ NoResize=�6n   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��10* �6   �� 5      $	��50* �6   �� 5      $	��50* �6   �� 5      $	��50* �6   �� 5      $	��10* �6    � RowDefinitions 5   
   �� 5      $��5* �6   �� 5      $��5* �6   �� 5      $	��10* �6   �� 5      $	��10* �6   �� 5      $	��10* �6   �� 5      $	��10* �6   �� 5      $	��12* �6   �� 5      $	��10* �6   .+�5   
   � -   5         �Name$ ErrorDialogBar��5      $��0��6(   $��5��65   
���   �6\   �� 5   
   -   5         ��Name$ errorDialogMessage��5   g   $��1��6   $��3��6#   $��0,0,0,0q�60   $��3��6B   $��3��6S   " �� VerticalScrollBarVisibility$
 Auto=�5      $��0q�63   	 �� TextWrapping$
	 Wrap=�5      .��6$   Select a record to be deleted6C   �� 5!   
   -   5        $��2��5!      $��5��6!   $
��Auto��6/   $
��Auto��6=   $��OK��6g   
���   �6�   5#      5$      