using System.Security;

using Microsoft.Win32;

using MC.UI.Core.MVVM;

namespace MC.UI.WindowsPowerTools.Views.RegReader {
  public partial class RegReaderViewModel : ViewModelBase {
    const bool OPEN_AS_WRITABLE = true;
    const bool OPEN_AS_READ_ONLY = false;


    [ObservableProperty]
    bool _hasError = false;

    [ObservableProperty]
    string _latestErrorMsg = string.Empty;
    partial void OnLatestErrorMsgChanged(string value) => HasError = value == String.Empty ? true : false;

    [ObservableProperty]
    RegistryHive _selectedHive = RegistryHive.CurrentUser;

    [ObservableProperty]
    ObservableCollection<RegistryKey> _subKeys = new();
    partial void OnSelectedHiveChanging(RegistryHive value) => System.Diagnostics.Debug.WriteLine($"OnSelectedHiveChanging: {SelectedHive.ToString()}");
    partial void OnSelectedHiveChanged(RegistryHive value) {
      SubKeys.Clear();

      using RegistryKey key = RegistryKey.OpenBaseKey(SelectedHive, RegistryView.Registry32);

      foreach (string subkeyName in key.GetSubKeyNames()) {
        try {
          SubKeys.Add(key.OpenSubKey(subkeyName, OPEN_AS_READ_ONLY)!);
        } catch (SecurityException e) { //  only SecurityExceptions.
          LogError(e, subkeyName);
        } catch (Exception e) { //          any other exception type...
          LogError(e, subkeyName);
        }
      }
    }



    public RegReaderViewModel() {
    }

    private void LogError(Exception e, string key) {
      LatestErrorMsg = $"{e.GetType().FullName} - {e.Message}";

#if DEBUG
      System.Diagnostics.Debug.WriteLine($"{e.GetType().FullName} - {e.Message}");
      System.Diagnostics.Debug.WriteLine(($"{("Registry Hive:").PadRight(15, ' ')} {SelectedHive.ToString().PadLeft(15, ' ')}").PadLeft(5, ' '));
      System.Diagnostics.Debug.WriteLine(($"{("Key:").PadRight(15, ' ')} {key.PadLeft(15, ' ')}").PadLeft(5, ' '));
#else
      // Actually pipe to logging solution.
#endif
    }
  }
}
