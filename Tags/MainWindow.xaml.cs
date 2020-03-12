using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Tags
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private HwndSource _source;
        private const int HOTKEY_ID = 9000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey();
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
            base.OnClosed(e);
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            if (!RegisterHotKey(helper.Handle, HOTKEY_ID, (uint)settings.hotkey_mod, (uint)settings.hotkey_key))
            {
                MessageBox.Show("Hotkey registration failed! It has probably already been taken by some other app. Please assign other keystroke in 'settings.ini'.", "Error registering hotkey", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }

        SettingsLoader settings = new SettingsLoader();

        public MainWindow()
        {
            InitializeComponent();

            Width = settings.width;
            Height = settings.height;

            foreach (var tag in settings.Tags) {
                Button btn = new Button();
                btn.Content = tag.Text;
                btn.Height = 48;
                btn.FontSize = 16;
                btn.BorderBrush = Brushes.Transparent;
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(tag.Color));
                btn.Style = FindResource("MyButton") as Style;
                btn.Tag = tag;
                btn.Click += Btn_Click;

                stackPanel.Children.Add(btn);
            }

            if (settings.Tags.Count == 0) {
                MessageBox.Show("There are no tags found. Please add your tags as [tag:my_tag] in 'settings.ini'");
                Application.Current.Shutdown();
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e) {
            var paths = WExplorerAPI.GetExplorerFiles();

            Button btn = (Button)sender;
            TagSettings tag = (TagSettings)btn.Tag;

            int total = 0;
            foreach(var path in paths) {
                total += path.Paths.Count;
            }

            MessageBox.Show(string.Format("Tag: '{0}' is going to be added to {1} files in {2} instances of Windows Explorer", tag.Tag, total, paths.Count));

            if (settings.hideOnClick)
                WindowState = WindowState.Minimized;
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnHotKeyPressed() {
            if (WindowState == WindowState.Minimized) {
                WindowState = WindowState.Normal;
                Activate();
                return;
            }
            if (settings.toogleVisibility) {
                WindowState = WindowState.Minimized;
            }
        }
    }
}
