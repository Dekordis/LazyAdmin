using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;




namespace LazyAdmin
{
    partial class App
    {
        static List<Colums> ColumsList = new List<Colums>();
        static public void Upload(DataGrid _DataGrid) //uploading information from Navision
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
                    if (ClipBoardData[Type] == null || ClipBoardData[Manufacturer] == null || ClipBoardData[Model] == null)
                    {
                        _DataGrid.Items.Add(new Colums { CiklumIDColum = ClipBoardData[CiklumID], SerialNumberColum = ClipBoardData[SerialNumber].ToUpper(), DesciptionColum = ClipBoardData[FullDescription] });
                    }
                    else
                    {
                        _DataGrid.Items.Add(new Colums { CiklumIDColum = ClipBoardData[CiklumID], SerialNumberColum = ClipBoardData[SerialNumber].ToUpper(), DesciptionColum = (ClipBoardData[Type] + " " + ClipBoardData[Manufacturer] + " " + ClipBoardData[Model]) });
                    }
                    _DataGrid.ItemsSource = ColumsList;
                }
            }
            catch (Exception)
            {
            }
        }
        class Colums
        {
            public string CiklumIDColum { get; set; }
            public string SerialNumberColum { get; set; }
            public string DesciptionColum { get; set; }
            public string StatusColum { get; set; }
        }//class for adding ID for colums
        static public void Input(DataGrid _DataGrid, string String) //input info from TextBox for DataGrids
        {
            int Itteration = 0;
            bool Success = false;
            int CiklumID = 0;
            string SerialNumber = null;
            string Description = null; ;
            QRConvert(String, true, Description);
            for (int i = 0; i == String.Length; i++)
            {
                if (Char.IsNumber(String, i) == true) Itteration++;
                if (Itteration == String.Length) Success = true;
            }
            if ((String.Length == 6 || String.Length == 13) && Success == true)
            {
                CiklumID = Int32.Parse(String);
            }
            else
            {
                SerialNumber = String;
            }
            Cheking(_DataGrid, CiklumID, SerialNumber, Description);
        }
        static private void Cheking(DataGrid _DataGrid, int CiklumID, string SerialNumber, string Description) //Checking info from amt/textbox and DataGrids
        {
            if (CiklumID != 0 && SerialNumber != null && Description != null)
            {
                _DataGrid.Items.Add(new Colums { CiklumIDColum = CiklumID.ToString(), SerialNumberColum = SerialNumber, DesciptionColum = Description });
            }
            else if (CiklumID != 0 && SerialNumber != null && Description == null)
            {
                _DataGrid.Items.Add(new Colums { CiklumIDColum = CiklumID.ToString(), SerialNumberColum = SerialNumber});
            }
            else
            {
                MessageBox.Show($"CiklumID:{CiklumID.ToString()} \n and serialnumber:{SerialNumber}");
            }
        }
        static public void Checkboxes()
        {

        }
        static public void Output()
        {

        }
        static public void QRConvert(string String, string Output)
        {
            if (Output == "Serial Number")
            {

            }
            else if (Output == "Description")
            {

            }
        }
        static public void QRConvert(string String, bool DescriptionAndSerialNumber, string Output)
        {
            if (DescriptionAndSerialNumber == true)
            {
                String = "SerialNumber";
                Output += "Some Description";
            }
        }
    }
}