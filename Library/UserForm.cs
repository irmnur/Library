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
    public partial class User_Form : Form
    {
        public User_Form()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Girmiş olduğunuz veriler silinecektir.\nDevam edilsin mi?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                foreach (Control c in this.Controls)
                {
                    if (c is TextBox && c.TabStop == true)
                    {
                        c.ResetText();
                    }
                    if (c is MaskedTextBox)
                        c.ResetText();
                    if (c is PictureBox)
                        (c as PictureBox).Image = null;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("If this window is closed, the data you entered will be deleted.\n Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.SystemUserID = 0;
                this.Close();
            }
        }
        byte[] PicData = null;
        private void pboxUser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog selectPic = new OpenFileDialog();
            selectPic.Filter = "(*.jpg)|*.jpg";
            if (selectPic.ShowDialog() == DialogResult.No) return;
            PicData = File.ReadAllBytes(selectPic.FileName);
            pboxBook.Image = Image.FromFile(selectPic.FileName);
        }
        private void btnSave_Click(object sender, EventArgs e)
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
                if (c is TextBox && c.TabStop == true && c.Text == string.Empty)
                {
                    c.BackColor = Color.MediumVioletRed;
                    Full = false;
                }
            }
            if (!Full)
            {
                MessageBox.Show("Please fill in the required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPassword.Text != txtPasswordRepeat.Text)
            {
                MessageBox.Show("Passwords do not match.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DB.SystemUser(DB.SystemUserID, txtName.Text, txtSurname.Text, txtEmail.Text, dtpBirthDate.Value.ToString(), txtPhoneNumber.Text, txtAddress.Text, PicData, txtPassword.Text);
        }

        private void txtPhoneNumber_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
