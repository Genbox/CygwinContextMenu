using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

[assembly: CLSCompliant(true)]
namespace CygwinContextMenu
{
    static class Program
    {
        // file type to register
        const string FileType = @"Directory\Background";

        // context menu name in the registry
        const string KeyName = "opencygwin";

        // context menu text
        const string MenuText = "Open cygwin window here";

        [STAThread]
        static void Main(string[] args)
        {
            DialogResult result = MessageBox.Show("You want the installer to run for all users?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            bool allUsers = result == DialogResult.Yes;

            try
            {
                bool isRegistered = FileShellExtension.IsRegistered(FileType, KeyName, !allUsers);

                if (isRegistered)
                {
                    // unregister the context menu
                    FileShellExtension.Unregister(FileType, KeyName, !allUsers);

                    MessageBox.Show(string.Format("The '{0}' shell extension was unregistered.", MenuText), MenuText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //Find the location of cygwin.
                    string cygwinPath = @"C:\cygwin";
                    string cygwinPath64 = @"C:\cygwin64";

                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                    {
                        using (RegistryKey subKey = baseKey.OpenSubKey(@"SOFTWARE\Cygwin\setup\"))
                        {
                            if (subKey != null)
                                cygwinPath = subKey.GetValue("rootdir").ToString();
                        }
                    }

                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        using (RegistryKey subKey = baseKey.OpenSubKey(@"SOFTWARE\Cygwin\setup\"))
                        {
                            if (subKey != null)
                                cygwinPath64 = subKey.GetValue("rootdir").ToString();
                        }
                    }

                    string cygwinTTY = Path.Combine(cygwinPath, @"bin\mintty.exe");
                    string cygwinTTY64 = Path.Combine(cygwinPath64, @"bin\mintty.exe");

                    if (!File.Exists(cygwinTTY) && !File.Exists(cygwinTTY64))
                    {
                        MessageBox.Show("Cygwin is not installed. Please install Cygwin.", "Cygwin not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //see http://code.google.com/p/mintty/wiki/Tips#Starting_mintty_from_a_batch_file
                    string menuCommand = File.Exists(cygwinTTY64) ? "\"" + cygwinTTY64 + "\" /bin/env CHERE_INVOKING=1 /bin/bash -l" : "\"" + cygwinTTY + "\" /bin/env CHERE_INVOKING=1 /bin/bash -l";

                    // register the context menu
                    FileShellExtension.Register(FileType, KeyName, MenuText, menuCommand, !allUsers);

                    MessageBox.Show(string.Format("The '{0}' shell extension was registered for " + (allUsers ? "all users." : "only you."), MenuText), MenuText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception caught", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}