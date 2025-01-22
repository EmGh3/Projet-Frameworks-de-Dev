namespace ERP_Project.Models.viewModels
{
    public class SendEmailViewModel
    {
     
        public string EmployeeName { get; set; } // Employee Full Name
        public string EmployeeEmail { get; set; } // Employee Email

        public string Subject { get; set; } // Email Subject (optional)
        public string Body { get; set; } // Email Body (can be input by user)
        public string Signature { get; set; } // Email Signature (can be input by user)
    }

}
