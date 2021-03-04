using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CBAData.Models
{
    public enum Permissions
    {
        [Description("Allows the current user to add new users to the system"), Display(Name = "Add new user")]
        CBA001,
        [Description("Allows the user with this function to edit existing user information in the system"), Display(Name = "Edit user information")]
        CBA002,
        [Description("Allows the user with this function to change his/her login password"), Display(Name = "Change user password")]
        CBA003,
        [Description("Allows the user with this function to reset another user's password"), Display(Name = "Reset user password")]
        CBA004,
        [Description("Allows the user with this function to view list of existing users"), Display(Name = "View list of users")]
        CBA005,
        [Description("Allows the user with this function to assign a till account to another user or himself"), Display(Name = "Assign a Till account to a user")]
        CBA006,
        [Description("Allows the user with this function to view list of existing tellers"), Display(Name = "View list of tellers ")]
        CBA007,
        [Description("Allows the user with this function to add a new customer"), Display(Name = "Add a new customer")]
        CBA008,
        [Description("Allows the user with this function to edit existing customer information"), Display(Name = "Edit customer info")]
        CBA009,
        [Description("Allows the user with this function to view list of existing customers"), Display(Name = "View list of all customers")]
        CBA010,
        [Description("Allows the user with this function to create a new account for any customer"), Display(Name = "Add a new customer account")]
        CBA011,
        [Description("Allows the user with this function to edit existing customer account information"), Display(Name = "Edit a customer account")]
        CBA012,
        [Description("Allows the user with this function to close a customer account"), Display(Name = "Close account")]
        CBA013,
        [Description("Allows the user with this function to view list of existing customer accounts"), Display(Name = "View list of all customer accounts")]
        CBA014,
        [Description("Allows the user with this function to edit savings account type setup information"), Display(Name = "Edit the savings account type configuration")]
        CBA015,
        [Description("Allows the user with this function to edit current account type setup information"), Display(Name = "Edit the current account type configuration")]
        CBA016,
        [Description("Allows the user with this function to edit loan account type setup information"), Display(Name = "Edit the loan account type configuration")]
        CBA017,
        [Description("Allows the user with this function to add a new GL Category"), Display(Name = "Add new GL category")]
        CBA018,
        [Description("Allows the user with this function to edit an existing GL Category"), Display(Name = "Edit GL category")]
        CBA019,
        [Description("Allows the user with this function to view existing GL Categories"), Display(Name = "View existing GL categories")]
        CBA020,
        [Description("Allows the user with this function to add a new GL Account"), Display(Name = "Add new GL account")]
        CBA021,
        [Description("Allows the user with this function to edit an existing GL Account"), Display(Name = "Edit GL accounts")]
        CBA022,
        [Description("Allows the user with this function to view existing GL Accounts"), Display(Name = "View existing GL accounts")]
        CBA023,
        [Description("Allows the user with this function to post transaction between GL's"), Display(Name = "Post transaction into GLs")]
        CBA024,
        [Description("Allows the user with this function to view GL to GL posting transactions"), Display(Name = "View list of GL postings")]
        CBA025,
        [Description("Allows the user with this function to perform deposit, withdrawal into customer accounts"), Display(Name = "Post transaction into customer accounts")]
        CBA026,
        [Description("Allows the user with this function to view teller account based transactions"), Display(Name = "View list of teller postings")]
        CBA027,
        [Description("Allows the user with this function to Generate and view Profit and Loss Report"), Display(Name = "Generate and view P&L report")]
        CBA028,
        [Description("Allows the user with this function to view balance sheet Report"), Display(Name = "Generate and view balance sheet report")]
        CBA029,
        [Description("Allows the user with this function to Generate and view the trial balance report"), Display(Name = "Trial Balance")]
        CBA030,
        [Description("Allows the user with this function to Close business activities"), Display(Name = "Close business")]
        CBA031,
        [Description("Allows the user with this function to Open business activities"), Display(Name = "Open business")]
        CBA032
    }
}
