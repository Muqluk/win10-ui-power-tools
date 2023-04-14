using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;
using MC.UI.Core.MVVM;

namespace MC.UI.WindowsPowerTools {
  public partial class App : Application {
    protected override void OnStartup(StartupEventArgs args) {
      base.OnStartup(args);

      var services = new ServiceCollectionBuilder();

      services.AddWindow<MainWindow>();
      //services.ConfigureServices(services => {
      //  services.AddTransient<IInjectionService, InjectionService>();
      //});
      services.AddConfiguration();
      services.AddViewModels();
      services.Build();

      Ioc.Default.GetService<MainWindow>()!.Show();
    }
  }
}
