using System;
using System.Windows;
using System.Windows.Controls;
using LazyAdmin.DataBase;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Timers;
using System.Media;

namespace LazyAdmin
{
    partial class App
    {
        #region Variables
        private static readonly string PATHFromAMT = $"{Environment.CurrentDirectory}\\GridOfAssetsFromAMT.json";
        private static readonly string PATHResult = $"{Environment.CurrentDirectory}\\GridOfAssetsResult.json";
        private static string InputCiklumID = null;
        private static string InputSerialNumber = null;
        private static string InputWrongCiklumID = null;
        private static string InputWrongSerialNumber = null;
        private static string ColumnOfDataGrid = null;
        private static int IndexOfAsset = -1;
        private static bool AssetCreated = false;
        private static int IndexOfSerialNumberAsset = -1;
        private static int IndexOfCiklumIDAsset = -1;
        private static ObservableCollection<Asset> GridOfAssets;
        private static ObservableCollection<Asset> GridOfAssetsResult;
        private static ObservableCollection<Asset> GridOfAssetsChecking;
        private static FileIOService FileIOServiceFromAMT;
        private static FileIOService FileIOServiceResult;
        private static Timer Timer;
        #region Status Library
        private static string Ok = "Ok";
        private static string NoInAMT = "No in AMT";
        private static string WrongCiklumID = "Wrong CiklumID";
        private static string WrongSerialNumber = "Wrong S/N";
        private static string CiklumIDNotMutchSerialNumber = "CiklumID does not match S/N";
        private static string StickCiklumID = "Stick CiklumID";
        private static string FixedNoInAMT = "Equipment was added to AMT";
        private static string FixedWrongCiklumID = "CiklumID was changed to correct";
        private static string FixedWrongSerialNumber = "S/N was changed to correct";
        private static string FixedCiklumIDNotMutchSerialNumber = "CiklumID and S/N were changed to correct";
        private static string FixedStickCiklumID = "CiklumID has been sticked";
        private static string PreparedToDelivery = "Was prepared to delivery";
        private static string[] FixedStatusLibrary = { FixedNoInAMT, FixedWrongCiklumID, FixedWrongSerialNumber, FixedCiklumIDNotMutchSerialNumber, FixedStickCiklumID };
        private static string[] StatusLibrary = { NoInAMT, WrongCiklumID, WrongSerialNumber, CiklumIDNotMutchSerialNumber, StickCiklumID };
        #endregion
        #endregion
        static public void Load(DataGrid _DataGridFromAMT, DataGrid _DataGridResult) //Things which load on start
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
            SetTimer(15000);
        }
        #region Events for Saving
        private static void AutoSave(Object source, ElapsedEventArgs e) //Autosaving event
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
        private static void GridOfAssets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) //Autosaving event
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
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
        #endregion
        #region Action with adding info to DataGrid
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
                        GridOfAssets.Add(new Asset() { CiklumID = ClipBoardData[IndexCiklumID], SerialNumber = ClipBoardData[IndexSerialNumber].ToUpper(), Description = (ClipBoardData[IndexType] + " " + ClipBoardData[IndexManufacturer] + " " + ClipBoardData[IndexModel]) });
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        static public void AddID(string Input)
        {
            bool success = long.TryParse(Input, out long Number);
            if (success && (Number.ToString().Length == 13 || Number.ToString().Length == 6))
            {
                if(InputCiklumID != null) SoundPlay("Repeat");
                InputCiklumID = Input;
            }
            else
            {
                if (InputCiklumID != null) SoundPlay("Repeat");
                InputSerialNumber = Input;
            }
            CreateAsset(GridOfAssets, GridOfAssetsResult);
        }
        static private void CreateAsset(ObservableCollection<Asset> Collection, ObservableCollection<Asset> CollectionResult)
        {
            if(InputCiklumID != null && InputSerialNumber != null)
            {
                CollectionResult.Add(new Asset { CiklumID = InputCiklumID, SerialNumber = InputSerialNumber });
                for (int i = 0; i < CollectionResult.Count; i++)
                {
                    if (CollectionResult[i].AssetRow == InputCiklumID + InputSerialNumber)
                    {
                        for (int b = 0; b < Collection.Count; b++)
                        {
                            if (CollectionResult[i].AssetRow == Collection[b].AssetRow)
                            {
                                CollectionResult[i].Description = Collection[b].Description;
                                if (InputCiklumID.Length == 6) CollectionResult[i].Status = Ok;
                                else CollectionResult[i].Status = StickCiklumID;
                                Collection.RemoveAt(b);
                                ClearVariable();
                                SoundPlay("Accept");
                                return;
                            }
                            else if (CollectionResult[i].CiklumID == Collection[b].CiklumID)
                            {
                                CollectionResult[i].Description = Collection[b].Description;
                                for (int s = 0; s < Collection.Count; s++)
                                {
                                    if (CollectionResult[i].SerialNumber == Collection[s].SerialNumber)
                                    {
                                        CollectionResult[i].Status = CiklumIDNotMutchSerialNumber;
                                        ClearVariable();
                                        SoundPlay("Error");
                                        return;
                                    }
                                }
                                CollectionResult[i].Status = WrongSerialNumber;
                                ClearVariable();
                                SoundPlay("Error");
                                return;
                            }
                            else if (CollectionResult[i].SerialNumber == Collection[b].SerialNumber)
                            {
                                CollectionResult[i].Description = Collection[b].Description;
                                for (int s = 0; s < Collection.Count; s++)
                                {
                                    if (CollectionResult[i].CiklumID == Collection[s].CiklumID)
                                    {
                                        CollectionResult[i].Status = CiklumIDNotMutchSerialNumber;
                                        ClearVariable();
                                        SoundPlay("Error");
                                        return;
                                    }
                                }
                                CollectionResult[i].Status = WrongCiklumID;
                                ClearVariable();
                                SoundPlay("Error");
                                return;
                            }
                        }
                        CollectionResult[i].Status = NoInAMT;
                        ClearVariable();
                        SoundPlay("Error");
                        return;
                    }
                }
            }
        }

        static public void Input(string String) //input info from TextBox for DataGrids
        {
            Search(GridOfAssets, String);
            bool success = long.TryParse(String, out long Number);
            if (IndexOfCiklumIDAsset != -1 && ColumnOfDataGrid == "CiklumID")
            {
                if (InputCiklumID != null)
                {
                    SoundPlay("Repeat");
                }
                InputCiklumID = String;
                ColumnOfDataGrid = null;
            }
            else if (IndexOfSerialNumberAsset != -1 && ColumnOfDataGrid == "SerialNumber")
            {
                if (InputSerialNumber != null)
                {
                    SoundPlay("Repeat");
                }
                InputSerialNumber = String;
                ColumnOfDataGrid = null;
            }
            else if (success && (Number.ToString().Length == 13 || Number.ToString().Length == 6))
            {
                if (InputWrongCiklumID != null)
                {
                    SoundPlay("Repeat");
                }
                InputWrongCiklumID = Number.ToString();
            }
            else
            {
                if (InputWrongSerialNumber != null)
                {
                    SoundPlay("Repeat");
                }
                InputWrongSerialNumber = String;
            }
            if (IndexOfCiklumIDAsset != -1 || IndexOfSerialNumberAsset != -1) CheckingToResult(IndexOfCiklumIDAsset);
            else if (InputWrongSerialNumber != null && InputWrongCiklumID != null) CheckingToResult();

        }
        static public void InputSending(DataGrid _DataGrid, string String) //input info from TextBox for DataGrids
        {
            bool success = int.TryParse(String, out int Number);
            if (success && (Number.ToString().Length == 13 || Number.ToString().Length == 6))
            {
                InputCiklumID = Number.ToString();
            }
            else
            {
                InputSerialNumber = String;
            }
            CheckingToResult();
        }
        static public void CheckingToResult() //Method for cheking input info and add it to DataGrid
        {
            if (InputCiklumID != null && InputSerialNumber != null)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper() });
                ClearVariable();
                SoundPlay("Accept");
            }
            else if (InputWrongCiklumID != null && InputWrongSerialNumber != null)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputWrongCiklumID.ToString(), SerialNumber = InputWrongSerialNumber.ToUpper(), Status = NoInAMT });
                ClearVariable();
                SoundPlay("Error");
            }
        }
        static public void CheckingToResult(int index) //Method for cheking input info and add it to DataGrid
        {
            if (InputCiklumID != null && InputSerialNumber != null && IndexOfCiklumIDAsset == IndexOfSerialNumberAsset && InputCiklumID.Length != 13)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper(), Description = GridOfAssets[IndexOfCiklumIDAsset].Description, Status = Ok });
                GridOfAssets.RemoveAt(index);
                ClearVariable();
                SoundPlay("Accept");
            }
            else if (InputCiklumID != null && InputSerialNumber != null && IndexOfCiklumIDAsset == IndexOfSerialNumberAsset && InputCiklumID.Length == 13)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper(), Description = GridOfAssets[IndexOfCiklumIDAsset].Description, Status = StickCiklumID });
                GridOfAssets.RemoveAt(index);
                ClearVariable();
                SoundPlay("Accept");
            }
            else if (InputCiklumID != null && InputSerialNumber != null && IndexOfCiklumIDAsset != IndexOfSerialNumberAsset)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper(), Status = CiklumIDNotMutchSerialNumber });
                ClearVariable();
                SoundPlay("Error");
            }
            else if (InputCiklumID != null && InputWrongSerialNumber != null)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputCiklumID.ToString(), SerialNumber = InputWrongSerialNumber.ToUpper(), Description = GridOfAssets[IndexOfCiklumIDAsset].Description, Status = WrongSerialNumber });
                ClearVariable();
                SoundPlay("Error");
            }
            else if (InputWrongCiklumID != null && InputSerialNumber != null)
            {
                GridOfAssetsResult.Add(new Asset() { CiklumID = InputWrongCiklumID.ToString(), SerialNumber = InputSerialNumber.ToUpper(), Description = GridOfAssets[IndexOfSerialNumberAsset].Description, Status = WrongCiklumID });
                ClearVariable();
                SoundPlay("Error");
            }
        }
        private static void ClearVariable()
        {
            InputWrongCiklumID = null;
            InputWrongSerialNumber = null;
            InputCiklumID = null;
            InputSerialNumber = null;
            IndexOfCiklumIDAsset = -1;
            IndexOfSerialNumberAsset = -1;
            IndexOfAsset = -1;
            ColumnOfDataGrid = null;
        } //Method for clearing temporary variables
        #endregion
        #region Some additional methods
        private static void Search(ObservableCollection<Asset> source, string input) //Method for searching 1. Index 2. Column. You need add some binding list, string with searching text and empty variable for result(column, index(int!))
        {
            for (int i = 0; i < source.Count; i++)
            {
                if ((source[i].CiklumID != null) && (source[i].CiklumID.IndexOf(input) != -1))
                {
                    IndexOfCiklumIDAsset = i;
                    ColumnOfDataGrid = "CiklumID";
                    break;
                }
                else if ((source[i].SerialNumber != null) && (source[i].SerialNumber.IndexOf(input) != -1))
                {
                    IndexOfSerialNumberAsset = i;
                    ColumnOfDataGrid = "SerialNumber";
                    break;
                }
                else if ((source[i].AssetRow != null) && (source[i].AssetRow.IndexOf(input) != -1))
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
        static public void ClearAssets() //Clearing GrindOfAsset
        {
            GridOfAssets.Clear();
            GridOfAssetsResult.Clear();
        }
        static public void FinishSending() //Method for equal 2 observables collection (GridOfAssetResult and GridOfAsset)
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
                        GridOfAssetsResult.Add(new Asset { CiklumID = GridOfAssets[i].CiklumID, SerialNumber = GridOfAssets[i].SerialNumber, Description = GridOfAssets[i].Description, Status = PreparedToDelivery });
                    }
                    GridOfAssets.Clear();
                    MessageBox.Show("Done!", "Cheking");
                }
                else
                {
                    GridOfAssetsChecking = new ObservableCollection<Asset>();
                    //-------------------------------
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        GridOfAssetsChecking.Add(new Asset { CiklumID = GridOfAssets[i].CiklumID, SerialNumber = GridOfAssets[i].SerialNumber });
                    }
                    string result = "Next assets from AMT: \n";
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        for (int b = 0; b < GridOfAssetsChecking.Count; b++)
                        {
                            if (GridOfAssetsResult[i].AssetRow == GridOfAssetsChecking[b].AssetRow)
                            {
                                GridOfAssetsChecking.RemoveAt(b);
                            }
                        }
                    }
                    for (int b = 0; b < GridOfAssetsChecking.Count; b++)
                    {
                        result += $"Ciklum ID: {GridOfAssetsChecking[b].CiklumID} SerialNumber: {GridOfAssetsChecking[b].SerialNumber} \n";
                    }
                    //-----------------------------------------
                    GridOfAssetsChecking.Clear();
                    //-----------------------------------------
                    for (int i = 0; i < GridOfAssetsResult.Count; i++)
                    {
                        GridOfAssetsChecking.Add(new Asset { CiklumID = GridOfAssetsResult[i].CiklumID, SerialNumber = GridOfAssetsResult[i].SerialNumber });
                    }
                    result += "Not equivalent to nexr scanned assets: \n";
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        for (int b = 0; b < GridOfAssetsChecking.Count; b++)
                        {
                            if (GridOfAssets[i].AssetRow == GridOfAssetsChecking[b].AssetRow)
                            {
                                GridOfAssetsChecking.RemoveAt(b);
                            }
                        }
                    }
                    for (int b = 0; b < GridOfAssetsChecking.Count; b++)
                    {
                        result += $"Ciklum ID: {GridOfAssetsChecking[b].CiklumID} SerialNumber: {GridOfAssetsChecking[b].SerialNumber} \n";
                    }
                    //-----------------------------------------
                    MessageBox.Show(result);
                    GridOfAssetsChecking.Clear();
                }
            }
            else
            {
                if (GridOfAssets.Count >= GridOfAssetsResult.Count)
                {
                    GridOfAssetsChecking = new ObservableCollection<Asset>();
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        GridOfAssetsChecking.Add(new Asset { CiklumID = GridOfAssets[i].CiklumID, SerialNumber = GridOfAssets[i].SerialNumber });
                    }
                    for (int i = 0; i < GridOfAssetsResult.Count; i++)
                    {
                        for (int b = 0; b < GridOfAssetsChecking.Count; b++)
                        {
                            if (GridOfAssetsResult[i].AssetRow == GridOfAssetsChecking[b].AssetRow)
                            {
                                GridOfAssetsChecking.RemoveAt(b);
                            }
                        }
                    }
                    string result = "Next assets not scanned \n";
                    for (int i = 0; i < GridOfAssetsChecking.Count; i++)
                    {
                        result += $"CiklumID: {GridOfAssetsChecking[i].CiklumID} ";
                        result += $"SerialNumber: {GridOfAssetsChecking[i].SerialNumber} \n";
                    }

                    MessageBox.Show(result);
                    GridOfAssetsChecking.Clear();
                    result = null;
                }
                else if (GridOfAssets.Count <= GridOfAssetsResult.Count)
                {
                    GridOfAssetsChecking = new ObservableCollection<Asset>();
                    for (int i = 0; i < GridOfAssetsResult.Count; i++)
                    {
                        GridOfAssetsChecking.Add(new Asset { CiklumID = GridOfAssetsResult[i].CiklumID, SerialNumber = GridOfAssetsResult[i].SerialNumber });
                    }
                    for (int i = 0; i < GridOfAssets.Count; i++)
                    {
                        for (int b = 0; b < GridOfAssetsChecking.Count; b++)
                        {
                            if (GridOfAssets[i].AssetRow == GridOfAssetsChecking[b].AssetRow)
                            {
                                GridOfAssetsChecking.RemoveAt(b);
                            }
                        }
                    }
                    string result = "Next assets NO IN AMT \n";
                    for (int i = 0; i < GridOfAssetsChecking.Count; i++)
                    {
                        result += $"CiklumID: {GridOfAssetsChecking[i].CiklumID} ";
                        result += $"SerialNumber: {GridOfAssetsChecking[i].SerialNumber} \n";
                    }

                    MessageBox.Show(result);
                    GridOfAssetsChecking.Clear();
                    result = null;
                }
            }

        }
        private static void SetTimer(int interval) //Timer for autosaving
        {
            // Create a timer with a two second interval.
            Timer = new Timer(interval);
            // Hook up the Elapsed event for the timer. 
            Timer.Elapsed += AutoSave;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }
        #endregion
        #region Output value to clipboard
        public static void GetAll() //Get info for excel or google sheet
        {
            ClearNullInCollection(GridOfAssets);
            ClearNullInCollection(GridOfAssetsResult);
            string ToClipBoard = "CiklumID\t" + "Serial Number\t" + "Description\t" + "Status\n";

            for (int i = 0; i < GridOfAssets.Count; i++)
            {
                ToClipBoard += $"{GridOfAssets[i].CiklumID}\t{GridOfAssets[i].SerialNumber}\t{GridOfAssets[i].Description}\tNot Found\n";
            }
            for (int i = 0; i < GridOfAssetsResult.Count; i++)
            {
                ToClipBoard += $"{GridOfAssetsResult[i].CiklumID}\t{GridOfAssetsResult[i].SerialNumber}\t{GridOfAssetsResult[i].Description}\t{GridOfAssetsResult[i].Status}\n";
            }
            Clipboard.SetText(ToClipBoard);
        }
        public static void GetAllCiklumID() //Get all CiklumID for Navision
        {
            string ToClipBoard = null;
            ClearNullInCollection(GridOfAssets);
            ClearNullInCollection(GridOfAssetsResult);
            for (int i = 0; i < GridOfAssets.Count; i++)
            {
                ToClipBoard += $"{GridOfAssets[i].CiklumID}";
                if (i != GridOfAssets.Count - 1) ToClipBoard += "|";
            }
            if (GridOfAssetsResult.Count != 0) ToClipBoard += "|";
            for (int i = 0; i < GridOfAssetsResult.Count; i++)
            {
                ToClipBoard += $"{GridOfAssetsResult[i].CiklumID}";
                if (i != GridOfAssetsResult.Count - 1) ToClipBoard += "|";
            }
            Clipboard.SetText(ToClipBoard);
        }
        public static void GetAllSerialNumber() //Get all Serial Numbers for Navision
        {
            ClearNullInCollection(GridOfAssets);
            ClearNullInCollection(GridOfAssetsResult);
            string ToClipBoard = null;
            for (int i = 0; i < GridOfAssets.Count; i++)
            {
                ToClipBoard += $"{GridOfAssets[i].SerialNumber}";
                if (i != GridOfAssets.Count - 1) ToClipBoard += "|";
            }
            if (GridOfAssetsResult.Count != 0) ToClipBoard += "|";
            for (int i = 0; i < GridOfAssetsResult.Count; i++)
            {
                ToClipBoard += $"{GridOfAssetsResult[i].SerialNumber}";
                if (i != GridOfAssetsResult.Count - 1) ToClipBoard += "|";
            }
            Clipboard.SetText(ToClipBoard);
        }
        private static void GetColumnCiklumID(ObservableCollection<Asset> Grid)
        {
            ClearNullInCollection(Grid);
            string ToClipBoard = null;
            for (int i = 0; i < Grid.Count; i++)
            {
                ToClipBoard += $"{Grid[i].CiklumID}";
                if (i != Grid.Count - 1) ToClipBoard += "|";
            }
            Clipboard.SetText(ToClipBoard);
        }
        private static void GetColumnSerialNumber(ObservableCollection<Asset> Grid)
        {
            ClearNullInCollection(Grid);
            string ToClipBoard = null;
            for (int i = 0; i < Grid.Count; i++)
            {
                ToClipBoard += $"{Grid[i].SerialNumber}";
                if (i != Grid.Count - 1) ToClipBoard += "|";
            }
            Clipboard.SetText(ToClipBoard);
        }
        public static void GetColumn(string String)
        {
            if (String == "Serial Number From AMT")
            {
                GetColumnSerialNumber(GridOfAssets);
            }
            else if (String == "Serial Number Result")
            {
                GetColumnSerialNumber(GridOfAssetsResult);
            }
            else if (String == "Ciklum ID From AMT")
            {
                GetColumnCiklumID(GridOfAssets);
            }
            else if (String == "Ciklum ID Result")
            {
                GetColumnCiklumID(GridOfAssetsResult);
            }
        }
        private static void ClearNullInCollection(ObservableCollection<Asset> Grid)
        {
            for (int i = Grid.Count - 1; i >= 0; i--)
            {
                if (Grid[i].AssetRow == null || Grid[i].AssetRow == "")
                    Grid.RemoveAt(i);
            }
        }
        #endregion

        #region Fixed
        static public void UploadToFixing(DataGrid datagrid)
        {
            GridOfAssetsChecking = new ObservableCollection<Asset>();
            datagrid.ItemsSource = GridOfAssetsChecking;

            for (int i = 0; i < GridOfAssetsResult.Count; i++)
            {
                for (int b = 0; b < FixedStatusLibrary.Length; b++)
                {
                    if (GridOfAssetsResult[i].Status != "Ok" && GridOfAssetsResult[i].Status != FixedStatusLibrary[b])
                    {
                        GridOfAssetsChecking.Add(new Asset { CiklumID = GridOfAssetsResult[i].CiklumID, SerialNumber = GridOfAssetsResult[i].SerialNumber, Status = GridOfAssetsResult[i].Status });
                        break;
                    }
                }

            }
        }
        static public void StatusConverter(string Status)
        {
            if (Status == NoInAMT) Status = FixedNoInAMT;
            else if (Status == WrongCiklumID) Status = FixedWrongCiklumID;
            else if (Status == WrongSerialNumber) Status = FixedWrongSerialNumber;
            else if (Status == CiklumIDNotMutchSerialNumber) Status = FixedCiklumIDNotMutchSerialNumber;
            else if (Status == StickCiklumID) Status = FixedStickCiklumID;
        }

        static public void FinishFixing(DataGrid datagrid) //Method for equal 2 observables collection (GridOfAssetResult and GridOfAsset)
        {
            if (MessageBox.Show("Please copy all assets for inventory form AMT\nAnd press OK", "Finish fix", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                for (int i = 0; i < GridOfAssetsResult.Count; i++)
                {
                    for (int b = 0; b < FixedStatusLibrary.Length; b++)
                    {
                        if (GridOfAssetsResult[i].Status != "Ok" && GridOfAssetsResult[i].Status != FixedStatusLibrary[b])
                        {
                            GridOfAssetsResult.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }
                for (int i = 0; i < GridOfAssetsChecking.Count; i++)
                {
                    for (int b = 0; b < StatusLibrary.Length; b++)
                    {
                        if (GridOfAssetsChecking[i].Fixed == true && GridOfAssetsChecking[i].Status == StatusLibrary[b])
                        {
                            GridOfAssetsChecking[i].Status = FixedStatusLibrary[b];
                            break;
                        }
                    }
                }
                for (int i = 0; i < GridOfAssetsChecking.Count; i++)
                {
                    GridOfAssetsResult.Add(new Asset { CiklumID = GridOfAssetsChecking[i].CiklumID, SerialNumber = GridOfAssetsChecking[i].SerialNumber, Description = GridOfAssetsChecking[i].Description, Status = GridOfAssetsChecking[i].Status });
                }
                GridOfAssets.Clear();
                Upload(datagrid);
                FixingEquals();
            }
        }
        static public void FixingEquals()
        {
            for (int i = 0; i < GridOfAssetsResult.Count; i++)
            {
                for (int b = 0; b < GridOfAssets.Count; b++)
                {
                    if (GridOfAssetsResult[i].AssetRow == GridOfAssets[b].AssetRow)
                    {
                        GridOfAssets.RemoveAt(b);
                        break;
                    }
                }
            }
        }
        #endregion
        #region Sounds
        static private void SoundPlay(string sound)
        {
            string MusicFile = "";
            if (sound == "Accept")
            {
                MusicFile = "Speech On";
            }
            else if (sound == "Error")
            {
                MusicFile = "Speech Off";
            }
            else if (sound == "Repeat")
            {
                MusicFile = "chimes";
            }
            using var soundPlayer = new SoundPlayer(@$"c:\Windows\Media\{MusicFile}.wav");
            soundPlayer.Play();
            soundPlayer.Play();
        }
        #endregion
    }
}