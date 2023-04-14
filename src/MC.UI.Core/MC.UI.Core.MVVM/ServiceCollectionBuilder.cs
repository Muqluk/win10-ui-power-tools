using System;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CommunityToolkit.Mvvm.DependencyInjection;


namespace MC.UI.Core.MVVM {
  public class ServiceCollectionBuilder {
    private readonly IServiceCollection _serviceCollection;

    public ServiceCollectionBuilder() {
      _serviceCollection = new ServiceCollection();
    }

    public ServiceCollectionBuilder ConfigureServices(Action<IServiceCollection> action) {
      action.Invoke(_serviceCollection);
      return this;
    }

    public ServiceCollectionBuilder AddViewModels() {
      var viewModels = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => type.IsSubclassOf(typeof(ViewModelBase)));

      foreach (var viewModel in viewModels) {
        _serviceCollection.AddTransient(viewModel);
      }

      return this;
    }

    public IServiceProvider Build() {
      var serviceProvider = _serviceCollection.BuildServiceProvider();
      Ioc.Default.ConfigureServices(serviceProvider);

      return serviceProvider;
    }

    public ServiceCollectionBuilder AddWindow<T>() where T : class {
      _serviceCollection.AddTransient<T>();

      return this;
    }

    public ServiceCollectionBuilder AddConfiguration(string fileName = "appsettings.json") {

      if (File.Exists($"{AppContext.BaseDirectory}/{fileName}") == false) {
        throw new FileNotFoundException($"Required file not found inMC.UI.Core.MVVM.ServiceCollectionBuilder.AddConfiguration", fileName);
      }

      var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile(fileName, false, true);

      _serviceCollection.AddSingleton(builder.Build() as IConfiguration);

      return this;
    }
  }
}
