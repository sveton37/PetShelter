   M S B A M L   `   `   `  ����H  DPetShelter.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Lab_10_PetShelter.ViewModelsLab_10_PetShelter.ViewModels  5   	   (    "Lab_10_PetShelter.Views.AddPetView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   A
viewModels*clr-namespace:Lab_10_PetShelter.ViewModels     5   	     � Title$)  #Добавление питомца��5   	   $	��450��6$   $	��400��61    � WindowStartupLocation$ CenterOwner=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0,0,0,10q�6"   .+�5      �� $ ��Тип животного:��6   �� 5      ��6   �� *
PetTypes+ �� SelectedItem 5      �� *SelectedPetType+��5      �� 5      .R�5      �� ��6$   �� *Name+5      5      5      �� 5!   
   $��1��6   $��0,0,0,10q�6"   .+�5"      �� $��Имя:��6   �� 5#      ��6   �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=�5$      �� 5&   
   $��2��6   $��0,0,0,10q�6"   .+�5'      �� $��Возраст:��6   �� 5(      ��6   �� *Age+$ PropertyChanged=�5)      �� 5+   
   $��3��6   $��0,0,0,10q�6"   .+�5,      �� $��Вес:��6   �� 5-      ��6   �� *Weight+$ PropertyChanged=�5.      �� 50   
   $��4��6   $ ��Клаустрофобия��6    $��0,0,0,10q�52       d� 	IsChecked 51      �� *IsClaustrophobic+52      �� 55   
   $��5��6   y�6"   �� *DogPropertiesVisibility+.+�56      �� $"��Дрессированная��6   $��0,0,0,5q�58       57      �� *	IsTrained+58      �� 59      $��Тип питания:��6   �� 5:      ��6   �� *
FoodType+5;      �� 5>   
   $��5��6   y�6"   �� *CatPropertiesVisibility+.+�5?      �� $ ��Вакцинирована��6   $��0,0,0,5q�5A       5@      �� *IsVaccinated+5A      �� 5B      $��Статус:��6   �� 5C      ��6   �� *AdoptionStatus+5D      �� 5G   
   $��5��6   y�6"   �� *RabbitPropertiesVisibility+.+�5H      �� $��Тип ушей:��6   �� 5I      $��0,0,0,5q�5J      ��5I      �� *	EarType+5J      �� 5K      $��Гигант��6    5L      �� *	IsGiant+5M      �� 5P   
   $��7��6    �� Orientation$ 
Horizontal=�6"   $��Right=�5Q      $��0,10,0,0q�61   .+�5R      �� $��Добавить��6   $	��100��5T      $��30��5U      $��0,0,10,0q�5V      ��5S      �� *
AddCommand+5V      �� 5W      $��Отмена��6   $	��100��5Y      ��5X      �� *CancelCommand+5Y      5Z      5[      5\      