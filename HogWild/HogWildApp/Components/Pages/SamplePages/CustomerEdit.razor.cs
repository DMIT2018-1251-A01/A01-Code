using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.Icons;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class CustomerEdit
    {
        #region Fields
        private string feedbackMessage = string.Empty;
        private string errorMessage = string.Empty;
        private bool hasFeedBack => !string.IsNullOrWhiteSpace(feedbackMessage);
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage) || errorDetails.Count() > 0;
        //  error details
        private List<string> errorDetails = new List<string>();

        //  the customer
        private CustomerEditView customer = new();
        //  mudform control
        private MudForm customerForm = new();
        #endregion


        #region Properties

        //  customer service
        [Inject]
        protected CustomerService CustomerService { get; set; } = default!;

        // Customer ID used to create or edit a customer
        [Parameter]
        public int CustomerID { get; set; } = 0;
        #endregion

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            try
            {
                //	clear previous error details and messages
                errorDetails.Clear();
                errorMessage = string.Empty;
                feedbackMessage = string.Empty;

                //	wrap the service call in a try/catch to handle unexpected exceptions
                try
                {
                    if (CustomerID > 0)
                    {
                        var result = CustomerService.GetCustomer(CustomerID);
                        if (result.IsSuccess)
                        {
                            customer = result.Value;
                        }
                        else
                        {
                            errorDetails = HogWildHelperClass.GetErrorMessages(result.Errors.ToList());
                        }
                    }
                    else
                    {
                        customer = new CustomerEditView();
                    }
                }
                catch (Exception ex)
                {
                    //	capture any exception message for display
                    errorMessage = ex.Message;
                }
            }
            catch (Exception ex)
            {
                //	capture any exception message for display
                errorMessage = ex.Message;
            }
        }

        #endregion
    }
}
