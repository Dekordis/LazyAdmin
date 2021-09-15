using System;
using System.Windows;
using System.Windows.Controls;

namespace LazyAdmin
{
    partial class App
    {
        static public void Upload(DataGrid _DataGrid)
        {
            string[] StringClipBoardData = Clipboard.GetText().Split('\n'); // Count of rows
            string[] ClipBoardData = Clipboard.GetText().Split('\t');  // Array elements from ClipBoard
            int elements = StringClipBoardData[0].Split('\t').Length - 1; // Lenght of row
            int rows = StringClipBoardData.Length - 1; // Count of rows without header
            int CiklumID = Array.IndexOf(ClipBoardData, "Ciklum ID");
            int SerialNumber = Array.IndexOf(ClipBoardData, "Serial No.");
            int Type = Array.IndexOf(ClipBoardData, "Type");
            int Manufacturer = Array.IndexOf(ClipBoardData, "Manufacturer");
            int Model = Array.IndexOf(ClipBoardData, "Model");
            int Description = Array.IndexOf(ClipBoardData, "Description");
            int FullDescription = Array.IndexOf(ClipBoardData, "Full Description");
            try
            {
                for (int i = 1; i <= rows; i++)
                {
                    CiklumID += elements;
                    SerialNumber += elements;
                    Type += elements;
                    Manufacturer += elements;
                    Model += elements;
                    FullDescription += elements;
                    //_DataGrid.Rows.Add(ClipBoardData[CiklumID], ClipBoardData[SerialNumber].ToUpper(), ClipBoardData[Type] + " " + ClipBoardData[Manufacturer] + " " + ClipBoardData[Model]);
                }
            }
            catch (Exception)
            {
            }
        }
        static public void Input()
        {

        }
        static private void Cheking()
        {

        }
        static public void Checkboxes()
        {

        }
        static public void Output()
        {

        }
    }
}