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

namespace InfertilityTreatment.HomePage
{
    /// <summary>
    /// Interaction logic for ContactUsPage.xaml
    /// </summary>
    public partial class ContactUsPage : Window
    {
        public ContactUsPage()
        {
            InitializeComponent();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
           this.Close();

        }
    }
}
