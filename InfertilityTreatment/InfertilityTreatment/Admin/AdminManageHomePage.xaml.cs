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

namespace InfertilityTreatment.Admin
{
    /// <summary>
    /// Interaction logic for AdminManageHomePage.xaml
    /// </summary>
    public partial class AdminManageHomePage : Window
    {
        public AdminManageHomePage()
        {
            InitializeComponent();
        }

        private void btAcccountManager_Click(object sender, RoutedEventArgs e)
        {
            try {
                UserManager userManager = new UserManager();
                userManager.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }


        }

        private void btDashboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserAccessHistoryDashPort userAccessHistoryDashPort = new UserAccessHistoryDashPort();
                userAccessHistoryDashPort.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        private void Button_Click_logout(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
