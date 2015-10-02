using System.Diagnostics;
using Microsoft.Win32;

namespace CygwinContextMenu
{
    //Sample code from Ralph Arvesen (www.vertigo.com / www.lostsprings.com)
    //Source: http://www.codeproject.com/Articles/15171/Simple-shell-context-menu

    /// <summary>
    /// Register and unregister simple shell context menus.
    /// </summary>
    public static class FileShellExtension
    {
        public static bool IsRegistered(string fileType, string shellKeyName, bool userOnly)
        {
            string regPath = string.Format(@"Software\Classes\{0}\shell\{1}", fileType, shellKeyName);

            RegistryKey hive = userOnly ? Registry.CurrentUser : Registry.LocalMachine;

            using (RegistryKey key = hive.OpenSubKey(regPath))
            {
                return key != null;
            }
        }

        /// <summary>
        /// Register a simple shell context menu.
        /// </summary>
        /// <param name="fileType">The file type to register.</param>
        /// <param name="shellKeyName">Name that appears in the registry.</param>
        /// <param name="menuText">Text that appears in the context menu.</param>
        /// <param name="menuCommand">Command line that is executed.</param>
        /// <param name="makeExtended">True means it will only show when CTRL key is down.</param>
        public static void Register(string fileType, string shellKeyName, string menuText, string menuCommand, bool userOnly, bool makeExtended = false)
        {
            Debug.Assert(!string.IsNullOrEmpty(fileType) && !string.IsNullOrEmpty(shellKeyName) && !string.IsNullOrEmpty(menuText) && !string.IsNullOrEmpty(menuCommand));

            // create full path to registry location
            string regPath = string.Format(@"Software\Classes\{0}\shell\{1}", fileType, shellKeyName);

            RegistryKey hive = userOnly ? Registry.CurrentUser : Registry.LocalMachine;

            // add context menu to the registry
            using (RegistryKey key = hive.CreateSubKey(regPath))
            {
                if (key != null) key.SetValue(null, menuText);

                if (makeExtended)
                {
                    if (key != null) key.SetValue("Extended", string.Empty);
                }
            }

            // add command that is invoked to the registry
            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(string.Format(@"{0}\command", regPath)))
            {
                if (key != null) key.SetValue(null, menuCommand);
            }
        }

        /// <summary>
        /// Unregister a simple shell context menu.
        /// </summary>
        /// <param name="fileType">The file type to unregister.</param>
        /// <param name="shellKeyName">Name that was registered in the registry.</param>
        public static void Unregister(string fileType, string shellKeyName, bool userOnly)
        {
            Debug.Assert(!string.IsNullOrEmpty(fileType) &&
                !string.IsNullOrEmpty(shellKeyName));

            // full path to the registry location			
            string regPath = string.Format(@"Software\Classes\{0}\shell\{1}", fileType, shellKeyName);

            RegistryKey hive = userOnly ? Registry.CurrentUser : Registry.LocalMachine;

            // remove context menu from the registry
            hive.DeleteSubKeyTree(regPath, false);
        }
    }
}