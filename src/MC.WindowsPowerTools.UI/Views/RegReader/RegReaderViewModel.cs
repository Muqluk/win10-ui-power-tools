using Microsoft.Win32;

using MC.UI.Core.MVVM;

namespace MC.UI.WindowsPowerTools.Views.RegReader {
  public partial class RegReaderViewModel : ViewModelBase {
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GetSubKeysCommand))]
    private RegistryHive selectedHive = RegistryHive.CurrentUser;

    [ObservableProperty]
    private ObservableCollection<RegistryKey> subKeys = new ObservableCollection<RegistryKey>();

    [RelayCommand]
    private void GetSubKeys(RegistryHive hive) {
      using (RegistryKey key = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64)) {
        SubKeys.Clear();
        foreach (string subkeyName in key.GetSubKeyNames()) {
          var derp = key.OpenSubKey(subkeyName);
          SubKeys.Add(key.OpenSubKey(subkeyName));
        }
      }
    }



  }
}
