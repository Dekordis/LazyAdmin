using System;
using System.Windows;

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
            else if (Action == "Disconnect")
            {
                string StringCmdText;
                StringCmdText = @"/C cmdkey /delete:cklfilesharestorage.file.core.windows.net";
                System.Diagnostics.Process.Start("CMD.exe", StringCmdText);
                MessageBox.Show("Disconnected");
            }
            

            //Dear Serhii please create method with 2 parameters (connect, disconnect) Azure disk 
            //When method was finished it must delete all temp files and other not necessary objects (!just only use disconnect parametr!) 
        }
        private void RunScript()
        {

        }
        private void UpdateProgram()
        {
        }
    }
}