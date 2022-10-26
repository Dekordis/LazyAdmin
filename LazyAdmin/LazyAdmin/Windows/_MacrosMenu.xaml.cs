using System.Windows;
using System.Windows.Input;

namespace LazyAdmin.Windows
{
    /// <summary>
    /// Interaction logic for _MacrosMenu.xaml
    /// </summary>
    public partial class _MacrosMenu : Window
    {
        public _MacrosMenu()
        {
            InitializeComponent();
            App.WindowSettings(_HeaderButtonGrid, this);
        }
        private void DoIt(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("lala");
        }
        private void EnterText(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MessageBox.Show("lala1");
            }
        }
        private void ShowMessage_Executed(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Test");
        }
        //public static class UserCommands
        //{
        //    static UserCommands()
        //    {
        //        // Можно прописать горячие клавиши по умолчанию
        //        InputGestureCollection inputs = new InputGestureCollection();
        //        inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+S"));

        //        SomeCommand = new RoutedUICommand("Some", "SomeCommand", typeof(UserCommands), inputs);
        //    }

        //    public static RoutedCommand SomeCommand { get; private set; }
        //}
    }
}

