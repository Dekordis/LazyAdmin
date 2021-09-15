using System;
using System.Windows;
using System.Diagnostics;

namespace LazyAdmin.Windows
{
    /// <summary>
    /// Interaction logic for _LinksMenu.xaml
    /// </summary>
    public partial class _LinksMenu : Window
    {
        public _LinksMenu()
        {
            InitializeComponent();
            App.WindowSettings(_HeaderButtonGrid, this);
        }
        private void Startlink(string link) //Method for run links in default browser
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"{link}",
                UseShellExecute = true
            }
            );
        }
    }
    public partial class _LinksMenu : Window //Buttons
    {
        private void RDPnav(object sender, EventArgs e)
        {
            var ip = "srv-nav-term.kyiv.ciklum.net";
            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "mstsc",
                Arguments = $"/v:{ip}"
            };
            process.Start();
        }

        private void RDPAD(object sender, EventArgs e)
        {
            var ip = "srv-term-az.kyiv.ciklum.net";
            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "mstsc",
                Arguments = $"/v:{ip}"
            };
            process.Start();
        } 

        private void MyCiklum(object sender, EventArgs e)
        {
            Startlink("https://my.ciklum.com");
        }

        private void ERM(object sender, EventArgs e)
        {
            Startlink("https://erm.ciklum.net");
        }

        private void ITWiki(object sender, EventArgs e)
        {
            Startlink("https://itwiki.ciklum.net/start");
        }

        private void Jira(object sender, EventArgs e)
        {
            Startlink("https://jira.ciklum.net");
        }

        private void JiraOld(object sender, EventArgs e)
        {
            Startlink("https://servicedesk.ciklum.net");
        }

        private void Biostar(object sender, EventArgs e)
        {
            Startlink("https://biostar2.ciklum.net");
        }

        private void ESET(object sender, EventArgs e)
        {
            Startlink("https://enc.ciklum.net");
        }

        private void Commvault(object sender, EventArgs e)
        {
            Startlink("https://commvault.ciklum.net");
        }

        private void TDStorage(object sender, EventArgs e)
        {
            Startlink(@"\\TDStorage");
        }

        private void ERMNENs(object sender, EventArgs e)
        {
            Startlink("https://erm.ciklum.net/employees/acceptedUsers/index?");
        }

        private void License(object sender, EventArgs e)
        {
            Startlink("https://docs.google.com/spreadsheets/d/1kA7baaBehrXSzDW34PTgYQ-WVSLYiYuVvvYEmFZ-NUk/edit#gid=0");
        }

        private void RepairSC(object sender, EventArgs e)
        {
            Startlink("https://docs.google.com/spreadsheets/d/1I0BEPBBtq0P5m5xscLltagwggNBzqx5mX3eU4Wh-a6g/edit#gid=520213509");
        }

        private void LocAdmin(object sender, EventArgs e)
        {
            Startlink("https://docs.google.com/spreadsheets/d/14oMEO5vNHe1igiTcdgicaJtK0E8yUl3rmfofULQ8fxY/edit#gid=0");
        }
    }
}
