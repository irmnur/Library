using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class UpdateBook : Form
    {
        public UpdateBook()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UpdateBook_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DB.GetBooksToEdit().Tables[0];
            dataGridView1.Columns["ID"].Visible = false;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int RowIndex = e.RowIndex;
            int ID = Convert.ToInt32(dataGridView1.Rows[RowIndex].Cells[0].Value);
            string BookName = dataGridView1.Rows[RowIndex].Cells[1].Value.ToString();
            string BarcodeNumber = dataGridView1.Rows[RowIndex].Cells[2].Value.ToString();
            string Author = dataGridView1.Rows[RowIndex].Cells[3].Value.ToString();
            int NumberOfPages = Convert.ToInt32(dataGridView1.Rows[RowIndex].Cells[4].Value);
            string Type = dataGridView1.Rows[RowIndex].Cells[5].Value.ToString();
            string Language = dataGridView1.Rows[RowIndex].Cells[6].Value.ToString();
            string Publisher = dataGridView1.Rows[RowIndex].Cells[7].Value.ToString();
            string Year = dataGridView1.Rows[RowIndex].Cells[8].Value.ToString();
            DB.UpdateBook(ID, BookName, BarcodeNumber, Author, NumberOfPages, Type, Language, Publisher, Year);
        }
    }
}
