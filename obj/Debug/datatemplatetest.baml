   M S B A M L   `   `   `  ����E  Asystemapps, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:systemapps
systemapps  5   	   !    systemapps.datatemplatetest   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   $localclr-namespace:systemapps  5   	     � Title$  datatemplatetest��5	   	   $	��600��6"   $
��1080��6/    � 	Resources 5
         OpenMenu �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value  	CloseMenu��%�  &	        &	 �     �� 5   
   .��5      U� $ (FrameworkElement.Width)�6,   $ GridMenu��6a   .\�5        $ 0��6'   $ 24Q�63     5      $ 0:0:0.25��6'   $	 200Q�6:   5      5      �� 5   
   .��5      U� $ (FrameworkElement.Width)�6,   $ GridMenu��6a   .\�5        $ 0��6'   $	 200Q�63     5      $ 0:0:0.25��6'   $ 24Q�6:   5      5      5       � Triggers 5      :� 5   
    :� RoutedEvent$ ButtonBase.Click��6    :� 
SourceName$ ButtonOpenMenu��66   .[�5      �� #��[  6   5      :� 5   
   $ ButtonBase.Click��6   $ ButtonCloseMenu��66   .[�5      �� #��[ 6   5      .��5       � ���	#FFDEDEDE.+�5!   
   � -   5        	 �Name$	 GridMain��5!      $��Right=�6!   $��Bottom=�6=   $	��540��6X   $
��1010��6e   � 5"   
   $��60��6   $	��Top=�6   ���	#FF5C99D6
 J� Effect
 5#      3  -System.Windows.Media.Effects.DropShadowEffect  5$         
BlurRadius$ 1Q�6#      ShadowDepth$ 1Q�62      Color$ 	#FFACACAC��6B   .+�5&      ��  �� Orientation$ 
Horizontal=�6   $��Right=�62   .+�5'      �� $��
Abel Dutra��6   $��Center=�6.   $��16(�6I   '  !MaterialDesignThemes.Wpf.PopupBox  5(         PlacementMode$ BottomAndAlignRightEdges=�6*      	StaysOpen .  $��10q�6e      PopupContent. 5)      �� $	��150��6!   .+�5*      �� $��Settings��6!   �� 5+      $��Account��6!   �� 5,      $
��Help��6!   �� 5-      �� 5.      $��Logout��6!   5/      50      51      �� 52      $��Center=�6   $��Center=�65   $��22(�6P   Design com WPF6n   53      � 54   
   -   5        $	 GridMenu��54      $��25��6!   $
��Left=�6,   
�������
 55        56      $ 2Q�6#   $ 1Q�62   $ AntiqueWhite��6B   .+�58      �� .+�59      � $��60��6   
���   � J� RenderTransformOrigin$ 0.5,0.5&�66   z�5:      V� 5;      .~�5<      �� �� 5=      �� 5>      T� 5?      5@      .+�5B      �� -   5         ��Name$ ButtonOpenMenu��5B      $��60��65   $��25��6A   $��Right=�6M   $��Center=�6i    0 ,MaterialDesignFloatingActionMiniAccentButton#��[ 6�   
���������6�   Z� ��6�   Z� .��5C      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 Menu=�62   $��25��6>   $��25��6I   5D      �� 5E      -   5        $ ButtonCloseMenu��5E      $y�	Collapsed=�66   $��60��6M   $��25��6Y   $��Right=�6d   $��Center=�6�   #��[ 6�   
���������6�   Z� ��6�   Z� .��5F        $ 	ArrowLeft=�62   $��25��6C   $��25��6N   5G      5J      5L      5M      5N      5P      