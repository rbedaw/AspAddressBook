using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AddressBook
{
    public partial class Contacts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateContacts();
            }
        }

        //function to populate the GridView with Contacts.
        private void PopulateContacts()
        {
            List<Contact> allContacts = null;
            using (Database1Entities dc = new Database1Entities())
            {                     // a -> contact
                var contacts = (from contact in dc.Contacts
                                join b in dc.PhoneTypes on contact.ContactPhoneType equals b.ContactPhoneType
                                select new             // a -> contact
                                { // a -> contact
                                    contact,
                                    b.PhoneTypeName
                                });
                if (contacts != null)
                {
                    allContacts = new List<Contact>();
                    foreach (var i in contacts)
                    {           // a -> contact
                        Contact c = i.contact;
                        c.PhoneTypeName = i.PhoneTypeName;
                        allContacts.Add(c);
                    }
                }

                if (allContacts == null || allContacts.Count == 0)
                {
                    // Tells the footer that there's no data in GridView.
                    allContacts.Add(new Contact());
                    myGridView.DataSource = allContacts;
                    myGridView.DataBind();
                    myGridView.Rows[0].Visible = false;
                }
                else
                {
                    myGridView.DataSource = allContacts;
                    myGridView.DataBind();
                }

                // Populate & bind PhoneType.
                if ( myGridView.Rows.Count > 0)
                {
                    DropDownList dd = (DropDownList)myGridView.FooterRow.FindControl("ddPhoneType");
                    BindPhoneType(dd, PopulatePhoneType());
                }
            }
        }

        //function to fetch PhoneType.
        private List<PhoneType> PopulatePhoneType()
        {
            using (Database1Entities dc = new Database1Entities())
            {
                return dc.PhoneTypes.OrderBy(a => a.PhoneTypeName).ToList();
            }
        }

        //function to bind PhoneType. (26.40)
        private void BindPhoneType(DropDownList ddPhoneType, List<PhoneType> phoneType)
        {
            ddPhoneType.Items.Clear();
            ddPhoneType.Items.Add(new ListItem { Text = "Select type of phone", Value = "0" });
            ddPhoneType.AppendDataBoundItems = true;
            ddPhoneType.DataTextField = "PhoneTypeName";
            ddPhoneType.DataValueField = "ContactPhoneType";
            ddPhoneType.DataSource = phoneType;
            ddPhoneType.DataBind();
        }


        protected void ddPhoneType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void myGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // insert a new contact.
            if (e.CommandName == "Insert")
            {
                Page.Validate("Add");
                if (Page.IsValid)
                {
                    var fRow = myGridView.FooterRow;
                    TextBox txtContactFirst = (TextBox)fRow.FindControl("txtContactFirst");
                    TextBox txtContactLast = (TextBox)fRow.FindControl("txtContactLast");
                    TextBox txtContactNo = (TextBox)fRow.FindControl("txtContactNo");
                    DropDownList ddPhoneType = (DropDownList)fRow.FindControl("ddPhoneType");

                    using (Database1Entities dc = new Database1Entities())
                    {
                        dc.Contacts.Add(new Contact
                        {
                            ContactFirstName = txtContactFirst.Text.Trim(),
                            ContactLastName = txtContactLast.Text.Trim(),
                            ContactPhone = txtContactNo.Text.Trim(),
                            ContactPhoneType = Convert.ToInt32(ddPhoneType.SelectedValue)
                        });
                        dc.SaveChanges();
                        PopulateContacts();
                    }
                }
            }
        }
    }
}