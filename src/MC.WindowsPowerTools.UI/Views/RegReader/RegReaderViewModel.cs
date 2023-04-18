﻿#region usings

using System.Security;

using Microsoft.Win32;

using MC.UI.Core.MVVM;

#endregion

namespace MC.UI.WindowsPowerTools.Views.RegReader {
  public partial class RegReaderViewModel : ViewModelBase {

    #region Local Constants

    const bool OPEN_AS_WRITABLE = true;
    const bool OPEN_AS_READ_ONLY = false;

    #endregion

    #region Properties

    [ObservableProperty]
    bool _hasError = false;

    [ObservableProperty]
    string _latestErrorMsg = string.Empty;
    partial void OnLatestErrorMsgChanged(string value) => HasError = value == String.Empty ? true : false;

    [ObservableProperty]
    RegistryHive _selectedHive = RegistryHive.CurrentUser;

    [ObservableProperty]
    ObservableCollection<RegistryKey> _subKeys = new();

    #endregion

    #region Event Handlers

    partial void OnSelectedHiveChanged(RegistryHive value) => OpenSelectedHive();

    #endregion

    #region Methods

    private void OpenSelectedHive() {
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

    #endregion

  }
}
