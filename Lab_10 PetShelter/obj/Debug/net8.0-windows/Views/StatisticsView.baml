   M S B A M L   `   `   `  ����H  DPetShelter.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:Lab_10_PetShelter.ViewsLab_10_PetShelter.Views  5   	   ,    &Lab_10_PetShelter.Views.StatisticsView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:Lab_10_PetShelter.Views     5   	     � Title$'  !Статистика приюта��5   	   $	��500��6#   $	��700��60   .��5	      � $��15q�6    � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0,0,10,0q�6%   .+�5      �� $��20(�5      $
��Bold"�5      $��0,0,0,20q�5      ��5      �� *ShelterName+5      �� 5      
�������$��1q�5       �� Padding$ 10q�5      $��0,0,0,10q�5      ���#F5F5F5._�5      �� .+�5      �� $%��Общая информация��6    $
��Bold"�5       $��0,0,0,5q�5       �� 5      $	��0,2q�6?   ��6    �� *TotalPetsInfo+6?   �� 5       $	��0,2q�6D   ��6    �� *AvailableSpaceInfo+6D   �� 5!      $	��0,2q�6?   ��6    �� *OccupancyInfo+6?   5"      5#      �� 5%      
�������$��1q�5&      $ 10q�5'      $��0,0,0,10q�5(      ���#F5F5F5._�5*      �� .+�5+      �� $0��*Распределение по типам��6    $
��Bold"�5,       $��0,0,0,5q�5-       �� 5.      $	��0,2q�6:   ��6    �� *
DogsInfo+6:   �� 5/      $	��0,2q�6:   ��6    �� *
CatsInfo+6:   �� 50      $	��0,2q�6=   ��6    �� *RabbitsInfo+6=   51      52      �� 54      
�������$��1q�55      $ 10q�56      $��0,0,0,10q�57      ���#F5F5F5._�59      �� .+�5:      �� $-��'Общие характеристики��6    $
��Bold"�5;       $��0,0,0,5q�5<       �� 5=      $	��0,2q�6@   ��6    �� *AverageAgeInfo+6@   �� 5>      $	��0,2q�6C   ��6    �� *AverageWeightInfo+6C   �� 5?      $	��0,2q�6D   ��6    �� *ClaustrophobicInfo+6D   5@      5A      5B      � 5E   
   $��1��6    � RowDefinitions 5F      �� 5G      $
��Auto �6    �� 5H      $��* �6    �� 5I      $
��Auto �6    .+�5L      �� $��0��6   $1��+Распределение питомцев��5M      $
��Bold"�5N      $��0,0,0,10q�5O      $��Center=�5P      �� 5R      $��1��6   $	��200��6"   $	��200��6.   .+�5S      F� ��6   �� *DogsSegmentBrush+��69   �� *DogsSegment+F� 5T      ��6   �� *CatsSegmentBrush+��69   �� *CatsSegment+F� 5U      ��6   �� *RabbitsSegmentBrush+��6<   �� *RabbitsSegment+5V      �� 5X      $��2��6   $��0,20,0,0q�6&   .+�5Y      �� $��Легенда:��6   $
��Bold"�6,   $��0,0,0,5q�6>   �� 5Z       �� Orientation$ 
Horizontal=�6   $	��0,2q�66   .+�5[      �� $��15��6    $��15��6+   $��0,0,5,0q�6Y   ��67   �� *DogsSegmentBrush+6Y   �� 5\      $��Собаки��6    5]      �� 5^      $ 
Horizontal=�6   $	��0,2q�66   .+�5_      �� $��15��6    $��15��6+   $��0,0,5,0q�6Y   ��67   �� *CatsSegmentBrush+6Y   �� 5`      $��
Кошки��6    5a      �� 5b      $ 
Horizontal=�6   $	��0,2q�66   .+�5c      �� $��15��6    $��15��6+   $��0,0,5,0q�6\   ��67   �� *RabbitsSegmentBrush+6\   �� 5d      $��Кролики��6    5e      5f      5g      5h      5i      