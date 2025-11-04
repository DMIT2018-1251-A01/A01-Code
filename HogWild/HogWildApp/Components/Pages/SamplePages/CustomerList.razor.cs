using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class CustomerList
    {
        #region Fields
        private string lastName = string.Empty;
        private string phoneNumber = string.Empty;
        //  Tell us if the search has been performed
        private bool noRecords;
        private string feedbackMessage = string.Empty;
        private string errorMessage = string.Empty;
        private bool hasFeedBack => !string.IsNullOrWhiteSpace(feedbackMessage);
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage) || errorDetails.Count() > 0;
        //  error details
        private List<string> errorDetails = new List<string>();
        #endregion

        #region Properties
        //  injects the CustomerService dependency
        [Inject]
        protected CustomerService CustomerService { get; set; } = default!;

        //  inject the NavigatioonManager dependency
        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        //  list of customer search view
        protected List<CustomerSearchView> Customers { get; set; } = new();
        #endregion

        #region Methods
        private void Search()
        {
            // clear the previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            //	wrap the service call in a try/catch to handle unexpected exceptions
            try
            {
                var result = CustomerService.GetCustomers(lastName, phoneNumber);
                if (result.IsSuccess)
                {
                    Customers = result.Value;
                }
                else
                {
                    errorDetails = HogWildHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                //	capture any exception message for display
                errorMessage = ex.Message;
            }
        }

        //  new customer
        private void New()
        {

        }

        //  edit selected customer
        private void EditCustomer(int customerID)
        {

        }

        //  new invoice for selected customer
        private void NewInvoice(int customerID)
        {

        }

        #endregion
    }
}
