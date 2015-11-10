using Krecik.HotKeys;
using Krecik.Nip;
using Krecik.Pesel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace KrecikWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotKeyManager _hotKeyManager;
        private NipGenerator _nipGenerator;
        private PeselGenerator _peselGenerator;

        private System.Windows.Forms.NotifyIcon notifyIcon;

        private bool _isClosedFromMenu;

        private List<string> _nipsHistory = new List<string>();
        private List<string> _peselsHistory = new List<string>();
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeNotifyIcon();
            InitializeHotKeys();

            this.Visibility = Visibility.Hidden;
        }

        private void InitializeHotKeys()
        {
            _hotKeyManager = new HotKeyManager();
            _nipGenerator = new NipGenerator();
            _peselGenerator = new PeselGenerator();

            _hotKeyManager.RegisterHotKey(
                KeyModifier.NotSet,
                Keys.F7,
                () =>
                {
                    string newNip = _nipGenerator.Generate();
                    SendKeys.SendWait(newNip);
                    _nipsHistory.Add(newNip);
                });

            _hotKeyManager.RegisterHotKey(
                KeyModifier.NotSet,
                Keys.F8,
                () =>
                {
                    string newPesel = _peselGenerator.Generate();
                    SendKeys.SendWait(newPesel);
                    _peselsHistory.Add(newPesel);
                });

            _hotKeyManager.RegisterHotKey(
                KeyModifier.NotSet,
                Keys.F9,
                () =>
                {
                    if (!this.IsActive)
                    {
                        this.Visibility = Visibility.Visible;
                        this.Activate();
                    }
                });
        }

        private void InitializeNotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("./Content/krecik.ico");
            notifyIcon.Click += NotifyIcon_Click;

            var closeAppMenuItem = new System.Windows.Forms.MenuItem("Zamknij", closeAppMenuItem_Click);
            var notifyIconMenu = new System.Windows.Forms.ContextMenu(new[] { closeAppMenuItem });
            notifyIcon.ContextMenu = notifyIconMenu;

            notifyIcon.Visible = true;
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            if (this.IsVisible)
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Visibility = Visibility.Visible;
                this.Activate();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_isClosedFromMenu)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Collapsed;
            }

            _isClosedFromMenu = false;
        }

        private void closeAppMenuItem_Click(object sender, System.EventArgs e)
        {
            _isClosedFromMenu = true;
            this.Close();
        }

        private void FillList(IEnumerable<string> newItems)
        {
            listBox.Items.Clear();
            foreach(var newItem in newItems)
            {
                listBox.Items.Add(newItem);
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem == nipItem)
            {
                FillList(_nipsHistory.Reverse<string>());
            }
            else if (comboBox.SelectedItem == peselItem)
            {
                FillList(_peselsHistory.Reverse<string>());
            }
        }
    }
}
