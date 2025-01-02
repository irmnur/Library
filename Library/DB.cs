using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IO;

namespace Library
{
    internal class DB
    {
        public static int BookID = 0;
        public static int SystemUserID = 0;
        public static int UserID = 0;
        public static bool Stop = false;
        public static bool Search = false;

        public static string Constr = "Data Source=DESKTOP-OSFLJP7\\SQLEXPRESS;Initial Catalog=BookStore;Integrated Security=True;";


        public static DataSet GetBooks()
        {
            DataSet ds = new DataSet();
        
            using (SqlConnection con = new SqlConnection(Constr))
            {
         
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Book", con))
                {
                    da.Fill(ds);
                }
            }
            return ds;
        }

        

       
        public static void SaveBook(int BookID, string BarcodeNumber, string BookName, string Author, int NumberOfPages, string Type, string Language, string Publisher, string Year, byte[] Picture)
        {
           
            using (SqlConnection con = new SqlConnection(Constr))
            {
                
                using (SqlCommand com = new SqlCommand("if not exists(select * from Book where BookID=@ID) " +
                    "insert into Book (BookName,BarcodeNumber,Author,NumberOfPages,Type,Picture,Language,Publisher,Year) " +
                    "values (@BookName,@BarcodeNumber,@Author,@NumberOfPages,@Type,@Picture,@Language,@Publisher,@Year)" + //Verilerin parametre olarak girilmesini sağladık
                    " else update Book set BookName=@BookName,BarcodeNumber=@BarcodeNumber,Author=@Author,NumberOfPages=@NumberOfPages,Type=@Type,Picture=@Picture,Language=@Language," +
                    "Publisher=@Publisher,Year=@Year Where BookID =@ID", con))
                {
                    com.Parameters.AddWithValue("@ID", BookID);
                    com.Parameters.AddWithValue("@BookName", BookName.ToUpper());
                    com.Parameters.AddWithValue("@BarcodeNumber", BarcodeNumber);
                    com.Parameters.AddWithValue("@Author", Author.ToUpper());
                    com.Parameters.AddWithValue("@NumberOfPages", NumberOfPages);
                    com.Parameters.AddWithValue("@Type", Type.ToUpper());
                    com.Parameters.AddWithValue("@Language", Language.ToUpper());
                    com.Parameters.AddWithValue("@Publisher", Publisher.ToUpper());
                    com.Parameters.AddWithValue("@Year", Year.ToUpper());

                    
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
            
            using (SqlConnection con = new SqlConnection(Constr))
            {
                
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

           
            using (SqlConnection con = new SqlConnection(Constr))
            {
                
                using (SqlCommand com = new SqlCommand("select distinct Author from Book", con))
                {
                    con.Open();
                    
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
            
            using (SqlConnection con = new SqlConnection(Constr))
            {
                
                using (SqlCommand com = new SqlCommand("backup database BookStore to disk='C:\\BookStore\\BookStore.bak' with format", con))
                {
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }
        }

        public static byte[] GetImageBytes(string imagePath)
        {
            byte[] imageBytes = null;
            if (!string.IsNullOrEmpty(imagePath))
            {
                
                imageBytes = File.ReadAllBytes(imagePath);
            }
            return imageBytes;
        }

        public static DataSet SearchBook(string text, string radiobutton)
        {
            DataSet ds = new DataSet();
            string query = "";  
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
                        
                        break;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                    {
                       
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

                        
                        da.Fill(ds);
                    }
                }
            }

            return ds;


        }
    }
}