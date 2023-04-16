using System.Windows.Controls;

using CommunityToolkit.Mvvm.DependencyInjection;

namespace MC.UI.WindowsPowerTools.Views.RegReader {
  public partial class RegReaderView : UserControl {
    public RegReaderView() {
      DataContext = Ioc.Default.GetService<RegReaderViewModel>();
      InitializeComponent();
    }
  }
}
