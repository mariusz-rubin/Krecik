using Generator.HotKeys;
using Generator.Nip;
using Generator.Pesel;
using System.Windows.Forms;

namespace Generator
{
    public partial class GeneratorForm : Form
    {
        private readonly HotKeyManager _hotKeyManager;
        private readonly NipGenerator _nipGenerator;
        private readonly PeselGenerator _peselGenerator;

        private ContextMenu notifyIconMenu;
        private MenuItem closeAppMenuItem;

        public GeneratorForm()
        {
            InitializeComponent();
            InitControls();

            _hotKeyManager = new HotKeyManager();
            _nipGenerator = new NipGenerator();
            _peselGenerator = new PeselGenerator();
        }

        private void InitControls()
        {
            closeAppMenuItem = new MenuItem("Zamknij", closeAppMenuItem_Click);
            notifyIconMenu = new ContextMenu(new[] { closeAppMenuItem });
            notifyIcon.ContextMenu = notifyIconMenu;

            FillKeyItems(cmbNip);
            FillKeyItems(cmbPesel);

            cmbNip.SelectedItem = Keys.F7;
            cmbPesel.SelectedItem = Keys.F8;
            btnStart.Focus();
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            btnStart.Enabled = false;
            cmbNip.Enabled = false;
            cmbPesel.Enabled = false;

            _hotKeyManager.RegisterHotKey(
                KeyModifier.NotSet,
                Keys.F7,
                () => SendKeys.SendWait(_nipGenerator.Generate()));

            _hotKeyManager.RegisterHotKey(
                KeyModifier.NotSet,
                Keys.F8,
                () => SendKeys.SendWait(_peselGenerator.Generate()));

            notifyIcon.Visible = true;
            this.Hide();
        }

        private void FillKeyItems(ComboBox comboBox)
        {
            int startKey = (int)Keys.F1;
            int endKey = (int)Keys.F12;

            for (int i = startKey; i <= endKey; i++)
            {
                Keys key = (Keys)i;
                comboBox.Items.Add(key);
            }
        }

        private void closeAppMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
