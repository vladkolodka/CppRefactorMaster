using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CppRefactorMaster.Core;

namespace CppRefactorMaster {
    /// <summary>
    ///     Логика взаимодействия для DeleteParamsWindow.xaml
    /// </summary>
    public partial class DeleteParamsWindow {
        public DeleteParamsWindow() {
            InitializeComponent();
            MethodName.Focus();
        }

        private void Field_OnGotFocus(object sender, RoutedEventArgs e) {
            var element = (TextBox) sender;
            element.Select(0, element.Text.Length);
        }

        private void ActionButton_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Comments_Checked(object sender, RoutedEventArgs e) {
            RefactorUtils.OnComments = true;
            Commetns.Foreground = new SolidColorBrush(Colors.MediumSeaGreen);
        }

        private void Comments_UnChecked(object sender, RoutedEventArgs e) {
            RefactorUtils.OnComments = false;
            Commetns.Foreground = new SolidColorBrush(Colors.Red);
        }
    }
}