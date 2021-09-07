using System;
using System.Windows;

namespace LazyAdmin
{
    partial class App
    {
        //We must use this .CS for all methods which work with Azure 
        string Key; //This variable needed for contain Key for Azure folder
        string User; //This variable needed for contain User of Azure
        string MainPath; //This variable contain Path of programm
        string RemotePath; ////This variable contain Remote Path to Azure folder 
        public void AzureConnection()//This method needed for connect and disconnect Azure Folder
        {
            string StringCmdText;
            StringCmdText = @"/C cmdkey /add:cklfsstorage.file.core.windows.net /user:Azure\cklfsstorage /pass:NSLF6WgwRXvwHdNQxvEjZWsCMvXrsBts3H+nqnAdycFNkxROKF6U3z9MP7/Lz+kluXfjdO+ewwcISduwQlKWgA== >nul 2>&1" + "\n" + @"net use L: \\cklfsstorage.file.core.windows.net\remoteinstall /persistent:No >nul 2>&1";
            System.Diagnostics.Process.Start("CMD.exe", StringCmdText);
            MessageBox.Show("Connected");

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