using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CppRefactorMaster {
    /// <summary>
    ///     Interaction logic for RenameMethodWindow.xaml
    /// </summary>
    public partial class RenameMethodWindow {
        public RenameMethodWindow() {
            InitializeComponent();
            OldMethodNameField.Focus();
        }

        private void Field_OnGotFocus(object sender, RoutedEventArgs e) {
            var element = (TextBox) sender;
            element.Select(0, element.Text.Length);
        }

        private void ActionButton_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void RenameMethodWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape) {
                e.Handled = true;
                Close();
            }
        }
    }
}