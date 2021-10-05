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
        static string InputCiklumID = null;
        static string InputSerialNumber = null;
        static string InputDescription = null;
        private static ObservableCollection<Asset> GridOfAssets;
        private static ObservableCollection<Asset> GridOfAssetsResult;

        static public void Load()
        {
            GridOfAssets = new ObservableCollection<Asset>();
            GridOfAssetsResult = new ObservableCollection<Asset>();
        }

        static public void Upload(DataGrid _DataGrid) //uploading information from Navision
        {
            string[] StringClipBoardData = Clipboard.GetText().Split('\n'); // Count of rows
            string[] ClipBoardData = Clipboard.GetText().Split('\t');  // Array elements from ClipBoard
            int elements = StringClipBoardData[0].Split('\t').Length - 1; // Lenght of row
            int rows = StringClipBoardData.Length - 1; // Count of rows without header
            int IndexCiklumID = Array.IndexOf(ClipBoardData, "Ciklum ID");
            int IndexSerialNumber = Array.IndexOf(ClipBoardData, "Serial No.");
            int IndexType = Array.IndexOf(ClipBoardData, "Type");
            int IndexManufacturer = Array.IndexOf(ClipBoardData, "Manufacturer");
            int IndexModel = Array.IndexOf(ClipBoardData, "Model");
            int IndexDescription = Array.IndexOf(ClipBoardData, "Description");
            int IndexFullDescription = Array.IndexOf(ClipBoardData, "Full Description");
            try
            {
                for (int i = 1; i <= rows; i++)
                {
                    IndexCiklumID += elements;
                    IndexSerialNumber += elements;
                    IndexFullDescription += elements;
                    if (IndexType == -1 || IndexManufacturer == -1 || IndexModel == -1)
                    {
                        try
                        {
                            GridOfAssets.Add(new Asset() { CiklumID = ClipBoardData[IndexCiklumID], SerialNumber = ClipBoardData[IndexSerialNumber].ToUpper(), Description = ClipBoardData[IndexFullDescription] });
                        }
                        catch (Exception)
                        {
                            GridOfAssets.Add(new Asset() { CiklumID = ClipBoardData[IndexCiklumID], SerialNumber = ClipBoardData[IndexSerialNumber].ToUpper() });
                        }
                    }
                    else if (IndexCiklumID != IndexSerialNumber)
                    {
                        IndexType += elements;
                        IndexManufacturer += elements;
                        IndexModel += elements;
                        GridOfAssets.Add(new Asset() { CiklumID = ClipBoardData[IndexCiklumID], SerialNumber = ClipBoardData[IndexSerialNumber].ToUpper(), Description = (ClipBoardData[IndexType] + ClipBoardData[IndexManufacturer] + ClipBoardData[IndexModel]) });
                    }
                }
                _DataGrid.ItemsSource = GridOfAssets;
            }
            catch (Exception)
            {
            }
        }
        static public void Input(DataGrid _DataGrid, string String) //input info from TextBox for DataGrids
        {
            int Number;
            bool success = int.TryParse(String, out Number);
            QRConvert(String, true, InputDescription);
            if (InputDescription != null);
            else if (success && (String.Length == 6 || String.Length == 13))
            {
                InputCiklumID = Number.ToString();
            }
            else
            {
                InputSerialNumber = String;
            }
            CheckingToResult();
            _DataGrid.ItemsSource = GridOfAssetsResult;
            
        }
        static public void CheckingToResult()
        {
            if (InputCiklumID != null && InputSerialNumber !=null)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper()});
                InputCiklumID = null;
                InputSerialNumber = null;
            }
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