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

namespace dotBAT_UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Browse Files",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Editor.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void buildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Save File",
                CheckPathExists = true,
                DefaultExt = "exe",
                Filter = "Application (*.exe)|*.exe",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (sfd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            Packer.Pack(Editor.Text, sfd.FileName, iconToolStripMenuItem.Checked);
        }

        private void hiddenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("lol no");
        }
    }
}
