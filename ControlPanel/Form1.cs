using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlPanel
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();

       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             client.GetAsync("http://localhost:46194/api/Index/SaveOptionInstrument");

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           client.GetAsync("http://localhost:46194/api/Option/SaveOptionData");

        }

        private void button8_Click(object sender, EventArgs e)
        {
            client.GetAsync("http://localhost:46194/api/Index/SaveFutureInstrument");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            client.GetAsync("http://localhost:46194/api/Index/SaveFutureData");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            client.GetAsync("http://localhost:46194/api/Market/StopStock");

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            client.GetAsync("http://localhost:46194/api/Market/GetMarketData");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            client.GetAsync("http://localhost:46194/api/Option/StopOption");

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            client.GetAsync("http://localhost:46194/api/Index/StopFuture");

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            client.GetAsync("http://localhost:46194/api/Index/DeletePreviousData");

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
