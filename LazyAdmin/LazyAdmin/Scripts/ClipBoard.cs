using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;

namespace LazyAdmin
{
    partial class App
    {
        static public void ClipBoardToNAV(string task, string[] Array) // This method must to process all information for copy it in our services
        {
            if (task == "ToNav") //Parametr which take all Variables form array and add "|" between them, for search all assets in Navision
            {
                string ClipboardInput = null;
                int Iteration = 0;
                foreach (var Variable in Array)
                {
                    ClipboardInput += Variable;
                    if (Array.Length != Iteration)
                    {
                        ClipboardInput += "|";
                    }
                    Iteration++;
                }
                Clipboard.SetText(ClipboardInput);
            }
            else if (task == "ToTicket") //Parametr which create our standart messege for copy to Jira Ticket
            {
                string ClipboardInput = null;
                bool CiklumID = false;
                foreach (var Variable in Array)
                {
                    if (CiklumID == false)
                    {
                        ClipboardInput += "Equipment with CiklumID: ";
                        ClipboardInput += Variable;
                        ClipboardInput += "\t";
                        CiklumID = true;
                    }
                    else
                    {
                        ClipboardInput += "And Serial Number: ";
                        ClipboardInput += Variable;
                        ClipboardInput += "\n";
                        CiklumID = false;
                    }
                }
                Clipboard.SetText(ClipboardInput);
            }

        }
        static public void ClipBoardToNAV(string task, string[] Array, int LengtOfString) // This method must to process all information for copy it in our services
        {
            if (task == "ToExcel") //Parametr which build text for copy it to Excel document
            {
                string ClipboardInput = null;
                int Iteration = 0;
                int EndOfString = Array.Length / LengtOfString;
                bool HeaderWasSet = false;
                foreach (var Variable in Array)
                {
                    if (Iteration == 0 && HeaderWasSet == false) //Add header to buffer
                    {
                        if (LengtOfString == 4)
                        {
                            ClipboardInput += ("Ciklum ID" + "\t" + "Serial Number" + "\t" + "Description" + "\t" + "Status" + "\n");
                        }
                        else if (LengtOfString == 3)
                        {
                            ClipboardInput += ("Ciklum ID" + "\t" + "Serial Number" + "\t" + "Description" + "\n");
                        }
                        else if (LengtOfString == 2)
                        {
                            ClipboardInput += ("Ciklum ID" + "\t" + "Serial Number" + "\n");
                        }
                        HeaderWasSet = true;
                    }
                    ClipboardInput += Variable;
                    if (Iteration == LengtOfString) //Add tab or end of string
                    {
                        ClipboardInput += "\n";
                        Iteration -= LengtOfString;
                    }
                    else
                    {
                        ClipboardInput += "\t";
                        Iteration++;
                    }
                }
                HeaderWasSet = false;
                Clipboard.SetText(ClipboardInput);
            }
        }
        static public void ChangeClipboard(string task)
        {
            string[] StringClipBoardData = Clipboard.GetText().Split('\n'); // Count of rows
            string[] ClipBoardData = Clipboard.GetText().Split('\t');  // Array elements from ClipBoard
            int elements = StringClipBoardData[0].Split('\t').Length - 1; // Lenght of row
            int rows = StringClipBoardData.Length -1; // Count of rows without header
            int CiklumID = Array.IndexOf(ClipBoardData, "Ciklum ID");
            int SerialNumber = Array.IndexOf(ClipBoardData, "Serial No.");
            int Type = Array.IndexOf(ClipBoardData, "Type");
            int Manufacturer = Array.IndexOf(ClipBoardData, "Manufacturer");
            int Model = Array.IndexOf(ClipBoardData, "Model");
            int FullDescription = Array.IndexOf(ClipBoardData, "Full Description");
            int Status = Array.IndexOf(ClipBoardData, "Status");
            int Eact = Array.IndexOf(ClipBoardData, "EAA Status");
            int Reimb = Array.IndexOf(ClipBoardData, "Reimbursable Type");
            int CurrentUser = Array.IndexOf(ClipBoardData, "Current User");
            int Branch = Array.IndexOf(ClipBoardData, "Branch");
            string Result = null; 
            try
            {
                for (int i = 1; i <= rows; i++)
                {
                    CiklumID += elements;
                    SerialNumber += elements;
                    FullDescription += elements;
                    Status += elements;
                    Eact += elements;
                    Reimb += elements;
                    CurrentUser += elements;
                    Branch += elements;
                    if (ClipBoardData[Reimb] == "Non-Reimb.") ClipBoardData[Reimb] = null;
                    if (task == "No comment")
                    {
                        if (Type == -1 || Manufacturer == -1 || Model == -1)
                        {
                            Result += $@"{ClipBoardData[FullDescription]} with CiklumID {ClipBoardData[CiklumID]} and serial number {ClipBoardData[SerialNumber]}";
                            Result += "\n \n";
                        }
                        else
                        {
                            Type += elements;
                            Manufacturer += elements;
                            Model += elements;
                            Result += $"{ClipBoardData[Reimb]} {ClipBoardData[Type]} {ClipBoardData[Manufacturer]} {ClipBoardData[Model]} with CiklumID {ClipBoardData[CiklumID]} and serial number {ClipBoardData[SerialNumber]}";
                            Result += "\n \n";
                        }
                    }
                    else if (task == "Add comment")
                    {
                        Type += elements;
                        Manufacturer += elements;
                        Model += elements;
                        if (ClipBoardData[Status] == "Operational" || ClipBoardData[Status] == "Temporary" || ClipBoardData[Status] == "Rented")
                        {
                            Result += $"{ClipBoardData[Reimb]} {ClipBoardData[Type]} {ClipBoardData[Manufacturer]} {ClipBoardData[Model]} with CiklumID {ClipBoardData[CiklumID]} and serial number {ClipBoardData[SerialNumber]} in status {ClipBoardData[Status]} assigned to {ClipBoardData[CurrentUser]}, E-Act {ClipBoardData[Eact]}";
                            Result += "\n \n";

                        }
                        else if (ClipBoardData[Status] == "To Check")
                        {
                            Result += $@"{ClipBoardData[Reimb]} {ClipBoardData[Type]} {ClipBoardData[Manufacturer]} {ClipBoardData[Model]} with CiklumID {ClipBoardData[CiklumID]} and serial number {ClipBoardData[SerialNumber]} in status {ClipBoardData[Status]} assigned to {ClipBoardData[CurrentUser]}";
                            Result += "\n \n";
                        }
                        else if (ClipBoardData[Status] == "Transfer")
                        {
                            Result += $@"{ClipBoardData[Reimb]} {ClipBoardData[Type]} {ClipBoardData[Manufacturer]} {ClipBoardData[Model]} with CiklumID {ClipBoardData[CiklumID]} and serial number {ClipBoardData[SerialNumber]} transfered to {ClipBoardData[Branch]}";
                            Result += "\n \n";
                        }
                        else
                        {
                            Result += $@"{ClipBoardData[Reimb]} {ClipBoardData[Type]} {ClipBoardData[Manufacturer]} {ClipBoardData[Model]} with CiklumID {ClipBoardData[CiklumID]} and serial number {ClipBoardData[SerialNumber]} in status {ClipBoardData[Status]}";
                            Result += "\n \n";
                        }
                    }

                }
                Clipboard.SetText(Result);
            }
            catch (Exception)
            {
            }
        }
    }
}