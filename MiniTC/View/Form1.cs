using System;
using System.IO;
using System.Windows.Forms;
using MiniTC.Presenter;
using MiniTC.View;

namespace MiniTC
{
    public partial class Form1 : Form
    {
        private readonly PanelTC leftPanel;
        private readonly PanelTC rightPanel;

        public Form1()
        {
            InitializeComponent();

            var leftWrapper = new PanelWrapper(this, true);
            var rightWrapper = new PanelWrapper(this, false);

            leftPanel = new PanelTC(leftWrapper);
            rightPanel = new PanelTC(rightWrapper);


            leftPanel.LoadDrives();
            rightPanel.LoadDrives();


        }

        
 

        private void ButtonCopy_Click(object sender, EventArgs e)
        {


            try
            {
                if (listBoxLeft.SelectedItem != null)
                {
                    CopyItem(leftPanel, rightPanel, listBoxLeft, textBoxLeft, textBoxRight);
                }
                else if (listBoxRight.SelectedItem != null)
                {
                    CopyItem(rightPanel, leftPanel, listBoxRight, textBoxRight, textBoxLeft);
                }
                else
                {
                    MessageBox.Show("Proszę wybrać plik i zaznaczyć panel przed kopiowaniem.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd kopiowania: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyItem(PanelTC sourcePanel, PanelTC destPanel, ListBox sourceList, TextBox sourcePath, TextBox destPath)
        {
            var selected = sourceList.SelectedItem.ToString();

            if (selected.StartsWith("<D>"))
            {
                MessageBox.Show("Kopiowanie folderów nie jest obsługiwane.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selected.StartsWith("<D> "))
                selected = selected.Substring(4);

            selected = Path.GetFileName(selected); // <- ważne!

            string srcPath = sourcePath.Text.TrimEnd('\\');
            string dstPath = destPath.Text.TrimEnd('\\');

            var sourceFile = Path.Combine(sourcePath.Text, selected);

            string destFile = Path.Combine(dstPath, selected);

            MessageBox.Show($"Kopiowanie z:\n{sourceFile}\ndo:\n{destFile}");

            File.Copy(sourceFile, destFile, true);
            destPanel.LoadDirectoryContent(destPath.Text);
            MessageBox.Show("Plik skopiowany pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        #region Properties for PanelWrapper
        public ComboBox comboBoxLeft => comboBox1;
        public ComboBox comboBoxRight => comboBox2;
        public TextBox textBoxLeft => textBox1;
        public TextBox textBoxRight => textBox2;
        public ListBox listBoxLeft => listBox1;
        public ListBox listBoxRight => listBox2;
        public Button buttonCopy => button1;
        #endregion
    }
}
