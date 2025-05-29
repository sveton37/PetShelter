using System.Windows;
using Lab_10_PetShelter.ViewModels;

namespace Lab_10_PetShelter.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}