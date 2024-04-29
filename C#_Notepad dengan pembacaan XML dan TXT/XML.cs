using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TEXTEDITOR
{
    public partial class XML : Form
    {
        private string _currentFile; // Untuk melacak file yang sedang dibuka

        public XML()
        {
            InitializeComponent();
            richTextBox1.Text = ""; // Memastikan TextBox kosong
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges())
            {
                return; // Keluar jika pengguna membatalkan perubahan yang dibuang
            }

            richTextBox1.Clear();
            _currentFile = null; // Menandakan tidak ada file yang dibuka
        }

        

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges())
            {
                return; // Keluar jika pengguna membatalkan perubahan yang dibuang
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "File Teks (*.txt)|*.txt|File XML (*.xml)|*.xml"; // Menambahkan filter untuk .xml
            openFileDialog.Title = "Buka File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    _currentFile = openFileDialog.FileName; // Perbarui file yang sedang dibuka

                    if (Path.GetExtension(_currentFile).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        // Memuat konten dari file XML dan menerapkannya ke richTextBox1
                        XMLFileManager xmlFileManager = new XMLFileManager();
                        xmlFileManager.filepath = _currentFile;
                        xmlFileManager.ReadXMLFile(richTextBox1);
                    }
                    else
                    {
                        richTextBox1.Text = fileContent; // Memuat konten dari file teks ke richTextBox1
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentFile == null)
            {
                saveAsToolStripMenuItem_Click(sender, e); // Gunakan saveAs jika tidak ada file yang dibuka
                return;
            }

            if (Path.GetExtension(_currentFile).Equals(".xml", StringComparison.OrdinalIgnoreCase))
            {
                // Menyimpan dalam format XML
                SaveXMLFile();
            }
            else
            {
                // Menyimpan dalam format teks biasa
                SaveTextFile();
            }
        }

        private void SaveXMLFile()
        {
            XMLFileManager xmlFileManager = new XMLFileManager();
            xmlFileManager.filepath = _currentFile;

            string fontfamily = richTextBox1.Font.FontFamily.Name;
            int fontsize = (int)richTextBox1.Font.Size;
            string fonttype = richTextBox1.Font.Style.ToString();
            Color fontcolor = richTextBox1.ForeColor;
            Color bgcolor = richTextBox1.BackColor;
            string content = richTextBox1.Text;

            xmlFileManager.WriteXMLFile(fontfamily, fontsize, fonttype, fontcolor, bgcolor, content);
        }

        private void SaveTextFile()
        {
            try
            {
                File.WriteAllText(_currentFile, richTextBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "File Teks (*.txt)|*.txt|File XML (*.xml)|*.xml"; // Menambahkan filter untuk .xml
            saveFileDialog.Title = "Simpan File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _currentFile = saveFileDialog.FileName;

                if (Path.GetExtension(_currentFile).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    // Menyimpan dalam format XML
                    SaveXMLFile();
                }
                else
                {
                    // Menyimpan dalam format teks biasa
                    SaveTextFile();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscardChanges())
            {
                return; // Keluar jika pengguna membatalkan perubahan yang dibuang
            }

            Application.Exit();
        }

        private bool ConfirmDiscardChanges()
        {
            if (richTextBox1.Text.Length > 0)
            {
                DialogResult result = MessageBox.Show("Apakah Anda ingin menyimpan perubahan?", "Konfirmasi", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                switch (result)
                {
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(null, null); // Simpan dan lanjutkan
                        return true;
                    case DialogResult.No:
                        return true; // Buang dan lanjutkan
                    case DialogResult.Cancel:
                        return false; // Batalkan operasi
                    default:
                        return false; // Tangani hasil yang tidak terduga
                }
            }
            else
            {
                return true; // Tidak ada perubahan yang dibuang
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Munculkan Font Dialog
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog.Font;
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Munculkan Color Dialog
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = colorDialog.Color;
            }
        }

        private void colorBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Munculkan Color Dialog
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = colorDialog.Color;
            }
        }
    }
}

