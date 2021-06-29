using System;
using System.Linq;
using System.Windows.Forms;

namespace PhoneNumber
{
    public partial class frmMain : Form
    {
        private readonly PhoneNumberDBEntities _db;

        public frmMain()
        {
            _db = new PhoneNumberDBEntities();
            InitializeComponent();
        }

        private void btnContactAdd_Click(object sender, EventArgs e)
        {
            var contactForm = new FrmContacts();
            contactForm.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            UpdateContactGridView();
        }


        private void frmMain_Activated(object sender, EventArgs e)
        {
            UpdateContactGridView();
        }

        /* Load Contact To Grid View */
        private void UpdateContactGridView()
        {
            dataGridView1.DataSource = _db.Contacts
                .ToList()
                .Select(x => new ContactViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MobileNumber = x.MobileNumber,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    GroupContact = GetGroupName(x.GroupContact),
                    Address = x.Address,
                    Description = x.Description
                })
                .ToList();
        }

        /* Edit Contact */
        private void btnContactEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0) return;
            var ContactId = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            var con = _db.Contacts.FirstOrDefault(x => x.Id == ContactId);
            var contactForm = new FrmContacts {ContactEdit = con};
            contactForm.ShowDialog();
        }

        /* Delete Contact */
        private void btnContactRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0) return;

            var id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            var contact = _db.Contacts.Find(id);


            if (MessageBox.Show($"آیا از حذف '{contact.FirstName + " " + contact.LastName}' مخاطب مطمئن هستید؟",
                "هشدار",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                return;


            _db.Contacts.Remove(contact);

            _db.SaveChanges();

            UpdateContactGridView();
        }

        /* Search Contact */
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var contacts = _db.Contacts
                .Where(x => 
                x.FirstName.Contains(txtSearch.Text) ||
                x.LastName.Contains(txtSearch.Text) ||
                x.MobileNumber.Contains(txtSearch.Text))
                .ToList()
                .Select(x => new ContactViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MobileNumber = x.MobileNumber,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    GroupContact = GetGroupName(x.GroupContact),
                    Address = x.Address,
                    Description = x.Description
                })
                .ToList();

            dataGridView1.DataSource = contacts;
        }

        public string GetGroupName(int groupNumber)
        {
            var result = "";
            switch (groupNumber)
            {
                case 0:
                    result = "معمولی";
                    break;
                case 1:
                    result = "همکاران";
                    break;
                case 2:
                    result = "دوستان";
                    break;
                case 3:
                    result = "خانواده";
                    break;
                case 4:
                    result = "ضروری";
                    break;
            }

            return result;
        }
    }
}