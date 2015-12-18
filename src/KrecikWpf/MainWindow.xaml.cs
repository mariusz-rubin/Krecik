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
using System.Collections.ObjectModel;

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

        private ObservableCollection<string> _numbersCollection = new ObservableCollection<string>();        
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeNotifyIcon();
            InitializeBinding();
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
                    AddNipNumberToHistory(newNip);
                });

            _hotKeyManager.RegisterHotKey(
                KeyModifier.NotSet,
                Keys.F8,
                () =>
                {
                    string newPesel = _peselGenerator.Generate();
                    SendKeys.SendWait(newPesel);
                    AddPeselNumberToHistory(newPesel);
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

        private void InitializeBinding()
        {
            listBox.ItemsSource = _numbersCollection;
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillSelectedList();
        }

        private void FillSelectedList()
        {
            if (comboBox.SelectedItem == nipItem)
            {
                _numbersCollection.Clear();
                foreach(string nip in _nipsHistory.Reverse<string>())
                {
                    _numbersCollection.Add(nip);
                }
            }
            else if (comboBox.SelectedItem == peselItem)
            {
                _numbersCollection.Clear();
                foreach(var pesel in _peselsHistory.Reverse<string>())
                {
                    _numbersCollection.Add(pesel);
                }
            }
        }

        private void AddNipNumberToHistory(string number)
        {
            _nipsHistory.Add(number);

            if (comboBox.SelectedItem == nipItem)
            {
                _numbersCollection.Insert(0, number);
            }          
        }

        private void AddPeselNumberToHistory(string number)
        {
            _peselsHistory.Add(number);

            if (comboBox.SelectedItem == peselItem)
            {
                _numbersCollection.Insert(0, number);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _nipsHistory.Clear();
            _peselsHistory.Clear();
            _numbersCollection.Clear();
        }
    }
}
