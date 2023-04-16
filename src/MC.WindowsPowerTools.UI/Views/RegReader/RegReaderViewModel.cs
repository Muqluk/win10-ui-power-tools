using Microsoft.Win32;

using MC.UI.Core.MVVM;

namespace MC.UI.WindowsPowerTools.Views.RegReader {
  public class RegReaderViewModel : ViewModelBase {

    private RegistryHive _selectedHive;
    public RegistryHive SelectedHive {
      get => _selectedHive;
      set {
        if (value != _selectedHive) {
          _selectedHive = value;
          OnPropertyChanged(nameof(SelectedHive));
        }
      }
    }

    public RegReaderViewModel() { }

  }
}
