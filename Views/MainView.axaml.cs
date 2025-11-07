using Avalonia.Controls;
using Loudspeaker.ViewModels;

namespace Loudspeaker.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    public MainViewModel? ViewModel => DataContext as MainViewModel;
}

