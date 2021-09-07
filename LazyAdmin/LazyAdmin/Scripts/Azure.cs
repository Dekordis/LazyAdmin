using System;

namespace LazyAdmin
{
    partial class App
    {
        //We must use this .CS for all methods which work with Azure 
        string Key; //This variable needed for contain Key for Azure folder
        string User; //This variable needed for contain User of Azure
        string MainPath; //This variable contain Path of programm
        string RemotePath; ////This variable contain Remote Path to Azure folder 
        private void AzureConnection()//This method needed for connect and disconnect Azure Folder
        {
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