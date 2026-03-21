using System;
using Avalonia.Controls;
using TP.ConcurrentProgramming.Presentation.ViewModel;

namespace TP.ConcurrentProgramming.PresentationView;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        Random random = new Random();
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.Start(random.Next(5, 10));
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.Dispose();
        }
        base.OnClosed(e);
    }
}