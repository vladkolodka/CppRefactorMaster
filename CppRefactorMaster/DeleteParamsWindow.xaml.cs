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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CppRefactorMaster.Core;

namespace CppRefactorMaster
{
    /// <summary>
    /// Логика взаимодействия для DeleteParamsWindow.xaml
    /// </summary>
    public partial class DeleteParamsWindow : Window
    {
        private readonly string _code;
        private readonly MainWindow _window;

        public DeleteParamsWindow(MainWindow window, string code)
        {
            _window = window;
            _code = code;

            InitializeComponent();
            MethodName.Focus();
        }

        private void Field_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var element = (TextBox)sender;
            element.Select(0, element.Text.Length);
        }

        private void ActionButton_OnClick(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(_code);

            _window.CodeEditorBox.Text = RefactorUtils.DeleteParams(_code, MethodName.Text, Parametr.Text);

            _window.Focus();
            Close();
        }

        private void Comments_Checked(object sender, RoutedEventArgs e)
        {
            RefactorUtils.OnComments = true;
            Commetns.Foreground = new SolidColorBrush(Colors.MediumSeaGreen);
        }

        private void Comments_UnChecked(object sender, RoutedEventArgs e)
        {
            RefactorUtils.OnComments = false;
            Commetns.Foreground = new SolidColorBrush(Colors.Red);
        }
    }
}
