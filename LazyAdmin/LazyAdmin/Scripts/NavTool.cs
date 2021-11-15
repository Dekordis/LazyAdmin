using System;
using System.Windows;
using System.Windows.Controls;
using LazyAdmin.DataBase;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Timers;
using System.Media;
using System.IO;
using System.IO.Compression;

namespace LazyAdmin
{
    partial class App
    {
        #region Variables
        private static readonly string PATHFromAMT = $"{Environment.CurrentDirectory}\\GridOfAssetsFromAMT.json";
        private static readonly string PATHResult = $"{Environment.CurrentDirectory}\\GridOfAssetsResult.json";
        private static string InputCiklumID;
        private static string InputSerialNumber;
        private static bool UploadError;
        private static bool Mute;
        private static ObservableCollection<Asset> GridOfAssets;
        private static ObservableCollection<Asset> GridOfAssetsResult;
        private static ObservableCollection<Asset> GridOfAssetsChecking;
        private static FileIOService FileIOServiceFromAMT;
        private static FileIOService FileIOServiceResult;
        private static Timer Timer;
        #region Status Library
        private static readonly string Ok = "Ok";
        private static readonly string NoInAMT = "No in AMT";
        private static readonly string WrongCiklumID = "Wrong CiklumID";
        private static readonly string WrongSerialNumber = "Wrong S/N";
        private static readonly string CiklumIDNotMutchSerialNumber = "CiklumID does not match S/N";
        private static readonly string StickCiklumID = "Stick CiklumID";
        private static readonly string FixedNoInAMT = "Equipment was added to AMT";
        private static readonly string FixedWrongCiklumID = "CiklumID was changed to correct";
        private static readonly string FixedWrongSerialNumber = "S/N was changed to correct";
        private static readonly string FixedCiklumIDNotMutchSerialNumber = "CiklumID and S/N were changed to correct";
        private static readonly string FixedStickCiklumID = "CiklumID has been sticked";
        private static readonly string PreparedToDelivery = "Was prepared to delivery";
        private static readonly string PreparingToDelivery = "Preparing to delivery";

