using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello1");
            MessageBox.Show("Hello2");
        }

        private void button1a_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread( () => MessageBox.Show("Hello1") );
            t1.IsBackground = true;
            Thread t2 = new Thread( () => MessageBox.Show("Hello2") );
            t1.Start();
            t2.Start();
        }

        private void button1b_Click(object sender, EventArgs e)
        {
            Task t1 = new Task( () => MessageBox.Show("Hello1") );
            t1.Start();

            Task.Run( () => MessageBox.Show("Hello2") );
        }

    }
}
