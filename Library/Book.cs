using Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        public int BookID { get; set; }
        public string BarcodeNumber { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int NumberOfPages { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public string Publisher { get; set; }
        public string Year { get; set; }
        public Book()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("If this window is closed, the data you entered will be deleted.\n Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.BookID = 0;
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
                MessageBox.Show("Please fill in the required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            

            //Veri Tabanına veri girişi yapıyoruz burda

            DB.SaveBook(DB.BookID, txtBarcodeNumber.Text, txtBookName.Text, txtAuthor.Text,txtNumberOfPages.Text==string.Empty ? 0 : Convert.ToInt32(
                txtNumberOfPages.Text), txtType.Text, txtLanguage.Text, txtPublisher.Text,txtYear.Text, PicData);
            DB.BookID = 0;
            this.Close();
        }

        byte[] PicData = null;
        private void pboxBook_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog selectPic = new OpenFileDialog();
            selectPic.Filter = "(*.jpg)|*.jpg";
            if (selectPic.ShowDialog() == DialogResult.No) return;
            PicData=File.ReadAllBytes(selectPic.FileName);
            pboxUser.Image=Image.FromFile(selectPic.FileName);
        }

        private void Book_Load(object sender, EventArgs e)
        {
            
        }

        private void pboxBook_Click(object sender, EventArgs e)
        {

        }

        private void Book_Load_1(object sender, EventArgs e)
        {
            Options();  // Genel ayarları yapıyoruz

            // Kitap ID'si geçerliyse işlemlere devam ediyoruz
            if (DB.BookID > 0)
            {
                txtBarcodeNumber.ReadOnly = true;  // Barkod numarasını düzenlenemez yapıyoruz

                // Veritabanı bağlantısını kullanmak için 'using' ifadesi kullanıyoruz
                using (SqlConnection con = new SqlConnection(DB.Constr))
                {
                    // SQL sorgusu hazırlıyoruz
                    SqlCommand com = new SqlCommand("SELECT * FROM Book WHERE BookID = @ID", con);
                    com.Parameters.AddWithValue("@ID", DB.BookID);  // Parametreyi ekliyoruz

                    try
                    {
                        con.Open();  // Bağlantıyı açıyoruz

                        // Verileri okumak için SqlDataReader kullanıyoruz
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            string fileName = "";  // Resim dosyasının adı

                            // Her bir kitap kaydını işleyecek döngü
                            while (dr.Read())
                            {
                                // Kitap bilgilerini TextBox'lara yerleştiriyoruz
                                txtBookName.Text = dr["BookName"].ToString();
                                txtBarcodeNumber.Text = dr["BarcodeNumber"].ToString();
                                txtAuthor.Text = dr["Author"].ToString();
                                txtNumberOfPages.Text = dr["NumberOfPages"].ToString();
                                txtType.Text = dr["Type"].ToString();
                                txtLanguage.Text = dr["Language"].ToString();
                                txtPublisher.Text = dr["Publisher"].ToString();
                                txtYear.Text = dr["Year"].ToString();

                                // Resim verisini alıyoruz
                                byte[] PicData = (byte[])dr["Picture"];

                                // Resim verisi varsa dosyaya kaydediyoruz
                                if (PicData.Length > 4)
                                {
                                    Guid guid = Guid.NewGuid();  // Benzersiz bir GUID oluşturuyoruz
                                    fileName = Path.Combine("C:\\BookStore\\Picture\\Images", guid.ToString().Substring(0, 5) + ".jpeg");

                                    // Resmi dosyaya yazıyoruz
                                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                                    {
                                        fs.Write(PicData, 0, PicData.Length);
                                        fs.Flush();  // Veriyi diske yazıyoruz
                                    }

                                    // Kaydedilen resimi formda gösteriyoruz
                                    pboxUser.Image = Image.FromFile(fileName);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda bir işlem yapılabilir
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
        }

        private void Options()
        {
            txtAuthor.AutoCompleteCustomSource = DB.Authors();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("The data you entered will be deleted.\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox && c.TabStop == true)
                    {
                        c.ResetText();
                    }
                    if (c is PictureBox)
                        (c as PictureBox).Image = null;
                }
            }
        }

    }
}




