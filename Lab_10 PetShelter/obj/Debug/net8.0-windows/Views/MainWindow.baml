   M S B A M L   `   `   `  ����H  DPetShelter.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Lab_10_PetShelterLab_10_PetShelter  5   	   (    "Lab_10_PetShelter.Views.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Lab_10_PetShelter     5   	     � Title$(  "Приют для животных��5   	   $	��450��6$   $	��800��61   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0,0,0,10q�6"   .+�5      �� $"��Выберите приют:��6   $��0,0,0,5q�6/   �� 5       �� DisplayMemberPath$
 Name��5      ��5      �� *
Shelters+ �� SelectedItem 5      �� *SelectedShelter+5      5      �� 5   
   $��1��6   $J��DТолько приюты с открытой территорией��5      $��0,0,0,10q�5       d� 	IsChecked 5      �� *ShowOpenYardOnly+5      �� 5"   
   $��2��6   $��0,0,0,10q�6"   .+�5#      �� $1��+Выберите тип животного:��6   $��0,0,0,5q�67   �� 5$      $
��Left=�5&      $	��200��5'      ��5$      �� *
PetTypes+ 5%      �� *SelectedPetType+��5(      �� 5)      .R�5*      �� ��6$   �� *Name+5+      5-      5.      �� 51   
   $��3��6   $��0,0,0,10q�6"   .+�53      �� $'��!Показать питомцев��6   $��30��55      $��0,0,0,10q�56      ��54      �� *ShowPetsCommand+56      �� 59      $+��%Показать статистику��6   $��30��5;      $��0,0,0,10q�5<      ��5:      �� *ShowStatisticsCommand+5<      5=      �� 5@   
   $��4��6    �� Orientation$ 
Horizontal=�6"   $��0,0,0,10q�6;   .+�5A      �� $/��)Текущий формат отчета:��6   $��Center=�66   $��0,0,5,0q�6Q   �� 5B      $	��100��5D      ��5B      �� *ReportFormats+ 5C      �� *SelectedReportFormat+5D      �� 5E      $6��0Конвертировать все отчеты��6    � ToolTip$y sКонвертирует все отчеты из текущего формата в противоположный��5G      $	��180��5H      $��30��5I      $��10,0,0,0q�5J      ��5F      �� *ConvertReportsCommand+5J      5K      5L      5M      