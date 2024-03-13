using Febraban;
using FoneClube.DataAccess;
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

namespace FoneClube.WinForm
{
    public partial class Parser : Form
    {
        public Parser()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);

            // Start the worker.
            bg.RunWorkerAsync();

            // Display the loading form.
            //loadingForm = new loadingForm();
            //loadingForm.ShowDialog();
        }

        private void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            selectFileButton.Click += new System.EventHandler(OnClickFileButton);
            // Perform your long running operation here.
            // If you need to pass results on to the next
            // stage you can do so by assigning a value
            // to e.Result.
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Retrieve the result pass from bg_DoWork() if any.
            // Note, you may need to cast it to the desired data type.
            object result = e.Result;

            // Close the loading form.
            ///////////////////////////loadingForm.Close();

            // Update any other UI controls that may need to be updated.
        }

        private void OnClickFileButton(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                var fileV2 = openFileDialog1.FileName;
                try
                {
                    var conta = new FebrabanParser().Parse(fileV2);
                    var salvaDB = new ContaAcesso().SaveConta(conta);

                    if(salvaDB)
                        MessageBox.Show("OH Yeah, conta foi salva com sucesso", "My Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Por algum motivo a conta não foi totalmente salva", "My Application", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (IOException)
                {
                }
            }
            
        }
    }
}
