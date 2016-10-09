using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CppRefactorMaster {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Window _renameMethodWindow;
        private Window _deleteParamsWindow;

        public MainWindow() {
            InitializeComponent();
            CenterWindow();
            //            LoadCodeFromFile("code.cpp");
            //            MessageBox.Show(this, AppDomain.CurrentDomain.BaseDirectory);
            Loaded += (sender, args) => {
                LoadCodeFromFile("code.cpp");
                CodeEditorBox.Focus();
            };
        }

        private void CenterWindow() {
            Left = SystemParameters.VirtualScreenWidth / 2 - Width / 2;
            Top = SystemParameters.VirtualScreenHeight / 2 - Height / 2;
        }

        private void RenameMethodButton_OnClick(object sender, RoutedEventArgs e) {
            IsEnabled = false;

            _renameMethodWindow = new RenameMethodWindow(this, CodeEditorBox.Text) {
                Owner = this
            };

            _renameMethodWindow.Left = Left - _renameMethodWindow.Width;
            _renameMethodWindow.Top = Top;

            _renameMethodWindow.Show();
            _renameMethodWindow.Closed += (o, args) => IsEnabled = true;
        }

        private void RemoveParameterButton_OnClick(object sender, RoutedEventArgs e) {
            IsEnabled = false;

            _deleteParamsWindow = new DeleteParamsWindow(this, CodeEditorBox.Text)
            {
                Owner = this
            };

            _deleteParamsWindow.Left = Left - _deleteParamsWindow.Width;
            _deleteParamsWindow.Top = Top;

            _deleteParamsWindow.Show();
            _deleteParamsWindow.Closed += (o, args) => IsEnabled = true;

        }

        private void LoadCodeFromFile(string path) {
            if(File.Exists(path)) CodeEditorBox.Text = File.ReadAllText(path);
        }
    }
}