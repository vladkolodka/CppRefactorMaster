using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CppRefactorMaster.Core;

namespace CppRefactorMaster {
    /// <summary>
    ///     Interaction logic for RenameMethodWindow.xaml
    /// </summary>
    public partial class RenameMethodWindow : Window {
        private readonly string _code;
        private readonly MainWindow _window;

        public RenameMethodWindow(MainWindow window, string code) {
            _window = window;
            _code = code;

            InitializeComponent();
            OldMethodNameField.Focus();
        }

        private void Field_OnGotFocus(object sender, RoutedEventArgs e) {
            var element = (TextBox) sender;
            element.Select(0, element.Text.Length);
        }

        private void ActionButton_OnClick(object sender, RoutedEventArgs e) {

            _window.CodeEditorBox.Text = RefactorUtils.RenameMethod(_code, OldMethodNameField.Text, NewMethodNameField.Text);

            Close();
            _window.Focus();
        }

        private void RenameMethodWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Escape) {
                e.Handled = true;
                Close();
                _window.Focus();
            }
        }
    }
}