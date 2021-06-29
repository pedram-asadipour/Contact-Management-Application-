using System;
using System.Windows.Forms;
using ValidationComponents;

namespace PhoneNumber
{
    public partial class FrmContacts : Form
    {
        private readonly PhoneNumberDBEntities _db;
        public Contact ContactEdit { get; set; }

        public FrmContacts()
        {
            _db = new PhoneNumberDBEntities();
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!BaseValidator.IsFormValid(this.components)) return;

            /* Edit Mode */
            if (ContactEdit != null)
            {
                ContactEdit.FirstName = txtName.Text.Trim();
                ContactEdit.LastName = txtFamily.Text.Trim();
                ContactEdit.PhoneNumber = txtPhone.Text.Trim();
                ContactEdit.MobileNumber = txtMobile.Text.Trim();
                ContactEdit.Email = txtEmail.Text.Trim();
                ContactEdit.GroupContact = cmbGroup.SelectedIndex;
                ContactEdit.Address = txtAddress.Text.Trim();
                ContactEdit.Description = txtDescription.Text.Trim();

                _db.Entry(ContactEdit).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                this.Close();
            }
            else
            {
                /* Create Mode */
                var myContact = new Contact
                {
                    FirstName = txtName.Text.Trim(),
                    LastName = txtFamily.Text.Trim(),
                    PhoneNumber = txtPhone.Text.Trim(),
                    MobileNumber = txtMobile.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    GroupContact = cmbGroup.SelectedIndex,
                    Address = txtAddress.Text.Trim(),
                    Description = txtDescription.Text.Trim()
                };

                _db.Contacts.Add(myContact);
                _db.SaveChanges();
                this.Close();
            }
        }

        private void frmContacts_Load(object sender, EventArgs e)
        {
            cmbGroup.SelectedIndex = 0;
        }

        private void frmContacts_Activated(object sender, EventArgs e)
        {
            if (this.ContactEdit != null)
            {
                this.Text = "ویرایش اطلاعات مخاطب";
                txtName.Text = ContactEdit.FirstName;
                txtFamily.Text = ContactEdit.LastName;
                txtPhone.Text = ContactEdit.PhoneNumber;
                txtMobile.Text = ContactEdit.MobileNumber;
                txtEmail.Text = ContactEdit.Email;
                cmbGroup.SelectedIndex = ContactEdit.GroupContact;
                txtAddress.Text = ContactEdit.Address;
                txtDescription.Text = ContactEdit.Description;
                btnSumbit.Text = "ویرایش";
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}