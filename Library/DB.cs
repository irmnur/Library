using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    internal class DB
    {
        public static int BookID = 0;
        public static bool Stop = false;
        public static bool Search = false;
        public static string Constr = "server=HIKAMSE\\SQLEXPRESS;database=BookStore;Integrated Security=True;";


        public static DataSet GetBooks()
        {
            DataSet ds = new DataSet();
            // SqlConnection'ı using ile kullanıyoruz
            using (SqlConnection con = new SqlConnection(Constr))
            {
                // SqlDataAdapter da 'using' ile kullanılmalı
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Book", con))
                {
                    da.Fill(ds);
                }
            }
            return ds;
        }

        public static void SaveBook(int BookID, string BarcodeNumber, string BookName, string Author, int NumberOfPages, string Type, string Language, string Publisher, string Year, byte[] Picture)
        {
            // SqlConnection'ı using ile kullanıyoruz
            using (SqlConnection con = new SqlConnection(Constr))
            {
                // SqlCommand'ı using ile kullanıyoruz
                using (SqlCommand com = new SqlCommand("if not exists(select * from Book where BookID=@ID) " +
                    "insert into Book (BookName,BarcodeNumber,Author,NumberOfPages,Type,Picture,Language,Publisher,Year) " +
                    "values (@BookName,@BarcodeNumber,@Author,@NumberOfPages,@Type,@Picture,@Language,@Publisher,@Year)" + //Verilerin parametre olarak girilmesini sağladık
                    " else update Book set BookName=@BookName,BarcodeNumber=@BarcodeNumber,Author=@Author,NumberOfPages=@NumberOfPages,Type=@Type,Picture=@Picture,Language=@Language,Publisher=@Publisher,Year=@Year Where BookID =@ID", con))
                {
                    com.Parameters.AddWithValue("@ID", BookID);
                    com.Parameters.AddWithValue("@BarcodeNumber", BarcodeNumber);
                    com.Parameters.AddWithValue("@BookName", BookName);
                    com.Parameters.AddWithValue("@Author", Author);
                    com.Parameters.AddWithValue("@NumberOfPages", NumberOfPages);
                    com.Parameters.AddWithValue("@Type", Type);
                    com.Parameters.AddWithValue("@Language", Language);
                    com.Parameters.AddWithValue("@Publisher", Publisher);
                    com.Parameters.AddWithValue("@Year", Year);

                    // Eğer resim null ise, 0 olarak ekliyoruz
                    if (Picture == null)
                    {
                        com.Parameters.AddWithValue("@Picture", 0);
                    }
                    else
                    {
                        com.Parameters.AddWithValue("@Picture", Picture);
                    }

                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            BackDatabase();
        }

        public static void DeleteBook(int BookID)
        {
            // SqlConnection'ı using ile kullanıyoruz
            using (SqlConnection con = new SqlConnection(Constr))
            {
                // SqlCommand'ı using ile kullanıyoruz
                using (SqlCommand com = new SqlCommand("Delete from Book Where BookID = @ID", con))
                {
                    com.Parameters.AddWithValue("@ID", BookID);
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
            BackDatabase();
        }
        public static AutoCompleteStringCollection Authors()
        {
            AutoCompleteStringCollection authors = new AutoCompleteStringCollection();

            // SqlConnection'ı using ile kullanıyoruz
            using (SqlConnection con = new SqlConnection(Constr))
            {
                // SqlCommand'ı using ile kullanıyoruz
                using (SqlCommand com = new SqlCommand("select distinct Author from Book", con))
                {
                    con.Open();
                    // SqlDataReader'ı using ile kullanıyoruz
                    using (SqlDataReader dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            authors.Add(dr["Author"].ToString());
                        }
                    }
                }
            }
            return authors;
        }
        public static void BackDatabase()
        {
            // SqlConnection'ı using ile kullanıyoruz
            using (SqlConnection con = new SqlConnection(Constr))
            {
                // SqlCommand'ı using ile kullanıyoruz
                using (SqlCommand com = new SqlCommand("backup database BookStore to disk='C:\\BookStore\\BookStore.bak' with format", con))
                {
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
        }

        public static DataSet SearchBook(string text, string radiobutton)
        {
            DataSet ds = new DataSet();
            string query = "";  // SQL sorgusunu buraya depolayacağız

            // Veritabanı bağlantısını 'using' bloğuyla kullanalım
            using (SqlConnection con = new SqlConnection(Constr))
            {
                con.Open();

                switch (radiobutton)
                {
                    case "Barcode Number":
                        query = "select BookID as [ID], BookName as [Book Name], BarcodeNumber as [Barcode Number], Author as [Author], " +
                                "NumberOfPages as [Number of Pages], Type as [Type], Picture as [Picture], Language as [Language], " +
                                "Publisher as [Publisher], Year as [Year] from Book WHERE BarcodeNumber LIKE '%' + @BarcodeNumber + '%'";
                        break;

                    case "Book Name":
                        query = "select BookID as [ID], BookName as [Book Name], BarcodeNumber as [Barcode Number], Author as [Author], " +
                                "NumberOfPages as [Number of Pages], Type as [Type], Picture as [Picture], Language as [Language], " +
                                "Publisher as [Publisher], Year as [Year] from Book WHERE BookName LIKE '%' + @BookName + '%'";
                        break;

                    case "Author":
                        query = "select BookID as [ID], BookName as [Book Name], BarcodeNumber as [Barcode Number], Author as [Author], " +
                                "NumberOfPages as [Number of Pages], Type as [Type], Picture as [Picture], Language as [Language], " +
                                "Publisher as [Publisher], Year as [Year] from Book WHERE Author LIKE '%' + @Author + '%'";
                        break;

                    default:
                        // Geçerli bir sorgu yoksa boş bırakabiliriz
                        break;
                }

                // Sorgu boş değilse, SqlDataAdapter ile veri çekmeye başlıyoruz
                if (!string.IsNullOrEmpty(query))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                    {
                        // Parametre ekliyoruz
                        switch (radiobutton)
                        {
                            case "Barcode Number":
                                da.SelectCommand.Parameters.AddWithValue("@BarcodeNumber", text);
                                break;
                            case "Book Name":
                                da.SelectCommand.Parameters.AddWithValue("@BookName", text.ToUpper());
                                break;
                            case "Author":
                                da.SelectCommand.Parameters.AddWithValue("@Author", text.ToUpper());
                                break;
                        }

                        // Veriyi DataSet'e yüklüyoruz
                        da.Fill(ds);
                    }
                }
            }

            return ds;


        }
    }
}