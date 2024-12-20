﻿using System;
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
            SqlConnection con = new SqlConnection(Constr);
            SqlDataAdapter da = new SqlDataAdapter("", con);
            da.Fill(ds);
            return ds;

        }

        public static void SaveBook(int BookID, string BarcodeNumber, string BookName, string Author, int NumberOfPages, string Type, string Language, string Publisher, string Year, byte [] Picture)
        {
            SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand("if not exists(select * from Book where BookID=@ID) " +
                "insert into Book (BookName,BarcodeNumber,Author,NumberOfPages,Type,Picture,Language,Publisher,Year) " +
                "values (@BookName,@BarcodeNumber,@Author,@NumberOfPages,@Type,@Picture,@Language,@Publisher,@Year)" + //Bu saırla verilerin parametre olarak girilmesini sağladık. //With this command, we enabled the data to be entered as parameters
                " else update Book set BookName=@BookName,BarcodeNumber=@BarcodeNumber,Author=@Author,NumberOfPages=@NumberOfPages,Type=@Type,Picture=@Picture,Language=@Language,Publisher=@Publisher,Year=@Year Where BookID =@ID", con);
            com.Parameters.AddWithValue("@ID",BookID);
            com.Parameters.AddWithValue("@BarcodeNumber",BarcodeNumber);
            com.Parameters.AddWithValue("@BookName", BookName);
            com.Parameters.AddWithValue("@Author", Author);
            com.Parameters.AddWithValue("@NumberOfPages", NumberOfPages);
            com.Parameters.AddWithValue("@Type", Type);
            com.Parameters.AddWithValue("@Language", Language);
            com.Parameters.AddWithValue("@Publisher", Publisher);
            com.Parameters.AddWithValue("@Year", Year);
            if (Picture == null)
            {
                com.Parameters.AddWithValue("@Picture", 0);
            }
            else { 
                com.Parameters.AddWithValue("@Picture", Picture);            
            }
 
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            BackDatabase();
        }

        public static void DeleteBook(int BookID) 
        {
            SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand("Delete from Book Where BookID = @ID", con);
            com.Parameters.AddWithValue("@ID", BookID);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            BackDatabase();
        }
        public static AutoCompleteStringCollection Authors()
        {
            AutoCompleteStringCollection authors = new AutoCompleteStringCollection();
            SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand("select distinct Author from Book", con);
            con.Open();
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                authors.Add(dr["Author"].ToString());
            }
            dr.Close(); 
            con.Close();
            return authors;
        }
        public static void BackDatabase()
        {
            SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand("backup database BookStore to disk='C:\\BookStore\\BookStore.bak' with format",con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }

        public static DataSet SearchBook(string text, string radiobutton)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(Constr);
            switch(radiobutton)
            {
                case "Barcode Number":
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select BookID as [ID], BookName as [Book Name], BarcodeNumber as [Barcode Number], Author as [Author], NumberOfPages as [Number of Pages], Type as [Type], Picture as [Picture], Language as [Language], Publisher as [Publisher], Year as [Year] from Book Where Barcode Number like '%'+@BarcodeNumber +'%' ", con);
                        da.SelectCommand.Parameters.AddWithValue("@BarcodeNumber", text);
                        da.Fill(ds);
                        break;
                    }
                case "Book Name":
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select BookID as [ID], BookName as [Book Name], BarcodeNumber as [Barcode Number], Author as [Author], NumberOfPages as [Number of Pages], Type as [Type], Picture as [Picture], Language as [Language], Publisher as [Publisher], Year as [Year] from Book Where BookName like '%'+@BookName +'%' ", con);
                        da.SelectCommand.Parameters.AddWithValue("@BookName", text.ToUpper());
                        da.Fill(ds);
                        break;
                    }
                case "Author":
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select BookID as [ID], BookName as [Book Name], BarcodeNumber as [Barcode Number], Author as [Author], NumberOfPages as [Number of Pages], Type as [Type], Picture as [Picture], Language as [Language], Publisher as [Publisher], Year as [Year] from Book Where Author like '%'+@Author +'%' ", con);
                        da.SelectCommand.Parameters.AddWithValue("@Author", text.ToUpper());
                        da.Fill(ds);
                        break;
                    }
            }
            return ds;
        }
    }
}
