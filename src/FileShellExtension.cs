using System.Diagnostics;
using Microsoft.Win32;

namespace VirusTotalContextMenu;

//Sample code from Ralph Arvesen (www.vertigo.com / www.lostsprings.com)
//Source: http://www.codeproject.com/Articles/15171/Simple-shell-context-menu

/// <summary>
/// Register and unregister simple shell context menus.
/// </summary>
static class FileShellExtension
{
    public static bool IsRegistered(string fileType, string shellKeyName)
    {
        string regPath = $@"Software\Classes\{fileType}\shell\{shellKeyName}";

        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(regPath);
        return key != null;
    }

    /// <summary>
    /// Register a simple shell context menu.
    /// </summary>
    /// <param name="fileType">The file type to register.</param>
    /// <param name="shellKeyName">Name that appears in the registry.</param>
    /// <param name="menuText">Text that appears in the context menu.</param>
    /// <param name="menuCommand">Command line that is executed.</param>
    public static void Register(string fileType, string shellKeyName, string menuText, string menuCommand, string iconPath)
    {
        Debug.Assert(!string.IsNullOrEmpty(fileType) && !string.IsNullOrEmpty(shellKeyName) && !string.IsNullOrEmpty(menuText) && !string.IsNullOrEmpty(menuCommand));

        // create full path to registry location
        string regPath = $@"Software\Classes\{fileType}\shell\{shellKeyName}";

        // add context menu to the registry
        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(regPath))
        {
            key.SetValue(null, menuText);
            key.SetValue("Icon", iconPath);
        }

        // add command that is invoked to the registry
        using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"{regPath}\command"))
        {
            key.SetValue(null, menuCommand);
        }
    }

    /// <summary>
    /// Unregister a simple shell context menu.
    /// </summary>
    /// <param name="fileType">The file type to unregister.</param>
    /// <param name="shellKeyName">Name that was registered in the registry.</param>
    public static void Unregister(string fileType, string shellKeyName)
    {
        Debug.Assert(!string.IsNullOrEmpty(fileType) && !string.IsNullOrEmpty(shellKeyName));

        // full path to the registry location
        string regPath = $@"Software\Classes\{fileType}\shell\{shellKeyName}";

        // remove context menu from the registry
        Registry.CurrentUser.DeleteSubKeyTree(regPath, false);
    }
}