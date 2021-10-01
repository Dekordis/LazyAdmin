using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LazyAdmin.DataBase
{
    //class GridDataBase : INotifyPropertyChanged
    //{
    //    private static int ciklumid;
    //    private static string serialnumber;
    //    private static string description;
    //    public static int CiklumIDDataBase
    //    {
    //        get { return ciklumid; }
    //        set { ciklumid = value; }
    //    }
    //    public static string SerialNumberDataBase
    //    {
    //        get { return serialnumber; }
    //        set { serialnumber = value; }
    //    }
    //    public static string DescriptionDataBase
    //    {
    //        get { return description; }
    //        set { description = value; }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //}

    class GridDataBase : INotifyPropertyChanged
    {
        private string _CiklumIDFromDataBase;
        private string _SerialNumberFromDataBase;
        private string _DescriptionFromDataBase;

        public string CiklumIDFromDataBase
        {
            get { return _CiklumIDFromDataBase; }
            set 
            {
                if (_CiklumIDFromDataBase == value) return;

                _CiklumIDFromDataBase = value;
                OnPropertyChanged("CiklumID");
            }
        }
        public string SerialNumberFromDataBase
        {
            get { return _SerialNumberFromDataBase; }
            set 
            {
                if (_SerialNumberFromDataBase == value) return;
                _SerialNumberFromDataBase = value;
                OnPropertyChanged("SerialNumber");
            }
        }
        public string DescriptionFromDataBase
        {
            get { return _DescriptionFromDataBase; }
            set 
            {
                if (_DescriptionFromDataBase == value) return;
                _DescriptionFromDataBase = value;
                OnPropertyChanged("Description");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
