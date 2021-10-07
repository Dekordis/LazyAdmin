using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using LazyAdmin.DataBase;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Timers;

namespace LazyAdmin
{
    partial class App
    {
        private static readonly string PATHFromAMT = $"{Environment.CurrentDirectory}\\GridOfAssetsFromAMT.json";
        private static readonly string PATHResult = $"{Environment.CurrentDirectory}\\GridOfAssetsResult.json";
        static string InputCiklumID = null;
        static string InputSerialNumber = null;
        static string InputWrongCiklumID = null;
        static string InputWrongSerialNumber = null;
        static string InputDescription = null;
        static string ColumnOfDataGrid = null;
        static int IndexOfAsset = -1;
        static int IndexOfSerialNumberAsset = -1;
        static int IndexOfCiklumIDAsset = -1;
        private static ObservableCollection<Asset> GridOfAssets;
        private static ObservableCollection<Asset> GridOfAssetsResult;
        private static ObservableCollection<Asset> GridOfAssetsCheking;
        private static FileIOService FileIOServiceFromAMT;
        private static FileIOService FileIOServiceResult;
        private static System.Timers.Timer Timer;

        static public void Load(DataGrid _DataGridFromAMT, DataGrid _DataGridResult)
        {
            FileIOServiceFromAMT = new FileIOService(PATHFromAMT);
            FileIOServiceResult = new FileIOService(PATHResult);
            try
            {
                GridOfAssets = FileIOServiceFromAMT.LoadData();
                GridOfAssetsResult = FileIOServiceResult.LoadData();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                GridOfAssets = new ObservableCollection<Asset>();
                GridOfAssetsResult = new ObservableCollection<Asset>();
            }
            _DataGridFromAMT.ItemsSource = GridOfAssets;
            _DataGridResult.ItemsSource = GridOfAssetsResult;

            GridOfAssets.CollectionChanged += GridOfAssets_CollectionChanged;
            GridOfAssetsResult.CollectionChanged += GridOfAssets_CollectionChanged;
            SetTimer(30000);

        }
        private static void AutoSave(Object source, ElapsedEventArgs e)
        {
            try
            {
                FileIOServiceFromAMT.SaveData(GridOfAssets);
                FileIOServiceResult.SaveData(GridOfAssetsResult);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private static void GridOfAssets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove)
            {
                try
                {
                    FileIOServiceFromAMT.SaveData(GridOfAssets);
                    FileIOServiceResult.SaveData(GridOfAssetsResult);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
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
            }
            catch (Exception)
            {
            }
        }
        static public void Input(string String)
        {
            int Number;
            Search(GridOfAssets, String);
            bool success = int.TryParse(String, out Number);
            if (IndexOfCiklumIDAsset != -1 && ColumnOfDataGrid == "CiklumID")
            {
                InputCiklumID = String;
                ColumnOfDataGrid = null;
            }
            else if (IndexOfSerialNumberAsset != -1 && ColumnOfDataGrid == "SerialNumber")
            {
                InputSerialNumber = String;
                ColumnOfDataGrid = null;
            }
            else if (success && (String.Length == 6 || String.Length == 13))
            {
                InputWrongCiklumID = Number.ToString();
            }
            else
            {
                InputWrongSerialNumber = String;
            }
            if (IndexOfCiklumIDAsset != -1 || IndexOfSerialNumberAsset != -1) CheckingToResult(IndexOfCiklumIDAsset);
            else if (InputWrongSerialNumber != null && InputWrongCiklumID != null) CheckingToResult();

        }
        static public void InputSending(DataGrid _DataGrid, string String) //input info from TextBox for DataGrids
        {
            int Number;
            bool success = int.TryParse(String, out Number);
            QRConvert(String, true, InputDescription);
            if (InputDescription != null) ;
            else if (success && (String.Length == 6 || String.Length == 13))
            {
                InputCiklumID = Number.ToString();
            }
            else
            {
                InputSerialNumber = String;
            }
            CheckingToResult();
        }
        static public void CheckingToResult()
        {
            if (InputCiklumID != null && InputSerialNumber != null)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper() });
                InputCiklumID = null;
                InputSerialNumber = null;
            }
            else if (InputWrongCiklumID != null && InputWrongSerialNumber != null)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputWrongCiklumID.ToString(), SerialNumber = InputWrongSerialNumber.ToUpper(), Status = "No In AMT" });
                InputWrongCiklumID = null;
                InputWrongSerialNumber = null;
            }
        }
        static public void CheckingToResult(int index)
        {
            if (InputCiklumID != null && InputSerialNumber != null && IndexOfCiklumIDAsset == IndexOfSerialNumberAsset)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper(), Description = GridOfAssets[IndexOfCiklumIDAsset].Description, Status = "Ok" });
                GridOfAssets.RemoveAt(index);
                InputCiklumID = null;
                InputSerialNumber = null;
                IndexOfCiklumIDAsset = -1;
                IndexOfSerialNumberAsset = -1;
                IndexOfAsset = -1;
                ColumnOfDataGrid = null;
            }
            else if (InputCiklumID != null && InputSerialNumber != null && IndexOfCiklumIDAsset != IndexOfSerialNumberAsset)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper(), Status = "CiklumID not equal Serial Number" });
                GridOfAssets.RemoveAt(index);
                InputCiklumID = null;
                InputSerialNumber = null;
                IndexOfCiklumIDAsset = -1;
                IndexOfSerialNumberAsset = -1;
                IndexOfAsset = -1;
                ColumnOfDataGrid = null;
            }
            else if (InputCiklumID != null && InputWrongSerialNumber != null)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputWrongSerialNumber.ToUpper(), Description = GridOfAssets[IndexOfCiklumIDAsset].Description, Status = "Wrong SerialNumber" });
                InputCiklumID = null;
                InputWrongSerialNumber = null;
                IndexOfCiklumIDAsset = -1;
                IndexOfSerialNumberAsset = -1;
                IndexOfAsset = -1;
                ColumnOfDataGrid = null;
            }
            else if (InputWrongCiklumID != null && InputSerialNumber != null)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputWrongCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper(), Description = GridOfAssets[IndexOfSerialNumberAsset].Description, Status = "Wrong CiklumID" });
                InputWrongCiklumID = null;
                InputSerialNumber = null;
                IndexOfCiklumIDAsset = -1;
                IndexOfSerialNumberAsset = -1;
                IndexOfAsset = -1;
                ColumnOfDataGrid = null;
            }
        }
        private static void Search(ObservableCollection<Asset> source, string input) //Method for searching 1. Index 2. Column. You need add some binding list, string with searching text and empty variable for result(column, index(int!))
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i].CiklumID.IndexOf(input) != -1)
                {
                    IndexOfCiklumIDAsset = i;
                    ColumnOfDataGrid = "CiklumID";
                    break;
                }
                else if (source[i].SerialNumber.IndexOf(input) != -1)
                {
                    IndexOfSerialNumberAsset = i;
                    ColumnOfDataGrid = "SerialNumber";
                    break;
                }
                else if (source[i].AssetRow.IndexOf(input) != -1)
                {
                    IndexOfAsset = i;
                    ColumnOfDataGrid = "AssetRow";
                    break;
                }
            }
        }
        private static void Search(ObservableCollection<Asset> source, string input, string status) //Method for searching 1. Index 2. Column. You need add some binding list, string with searching text and empty variable for result(column, index(int!))
        {
            if (status == "Status")
                for (int i = 0; i < source.Count; i++)
                {
                    if (source[i].Status.IndexOf(input) != -1)
                    {
                        IndexOfAsset = source[i].Status.IndexOf(input);
                        break;
                    }
                };
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
        static public void ClearAssets()
        {
            GridOfAssets.Clear();
            GridOfAssetsResult.Clear();
        }
        static public void FinishSending()
        {
            if (GridOfAssets.Count == GridOfAssetsResult.Count)
            {
                int rowcount = 0;
                for (int i = 0; i < GridOfAssets.Count; i++)
                {
                    for (int b = 0; b < GridOfAssets.Count; b++)
                    {
                        if (GridOfAssets[i].AssetRow == GridOfAssetsResult[b].AssetRow) rowcount++;
                    }
                }
                if (rowcount == GridOfAssets.Count)
                {
                    GridOfAssetsResult.Clear();
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        GridOfAssetsResult.Add(new Asset { CiklumID = GridOfAssets[i].CiklumID, SerialNumber = GridOfAssets[i].SerialNumber, Description = GridOfAssets[i].Description, Status = "Prepared to delivery" });
                    }
                    GridOfAssets.Clear();
                    MessageBox.Show("Done!", "Cheking");
                }
                else
                {
                    GridOfAssetsCheking = new ObservableCollection<Asset>();
                    //-------------------------------
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        GridOfAssetsCheking.Add(new Asset { CiklumID = GridOfAssets[i].CiklumID, SerialNumber = GridOfAssets[i].SerialNumber });
                    }
                    string result = "Next assets from AMT: \n";
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        for (int b = 0; b < GridOfAssetsCheking.Count; b++)
                        {
                            if (GridOfAssetsResult[i].AssetRow == GridOfAssetsCheking[b].AssetRow)
                            {
                                GridOfAssetsCheking.RemoveAt(b);
                            }
                        }
                    }
                    for (int b = 0; b < GridOfAssetsCheking.Count; b++)
                    {
                        result += $"Ciklum ID: {GridOfAssetsCheking[b].CiklumID} SerialNumber: {GridOfAssetsCheking[b].SerialNumber} \n";
                    }
                    //-----------------------------------------
                    GridOfAssetsCheking.Clear();
                    //-----------------------------------------
                    for (int i = 0; i < GridOfAssetsResult.Count; i++)
                    {
                        GridOfAssetsCheking.Add(new Asset { CiklumID = GridOfAssetsResult[i].CiklumID, SerialNumber = GridOfAssetsResult[i].SerialNumber });
                    }
                    result += "Not equivalent to nexr scanned assets: \n";
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        for (int b = 0; b < GridOfAssetsCheking.Count; b++)
                        {
                            if (GridOfAssets[i].AssetRow == GridOfAssetsCheking[b].AssetRow)
                            {
                                GridOfAssetsCheking.RemoveAt(b);
                            }
                        }
                    }
                    for (int b = 0; b < GridOfAssetsCheking.Count; b++)
                    {
                        result += $"Ciklum ID: {GridOfAssetsCheking[b].CiklumID} SerialNumber: {GridOfAssetsCheking[b].SerialNumber} \n";
                    }
                    //-----------------------------------------
                    MessageBox.Show(result);
                    GridOfAssetsCheking.Clear();
                }
            }
            else
            {
                if (GridOfAssets.Count >= GridOfAssetsResult.Count)
                {
                    GridOfAssetsCheking = new ObservableCollection<Asset>();
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        GridOfAssetsCheking.Add(new Asset { CiklumID = GridOfAssets[i].CiklumID, SerialNumber = GridOfAssets[i].SerialNumber });
                    }
                    for (int i = 0; i < GridOfAssetsResult.Count; i++)
                    {
                        for (int b = 0; b < GridOfAssetsCheking.Count; b++)
                        {
                            if (GridOfAssetsResult[i].AssetRow == GridOfAssetsCheking[b].AssetRow)
                            {
                                GridOfAssetsCheking.RemoveAt(b);
                            }
                        }
                    }
                    string result = "Next assets not scanned \n";
                    for (int i = 0; i < GridOfAssetsCheking.Count; i++)
                    {
                        result += $"CiklumID: {GridOfAssetsCheking[i].CiklumID} ";
                        result += $"SerialNumber: {GridOfAssetsCheking[i].SerialNumber} \n";
                    }

                    MessageBox.Show(result);
                    GridOfAssetsCheking.Clear();
                    result = null;
                }
                else if (GridOfAssets.Count <= GridOfAssetsResult.Count)
                {
                    GridOfAssetsCheking = new ObservableCollection<Asset>();
                    for (int i = 0; i < GridOfAssetsResult.Count; i++)
                    {
                        GridOfAssetsCheking.Add(new Asset { CiklumID = GridOfAssetsResult[i].CiklumID, SerialNumber = GridOfAssetsResult[i].SerialNumber });
                    }
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        for (int b = 0; b < GridOfAssetsCheking.Count; b++)
                        {
                            if (GridOfAssets[i].AssetRow == GridOfAssetsCheking[b].AssetRow)
                            {
                                GridOfAssetsCheking.RemoveAt(b);
                            }
                        }
                    }
                    string result = "Next assets NO IN AMT \n";
                    for (int i = 0; i < GridOfAssetsCheking.Count; i++)
                    {
                        result += $"CiklumID: {GridOfAssetsCheking[i].CiklumID} ";
                        result += $"SerialNumber: {GridOfAssetsCheking[i].SerialNumber} \n";
                    }

                    MessageBox.Show(result);
                    GridOfAssetsCheking.Clear();
                    result = null;
                }
            }

        }
        private static void SetTimer(int interval)
        {
            // Create a timer with a two second interval.
            Timer = new Timer(interval);
            // Hook up the Elapsed event for the timer. 
            Timer.Elapsed += AutoSave;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

    }
}