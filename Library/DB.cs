using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Library
{
    internal class DB
    {
        public static int BookID = 0;
        public static int SystemUserID = 0;
        public static int UserID = 0;
        public static bool Stop = false;
        public static bool Search = false;
        public static string Constr = "HIKAMSE\\SQLEXPRESS;database=BookStore;Integrated Security=True;";


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
        public static void SystemUser(int SystemUserID, string UserName, string UserSurname, string Email, string BirthDate, string PhoneNumber, string Address, byte[] UserPicture, string Password)
        {
            // SqlConnection'ı using ile kullanıyoruz
            using (SqlConnection con = new SqlConnection(Constr))
            {
                // SqlCommand'ı using ile kullanıyoruz
                using (SqlCommand com = new SqlCommand("IF NOT EXISTS (SELECT * FROM SystemUser WHERE SystemUserID = @ID) " +
                                                        "INSERT INTO SystemUser (UserName, UserSurname, Email, BirthDate, PhoneNumber, Address, UserPicture, Password) " +
                                                        "VALUES (@UserName, @UserSurname, @Email, @BirthDate, @PhoneNumber, @Address, @UserPicture, Encryptbypassphase('Moka', Convert(varbinary(max), @Password))) " +
                                                        "ELSE " +
                                                        "UPDATE SystemUser SET UserName = @UserName, UserSurname = @UserSurname, Email = @Email, BirthDate = @BirthDate, " +
                                                        "PhoneNumber = @PhoneNumber, Address = @Address, UserPicture = @UserPicture, Password = @Password " +
                                                        "WHERE SystemUserID = @ID", con))
                {
                    com.Parameters.AddWithValue("@ID", SystemUserID);
                    com.Parameters.AddWithValue("@UserName", UserName.ToUpper());
                    com.Parameters.AddWithValue("@UserSurname", UserSurname.ToUpper());
                    com.Parameters.AddWithValue("@Email", Email.ToLower());
                    com.Parameters.AddWithValue("@BirthDate", BirthDate);
                    com.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                    com.Parameters.AddWithValue("@Address", Address.ToUpper());
                    com.Parameters.AddWithValue("@Password", Password);

                    // Eğer resim null ise, 0 olarak ekliyoruz
                    if (UserPicture == null)
                    {
                        com.Parameters.AddWithValue("@UserPicture", 0);
                    }
                    else
                    {
                        com.Parameters.AddWithValue("@UserPicture", UserPicture);
                    }
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }

            BackDatabase();  // Database işlemleri sonrasında çağrılan metod
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