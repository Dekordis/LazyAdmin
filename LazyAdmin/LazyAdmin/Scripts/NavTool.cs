using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using LazyAdmin.DataBase;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace LazyAdmin
{
    partial class App
    {
        static string CiklumID = null;
        static string SerialNumber = null;
        static string Description = null;
        private static BindingList<GridDataBase> _GridDataBases;
        private static BindingList<GridDataBase> _GridDataBasesResult;
        static public void Load()
        {
            _GridDataBases = new BindingList<GridDataBase>();
            _GridDataBasesResult = new BindingList<GridDataBase>();
            _GridDataBases.ListChanged += _GridDataBases_ListChanged;
            _GridDataBasesResult.ListChanged += _GridDataBasesResult_ListChanged;
        }

        private static void _GridDataBasesResult_ListChanged(object sender, ListChangedEventArgs e)
        {
            try
            {
                if (e.ListChangedType == ListChangedType.ItemChanged || e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted) ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private static void _GridDataBases_ListChanged(object sender, ListChangedEventArgs e)
        {
            try
            {
                if (e.ListChangedType == ListChangedType.ItemChanged || e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

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
                    FullDescription += elements;
                    if (Type == -1 || Manufacturer == -1 || Model == -1)
                    {
                        try
                        {
                            _GridDataBases.Add(new GridDataBase() { CiklumIDFromDataBase = ClipBoardData[CiklumID], SerialNumberFromDataBase = ClipBoardData[SerialNumber].ToUpper(), DescriptionFromDataBase = ClipBoardData[FullDescription] });
                        }
                        catch (Exception)
                        {
                            _GridDataBases.Add(new GridDataBase() { CiklumIDFromDataBase = ClipBoardData[CiklumID], SerialNumberFromDataBase = ClipBoardData[SerialNumber].ToUpper()});
                        }
                    }
                    else if(CiklumID != SerialNumber)
                    {
                        Type += elements;
                        Manufacturer += elements;
                        Model += elements;
                        _GridDataBases.Add(new GridDataBase() { CiklumIDFromDataBase = ClipBoardData[CiklumID], SerialNumberFromDataBase = ClipBoardData[SerialNumber].ToUpper(), DescriptionFromDataBase = (ClipBoardData[Type] + ClipBoardData[Manufacturer] + ClipBoardData[Model])});
                    }
                }
                _DataGrid.ItemsSource = _GridDataBases;

            }
            catch (Exception)
            {
            }
        }
        static public void Input(DataGrid _DataGrid, string String) //input info from TextBox for DataGrids
        {
            int Number;
            bool success = int.TryParse(String, out Number);
            QRConvert(String, true, Description);
            if (Description != null);
            else if (success && (String.Length == 6 || String.Length == 13))
            {
                CiklumID = Number.ToString();
            }
            else
            {
                SerialNumber = String;
            }
            CheckingToResult();
            _DataGrid.ItemsSource = _GridDataBasesResult;
            
        }
        static public void CheckingToResult()
        {
            if (CiklumID != null && SerialNumber !=null)
            {
                _GridDataBasesResult.Add(new GridDataBase() { CiklumIDFromDataBase = CiklumID.ToString(), SerialNumberFromDataBase = SerialNumber.ToUpper()});
                CiklumID = null;
                SerialNumber = null;
                Checking();
            }
        }
        static public void Checking() //сравнение листов, сравнение строк без дескрипшена, добовление статуа.
        {
            if (_GridDataBases.Equals(_GridDataBasesResult)) MessageBox.Show("true");
        }
        //class Colums
        //{
        //    public string CiklumIDColum { get; set; }
        //    public string SerialNumberColum { get; set; }
        //    public string DesciptionColum { get; set; }
        //    public string StatusColum { get; set; }
        //}//class for adding ID for colums
        //static public void Input(DataGrid _DataGrid, string String) //input info from TextBox for DataGrids
        //{
        //    int Itteration = 0;
        //    bool Success = false;
        //    QRConvert(String, true, Description);
        //    for (int i = 0; i != String.Length; i++)
        //    {
        //        if (Char.IsNumber(String, i) == true) Itteration++;
        //        if (Itteration == String.Length -1) Success = true;
        //    }

        //    if ((String.Length == 6 || String.Length == 13) && Success == true)
        //    {
        //        CiklumID = Int32.Parse(String);
        //    }
        //    else
        //    {
        //        SerialNumber = String;
        //    }
        //    Cheking(_DataGrid, CiklumID, SerialNumber, Description);
        //}
        //static private void Cheking(DataGrid _DataGrid, int LocalCiklumID, string LocalSerialNumber, string LocalDescription) //Checking info from amt/textbox and DataGrids
        //{
        //    if (LocalCiklumID != 0 && LocalSerialNumber != null && LocalDescription != null)
        //    {
        //        _DataGrid.Items.Add(new Colums { CiklumIDColum = CiklumID.ToString(), SerialNumberColum = LocalSerialNumber, DesciptionColum = LocalDescription });
        //        CiklumID = 0;
        //        SerialNumber = null;
        //        Description = null;
        //    }
        //    else if (LocalCiklumID != 0 && SerialNumber != null && LocalDescription == null)
        //    {
        //        _DataGrid.Items.Add(new Colums { CiklumIDColum = LocalCiklumID.ToString(), SerialNumberColum = LocalSerialNumber });
        //        CiklumID = 0;
        //        SerialNumber = null;
        //    }
        //    else
        //    {
        //        MessageBox.Show($"CiklumID:{CiklumID.ToString()} \n and serialnumber:{SerialNumber}");
        //    }
        //}
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