using Library;
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
using System.Xml.Linq;

namespace Library
{
    public partial class Book : Form
    {
        public Book()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("If this window is closed, the data you entered will be deleted.\n Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                this.Close();
            }
        }

   
        
  

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            bool Full = true;
            foreach (Control c in this.Controls)
            {
                if (c is TextBox && c.TabStop == true)
                {
                    c.BackColor = Color.White;
                }

            }

            foreach (Control c in this.Controls)
            {
                if (c is TextBox && c.TabStop == true)
                {
                    // Tag değerinin "B" olup olmadığını kontrol ediyoruz
                    if (c.Tag != null && c.Tag.ToString() == "B")
                    {
                        // Eğer TextBox'ın içi boşsa, arka planı kırmızı yapıyoruz
                        if (c.Text == string.Empty)
                        {
                            Full = false;
                            c.BackColor = Color.Red;
                        }
                    }
                }

            }

            if (!Full)
            {
                MessageBox.Show("Barkod numarası ve kitap adı sekmeleri boş bırakılamaz", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
             

            }
            

            //Veri Tabanına veri girişi yapıyoruz burda

            DB.SaveBook(DB.BookID, txtBarcodeNumber.Text, txtBookName.Text, txtAuthor.Text,txtNumberOfPages.Text==string.Empty ? 0 : Convert.ToInt32(
                txtNumberOfPages.Text), txtType.Text, txtLanguage.Text, txtPublisher.Text,txtYear.Text, PicData);

        }

        byte[] PicData = null;
        private void pboxBook_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog selectPic = new OpenFileDialog();
            selectPic.Filter = "(*.jpg)|*.jpg";
            if (selectPic.ShowDialog() == DialogResult.No) return;
            PicData=File.ReadAllBytes(selectPic.FileName);
            pboxBook.Image=Image.FromFile(selectPic.FileName);

        }

       
    }
}




