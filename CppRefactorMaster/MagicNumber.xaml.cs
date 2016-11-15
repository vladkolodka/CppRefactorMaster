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

namespace CppRefactorMaster
{
    /// <summary>
    /// Interaction logic for MagicNumber.xaml
    /// </summary>
    public partial class MagicNumber : Window
    {
        public MagicNumber()
        {
            InitializeComponent();
        }

        private void ActionButton_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Field_OnGotFocus(object sender, RoutedEventArgs e) {
            var element = (TextBox) sender;
            element.Select(0, element.Text.Length);
        }
    }
}
