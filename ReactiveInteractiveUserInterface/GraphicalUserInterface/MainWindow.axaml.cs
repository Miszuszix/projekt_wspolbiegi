using System;
using Avalonia.Controls;
using TP.ConcurrentProgramming.Presentation.ViewModel;

namespace TP.ConcurrentProgramming.PresentationView;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosed(EventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.Dispose();
        }
        base.OnClosed(e);
    }

    private void OnPointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Avalonia.Controls.Control);

        if (point.Properties.IsLeftButtonPressed)
        {
            var position = point.Position;

            if (DataContext is MainWindowViewModel vm)
            {
                vm.MoveInteractiveBall(position.X, position.Y);
            }
        }
    }
}