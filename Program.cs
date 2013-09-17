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
            try
            {
                // process register or unregister commands
                if (args.Length == 0)
                {
                    bool isRegistered = FileShellExtension.IsRegistered(FileType, KeyName);

                    if (isRegistered)
                    {
                        // unregister the context menu
                        FileShellExtension.Unregister(FileType, KeyName);

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

                        bool cygwinTTYExists = File.Exists(cygwinTTY);
                        bool cygwinTTY64Exists = File.Exists(cygwinTTY64);

                        if (!cygwinTTYExists && !cygwinTTY64Exists)
                        {
                            MessageBox.Show("Cygwin is not installed. Please install Cygwin.", "Cygwin not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //see http://code.google.com/p/mintty/wiki/Tips#Starting_mintty_from_a_batch_file
                        string menuCommand = cygwinTTY64Exists ? "\"" + cygwinTTY64 + "\" /bin/env CHERE_INVOKING=1 /bin/bash -l" : "\"" + cygwinTTY + "\" /bin/env CHERE_INVOKING=1 /bin/bash -l";

                        // register the context menu
                        FileShellExtension.Register(FileType, KeyName, MenuText, menuCommand, true);

                        MessageBox.Show(string.Format("The '{0}' shell extension was registered.", MenuText), MenuText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception caught", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}