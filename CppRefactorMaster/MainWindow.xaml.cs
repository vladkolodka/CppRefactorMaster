using System;
using System.IO;
using System.Windows;
using CppRefactorMaster.Core;
using Microsoft.Win32;

namespace CppRefactorMaster {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }

        private void RenameMethodButton_OnClick(object sender, RoutedEventArgs e) {
            var renameMethodWindow = new RenameMethodWindow();

            ShowWindow(renameMethodWindow);

            try {
                CodeEditorBox.Text = RefactorUtils.RenameMethod(CodeEditorBox.Text,
                    renameMethodWindow.OldMethodNameField.Text,
                    renameMethodWindow.NewMethodNameField.Text);
            }
            catch (ArgumentException) {
                MessageBox.Show("Error!");
            }
        }


        private void RemoveParameterButton_OnClick(object sender, RoutedEventArgs e) {
            var deleteParamsWindow = new DeleteParamsWindow();

            ShowWindow(deleteParamsWindow);

            try {
                CodeEditorBox.Text = RefactorUtils.DeleteParams(CodeEditorBox.Text,
                    deleteParamsWindow.MethodName.Text,
                    deleteParamsWindow.Parametr.Text);
            }
            catch (ArgumentException) {
                MessageBox.Show("Error!");
            }
        }

        private void ShowWindow(Window renameMethodWindow) {
            if (WindowState != WindowState.Maximized) {
                renameMethodWindow.Left = Left - renameMethodWindow.Width;
                renameMethodWindow.Top = Top;
            }
            else renameMethodWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            renameMethodWindow.ShowDialog();
        }

        private void LoadCodeFromFile(string path) {
            if (File.Exists(path)) CodeEditorBox.Text = File.ReadAllText(path);
        }

        private void CloseForm(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

        private void LoadFile_Button(object sender, RoutedEventArgs e) {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                LoadCodeFromFile(openFileDialog.FileName);
        }

        private void SaveFile_Button(object sender, RoutedEventArgs e) {
            var saveFileDialog = new SaveFileDialog {FileName = "code.cpp"};

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, CodeEditorBox.Text);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {
            LoadCodeFromFile("code.cpp");
            CodeEditorBox.Focus();
        }

        private void MagicNumber_OnClick(object sender, RoutedEventArgs e) {
            var mgWindow = new MagicNumber();
            ShowWindow(mgWindow);

            try {
                CodeEditorBox.Text = RefactorUtils.MagicNumber(CodeEditorBox.Text,
                    mgWindow.Number.Text, mgWindow.ConstName.Text);
            }
            catch (Exception) {
                MessageBox.Show("Error!");
                throw;
            }
        }

        private void BlockFormat_OnClick(object sender, RoutedEventArgs e) {
            try {
                CodeEditorBox.Text = RefactorUtils.BlockFormat(CodeEditorBox.Text);
            }
            catch (Exception) {
                MessageBox.Show("Error!");
                throw;
            }
        }
    }
}