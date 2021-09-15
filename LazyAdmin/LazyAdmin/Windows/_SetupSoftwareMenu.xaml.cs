using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LazyAdmin.Windows
{
    /// <summary>
    /// Interaction logic for _SetupSoftwareMenu.xaml
    /// </summary>
    public partial class _SetupSoftwareMenu : Window
    {
        public _SetupSoftwareMenu()
        {
            InitializeComponent();
            App.WindowSettings(_HeaderButtonGrid, this);
        }
    }
}
