using System.ComponentModel;

namespace LazyAdmin.DataBase
{
    class Asset : INotifyPropertyChanged
    {
        private string _CiklumID;
        private string _SerialNumber;
        private string _Description;
        private string _Status;

        public string CiklumID
        {
            get { return _CiklumID; }
            set
            {
                if (_CiklumID == value) return;

                _CiklumID = value;
                OnPropertyChanged("CiklumID");
            }
        }
        public string SerialNumber
        {
            get { return _SerialNumber; }
            set
            {
                if (_SerialNumber == value) return;
                _SerialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description == value) return;
                _Description = value;
                OnPropertyChanged("Description");
            }
        }
        public string Status
        {
            get { return _Status; }
            set
            {
                if (_Status == value) return;
                _Status = value;
                OnPropertyChanged("Status");
            }
        }
        public string AssetRow
        {
            get { return _CiklumID + _SerialNumber; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