        #region Instruction Library
        private static readonly string InstructionNoInAMT = "1. Create IT Request\n1.1. Tittle: Please add equipment to AMT\n1.2. Add all information with asset to ticket\n1.3. Attach photo with current asset to tciket\n2. Waiting while HW Asset Manager added asset to AMT";
        private static readonly string InstructionWrongCiklumID = "1. Find this asset in AMT via SerialNumber\n2. Change CiklumID in AMT to correct from asset";
        private static readonly string InstructionWrongSerialNumber = "1. Finde this asset in AMT via CiklumID\n2. Change SerialNumber in AMT to correct from asset";
        private static readonly string InstructionCiklumIDNotMutchSerialNumber = "1. Finde this asset in AMT\n2. Change CiklumID in AMT to correct from asset\n(Reminder: Don't forget repeat this action for second asset with same issue)";
        private static readonly string InstructionStickCiklumID = "1. Stick CiklumID to asset\n2. Change Barcode in column(CiklumID) in AMT to stiked CiklumID\n3. Change Barcode in column(CiklumID) to stiked CiklumID";
        #endregion
        private static readonly string[] FixedStatusLibrary = { FixedNoInAMT, FixedWrongCiklumID, FixedWrongSerialNumber, FixedCiklumIDNotMutchSerialNumber, FixedStickCiklumID };
        private static readonly string[] StatusLibrary = { NoInAMT, WrongCiklumID, WrongSerialNumber, CiklumIDNotMutchSerialNumber, StickCiklumID };
        private static readonly string[] InstructionLibrary = { InstructionNoInAMT, InstructionWrongCiklumID, InstructionWrongSerialNumber, InstructionCiklumIDNotMutchSerialNumber, InstructionStickCiklumID };
        #endregion
        #endregion
        public static void Load(DataGrid _DataGridFromAMT, DataGrid _DataGridResult) //Things which load on start
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
            if (e.Action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Remove or NotifyCollectionChangedAction.Reset)
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
        public static void Upload(DataGrid _DataGrid) //uploading information from Navision
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
                UploadError = true;
            }
        }
        public static void AddID(string Input, string Method)
        {
            bool success = long.TryParse(Input, out long Number);
            if (success && (Number.ToString().Length == 13 || Number.ToString().Length == 6))
            {
                if (InputCiklumID != null) SoundPlay("Repeat");
                InputCiklumID = Input;
            }
            else
            {
                if (InputCiklumID != null) SoundPlay("Repeat");
                InputSerialNumber = Input;
            }
            CreateAsset(GridOfAssets, GridOfAssetsResult, Method);
        }
        private static void CreateAsset(ObservableCollection<Asset> Collection, ObservableCollection<Asset> CollectionResult, string Method)
        {
            if (InputCiklumID != null && InputSerialNumber != null)
            {
                CollectionResult.Add(new Asset { CiklumID = InputCiklumID, SerialNumber = InputSerialNumber });
                for (int i = 0; i < CollectionResult.Count; i++)
                {
                    if (CollectionResult[i].AssetRow == InputCiklumID + InputSerialNumber && CollectionResult[i].Status == null && Method == "Inventory")
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
                    else if(CollectionResult[i].AssetRow == InputCiklumID + InputSerialNumber && Method == "Sending")
                    {
                        CollectionResult[i].Status = PreparingToDelivery;
                        ClearVariable();
                        SoundPlay("Accept");
                        return;
                    }
                }
            }
        }
        public static void FinishSending() //Method for equal 2 observables collection (GridOfAssetResult and GridOfAsset)
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
        #endregion
        #region Output value to clipboard
        public static void GetAll() //Get info for excel or google sheet
        {
            ClearNullInCollection(GridOfAssets);
            ClearNullInCollection(GridOfAssetsResult);
            string ToClipBoard = "CiklumID\t" + "Serial Number\t" + "Description\t" + "Status\n";
            if (GridOfAssets.Count != 0)
            {
                for (int i = 0; i < GridOfAssets.Count; i++)
                {
                    ToClipBoard += $"{GridOfAssets[i].CiklumID}\t{GridOfAssets[i].SerialNumber}\t{GridOfAssets[i].Description}\tNot Found\n";
                }
            }
            if (GridOfAssetsResult.Count != 0)
            {
                for (int i = 0; i < GridOfAssetsResult.Count; i++)
                {
                    ToClipBoard += $"{GridOfAssetsResult[i].CiklumID}\t{GridOfAssetsResult[i].SerialNumber}\t{GridOfAssetsResult[i].Description}\t{GridOfAssetsResult[i].Status}\n";
                }
            }
            Clipboard.SetText(ToClipBoard);
        }
        public static void GetAllCiklumID() //Get all CiklumID for Navision
        {
            string ToClipBoard = null;
            ClearNullInCollection(GridOfAssets);
            ClearNullInCollection(GridOfAssetsResult);
            if (GridOfAssets.Count != 0)
            {
                for (int i = 0; i < GridOfAssets.Count; i++)
                {
                    ToClipBoard += $"{GridOfAssets[i].CiklumID}";
                    if (i != GridOfAssets.Count - 1) ToClipBoard += "|";
                }
            }
            if (GridOfAssetsResult.Count != 0) 
            {
                if(GridOfAssets.Count != 0)ToClipBoard += "|";
                for (int i = 0; i < GridOfAssetsResult.Count; i++)
                {
                    ToClipBoard += $"{GridOfAssetsResult[i].CiklumID}";
                    if (i != GridOfAssetsResult.Count - 1) ToClipBoard += "|";
                }
            }
            Clipboard.SetText(ToClipBoard);
        }
        public static void GetAllSerialNumber() //Get all Serial Numbers for Navision
        {
            ClearNullInCollection(GridOfAssets);
            ClearNullInCollection(GridOfAssetsResult);
            string ToClipBoard = null;
            if (GridOfAssets.Count != 0)
            {
                for (int i = 0; i < GridOfAssets.Count; i++)
                {
                    ToClipBoard += $"{GridOfAssets[i].SerialNumber}";
                    if (i != GridOfAssets.Count - 1) ToClipBoard += "|";
                }
            }
            if (GridOfAssetsResult.Count != 0)
            {
                if (GridOfAssets.Count != 0) ToClipBoard += "|";
                for (int i = 0; i < GridOfAssetsResult.Count; i++)
                {
                    ToClipBoard += $"{GridOfAssetsResult[i].SerialNumber}";
                    if (i != GridOfAssetsResult.Count - 1) ToClipBoard += "|";
                }
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
            else return;
        }
        private static void ClearNullInCollection(ObservableCollection<Asset> Grid)
        {
            for (int i = Grid.Count - 1; i >= 0; i--)
            {
                if (Grid[i].AssetRow is null or "")
                    Grid.RemoveAt(i);
            }
        }
        #endregion
        #region Fixed
        public static void UploadToFixing(DataGrid datagrid)
        {
            GridOfAssetsChecking = new ObservableCollection<Asset>();
            datagrid.ItemsSource = GridOfAssetsChecking;

            for (int i = 0; i < GridOfAssetsResult.Count; i++)
            {
                for (int b = 0; b < FixedStatusLibrary.Length; b++)
                {
                    bool Error = false;
                    for (int e = 0; e < FixedStatusLibrary.Length; e++)
                    {
                        if (GridOfAssetsResult[i].Status != Ok && GridOfAssetsResult[i].Status == FixedStatusLibrary[e])
                        {
                            Error = true;
                        }
                    }
                    if (Error) break;
                    else if (GridOfAssetsResult[i].Status != Ok && GridOfAssetsResult[i].Status != FixedStatusLibrary[b])
                    {
                        string Help = "Error";
                        for (int h = 0; h < StatusLibrary.Length; h++)
                        {
                            if (GridOfAssetsResult[i].Status == StatusLibrary[h])
                            {
                                Help = InstructionLibrary[h];
                            }
                        }
                        GridOfAssetsChecking.Add(new Asset { CiklumID = GridOfAssetsResult[i].CiklumID, SerialNumber = GridOfAssetsResult[i].SerialNumber, Status = GridOfAssetsResult[i].Status, Description = GridOfAssetsResult[i].Description, Instruction = Help });
                        break;
                    }
                }
            }
        }
        public static void StatusConverter(string Status)
        {
            if (Status == NoInAMT) Status = FixedNoInAMT;
            else if (Status == WrongCiklumID) Status = FixedWrongCiklumID;
            else if (Status == WrongSerialNumber) Status = FixedWrongSerialNumber;
            else if (Status == CiklumIDNotMutchSerialNumber) Status = FixedCiklumIDNotMutchSerialNumber;
            else if (Status == StickCiklumID) Status = FixedStickCiklumID;
        }

        public static void FinishFixing(DataGrid datagrid, DataGrid _DataGridFromAMT, DataGrid _DataGridResult) //Method for equal 2 observables collection (GridOfAssetResult and GridOfAsset)
        {
            UploadError = false;
            BackUp();
            if (GridOfAssetsChecking.Count <= 0)
            {
                return;
            }
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
                        if (GridOfAssetsChecking[i].Fixed && GridOfAssetsChecking[i].Status == StatusLibrary[b])
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
                ErrorWithUpload(datagrid);
                FixingEquals();
            }
        }
        public static void FixingEquals()
        {
            for (int i = 0; i < GridOfAssetsResult.Count; i++)
            {
                for (int b = 0; b < GridOfAssets.Count; b++)
                {
                    if (GridOfAssetsResult[i].AssetRow == GridOfAssets[b].AssetRow)
                    {
                        if(GridOfAssetsResult[i].Status != NoInAMT)
                        {
                            GridOfAssets.RemoveAt(b);
                            break;
                        }
                        else
                        {
                            GridOfAssetsResult[i].Description = GridOfAssets[b].Description;
                            GridOfAssets.RemoveAt(b);
                            break;
                        }

                    }
                }
            }
        }
        #endregion
        #region Sounds
        private static void SoundPlay(string sound)
        {
            if (Mute == false)
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
        }
        public static void MuteSound()
        {
            if (Mute == false) Mute = true;
            else Mute = false;
        }
        #endregion
        #region Some additional methods
        private static void ClearVariable()
        {
            InputCiklumID = null;
            InputSerialNumber = null;
        } //Method for clearing temporary variables
        public static void ClearAssets() //Clearing GrindOfAsset
        {
            GridOfAssets.Clear();
            GridOfAssetsResult.Clear();
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
        public static void BackUp()
        {
            Directory.CreateDirectory($@"{Environment.CurrentDirectory}\BackUpDataBase");
            File.Copy($@"{Environment.CurrentDirectory}\GridOfAssetsFromAMT.json", $@"{Environment.CurrentDirectory}\BackUpDataBase\GridOfAssetsFromAMT.json", true);
            File.Copy($@"{Environment.CurrentDirectory}\GridOfAssetsResult.json", $@"{Environment.CurrentDirectory}\BackUpDataBase\GridOfAssetsResult.json", true);
        }
        public static void BackUp(DataGrid _DataGridFromAMT, DataGrid _DataGridResult)
        {

            File.Delete($@"{Environment.CurrentDirectory}\GridOfAssetsFromAMT.json");
            File.Delete($@"{Environment.CurrentDirectory}\GridOfAssetsResult.json");
            File.Move($@"{Environment.CurrentDirectory}\BackUpDataBase\GridOfAssetsFromAMT.json", $@"{Environment.CurrentDirectory}\GridOfAssetsFromAMT.json");
            File.Move($@"{Environment.CurrentDirectory}\BackUpDataBase\GridOfAssetsResult.json", $@"{Environment.CurrentDirectory}\GridOfAssetsResult.json");
            Directory.Delete($@"{Environment.CurrentDirectory}\BackUpDataBase");
            _DataGridFromAMT.ItemsSource = GridOfAssets;
            _DataGridResult.ItemsSource = GridOfAssetsResult;
        }
        private static void ErrorWithUpload(DataGrid datagrid)
        {
            if (UploadError || GridOfAssets.Count == 0)
            {
                UploadError = false;
                if (MessageBox.Show("Plese copy correct data from AMT and press OK \nIf you press Cancel your information from AMT will be lost", "Error", MessageBoxButton.OKCancel) == MessageBoxResult.OK) Upload(datagrid);
                else return;
                if (UploadError || GridOfAssets.Count == 0)
                {
                    MessageBox.Show("Failed to upload, try repeat");
                    ErrorWithUpload(datagrid);
                }
            }
        }
        #endregion
    }
}