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

namespace InfertilityTreatment.Manager
{
    /// <summary>
    /// Interaction logic for NoteInputDialog.xaml
    /// </summary>
    public partial class NoteInputDialog : Window
    {
        public string NoteText { get; private set; }

        public NoteInputDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NoteText = NoteTextBox.Text.Trim();
            if (string.IsNullOrEmpty(NoteText))
            {
                MessageBox.Show("Note cannot be empty.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
