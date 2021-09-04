using System;
using System.Windows;

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
    }
}