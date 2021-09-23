using System;
using System.Windows;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace LazyAdmin
{
    partial class App
    {
        //We must use this .CS for all methods which work with Azure 
        static string Key = @"NSLF6WgwRXvwHdNQxvEjZWsCMvXrsBts3H+nqnAdycFNkxROKF6U3z9MP7/Lz+kluXfjdO+ewwcISduwQlKWgA=="; //This variable needed for contain Key for Azure folder
        static string User = @"Azure\cklfsstorage"; //This variable needed for contain User of Azure
        static string MainPath; //This variable contain Path of programm
        static string RemotePath; ////This variable contain Remote Path to Azure folder 
        
        static public void AzureConnection(string Action)//This method needed for connect and disconnect Azure Folder
        {
            if (Action == "Connect")
            {
                string StringCmdText;
                StringCmdText = @$"/C cmdkey /add:cklfsstorage.file.core.windows.net /user:{User} /pass:{Key}";
                System.Diagnostics.Process.Start("CMD.exe", StringCmdText);
                MessageBox.Show("Connected");
            }
            else if (Action == "Disconnect") //disconnect not work
            {
                string StringCmdText;
                StringCmdText = @"/C cmdkey /delete:cklfilesharestorage.file.core.windows.net";
                System.Diagnostics.Process.Start("CMD.exe", StringCmdText);
                MessageBox.Show("Disconnected");
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
                ProcessStartInfo Script = new ProcessStartInfo();
                Script.FileName = @"powershell.exe";
                Script.Arguments = "\"&'" + PathtoFile + "'\"";
                Script.UseShellExecute = false;
                Script.CreateNoWindow = true;
                Process PowerShellScriptToll = new Process();
                PowerShellScriptToll.StartInfo = Script;
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

        private void UpdateProgram()
        {

        }
    }
}