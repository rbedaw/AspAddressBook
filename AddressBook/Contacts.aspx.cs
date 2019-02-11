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

        //function to bind PhoneType.
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

        protected void myGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Get PhoneType of editable row.
            string contactPhoneType = myGridView.DataKeys[e.NewEditIndex]["ContactPhoneType"].ToString();
            // Opens edit mode.
            myGridView.EditIndex = e.NewEditIndex;
            PopulateContacts();
            // Populate PhoneType & bind.
            DropDownList ddPhoneType = (DropDownList)myGridView.Rows[e.NewEditIndex].FindControl("ddPhoneType");
            if(ddPhoneType != null)
            {
                BindPhoneType(ddPhoneType, PopulatePhoneType());
                ddPhoneType.SelectedValue = contactPhoneType;
            }
        }

        protected void myGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Cancels edit mode.
            myGridView.EditIndex = -1;
            PopulateContacts();
        }

        protected void myGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Validate the page.
            Page.Validate("edit");
            if(!Page.IsValid)
            {
                return;
            }

            // Get ContactId.
            int contactId = (int)myGridView.DataKeys[e.RowIndex]["ContactId"];

            // Find controls.
            TextBox txtContactFirst = (TextBox)myGridView.Rows[e.RowIndex].FindControl("txtContactFirst");
            TextBox txtContactLast = (TextBox)myGridView.Rows[e.RowIndex].FindControl("txtContactLast");
            TextBox txtContactNo = (TextBox)myGridView.Rows[e.RowIndex].FindControl("txtContactNo");
            DropDownList ddPhoneType = (DropDownList)myGridView.Rows[e.RowIndex].FindControl("ddPhoneType");

            // Get values.
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.Contacts.Where(a => a.ContactPhoneType.Equals(contactId)).FirstOrDefault();
                if(v != null)
                {
                    v.ContactFirstName = txtContactFirst.Text.Trim();
                    v.ContactLastName = txtContactLast.Text.Trim();
                    v.ContactNo = txtContactNo.Text.Trim();
                    v.ContactPhoneType = Convert.ToInt32(ddPhoneType.SelectedValue);
                }
                dc.SaveChanges();
                myGridView.EditIndex = -1;
                PopulateContacts();
            }
        }

        protected void myGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int contactId = (int)myGridView.DataKeys[e.RowIndex]["ContactId"];
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.Contacts.Where(a => a.ContactId.Equals(contactId)).FirstOrDefault();
                if(v != null)
                {
                    dc.Contacts.Remove(v);
                    dc.SaveChanges();
                    PopulateContacts();
                }
            }
        }
    }
}