using System;
using System.Windows;
using System.Management.Automation;
using System.IO;
using System.Diagnostics;

namespace LazyAdmin
{
    partial class App
    {
        //We must use this .CS for all methods which work with Azure 
        static readonly string Key = @"NSLF6WgwRXvwHdNQxvEjZWsCMvXrsBts3H+nqnAdycFNkxROKF6U3z9MP7/Lz+kluXfjdO+ewwcISduwQlKWgA=="; //This variable needed for contain Key for Azure folder
        static readonly string User = @"Azure\cklfsstorage"; //This variable needed for contain User of Azure

        static public void AzureConnection(string Action)//This method needed for connect and disconnect Azure Folder
        {
            if (Action == "Connect")
            {
                var ConnectionToAzure = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        //WindowStyle = ProcessWindowStyle.Hidden,
                        //UseShellExecute = false,
                        CreateNoWindow = true,
                        FileName = "cmd.exe",
                        Arguments = $@"/C cmdkey /add:cklfilesharestorage.file.core.windows.net\remoteinstall /user:{User} /pass:{Key}"
                    }
                };
                ConnectionToAzure.Start();
            }
            else if (Action == "Disconnect") //disconnect not work
            {
                var DisconnectAzure = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        FileName = "cmd.exe",
                        Arguments = @"/C cmdkey /delete:cklfilesharestorage.file.core.windows.net"
                    }
                };
                DisconnectAzure.Start();
            }


            //Dear Serhii please create method with 2 parameters (connect, disconnect) Azure disk 
            //When method was finished it must delete all temp files and other not necessary objects (!just only use disconnect parametr!) 
        }
        static public void RunScript(string PathtoFile, string TypeOfRun)
        {

            if (TypeOfRun == "Single")  //простой запус пс1
            {
                PowerShell Script = PowerShell.Create();
                Script.AddScript(PathtoFile);
                Script.Invoke();
            }

            else if (TypeOfRun == "Multi")  //запуск пс1 новым процессом
            {
                ProcessStartInfo Script = new()
                {
                    FileName = @"powershell.exe",
                    Arguments = "\"&'" + PathtoFile + "'\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process PowerShellScriptToll = new()
                {
                    StartInfo = Script
                };
                PowerShellScriptToll.Start();
            }
        }
        static public void RunScript(string[] PathtoFile)
        {
            PowerShell Script = PowerShell.Create();
            foreach (var Path in PathtoFile)
            {
                Script.AddScript(Path);
            }
            Script.Invoke();
        }
        static public void GetFilesFromFolder(string Pathtofolder, string ExtentionFile)
        {
            string[] files = System.IO.Directory.GetFiles(Pathtofolder, $"*.{ExtentionFile}");//set path to folder with files in var PathToFolder, set extention of needed files 
            foreach (var file in files)
            {
                MessageBox.Show($"{file}");
            }
        }
    }
}