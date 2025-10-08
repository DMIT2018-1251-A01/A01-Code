<Query Kind="Program">
  <Connection>
    <ID>53bfbe22-8f63-4146-92e7-5b76f960a947</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>OLTP-DMIT2018</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
  <NuGetReference>BYSResults</NuGetReference>
</Query>

// 	Lightweight result types for explicit success/failure 
//	 handling in .NET applications.
using BYSResults;

// —————— PART 1: Main → UI ——————
//	Driver is responsible for orchestrating the flow by calling 
//	various methods and classes that contain the actual business logic 
//	or data processing operations.
void Main()
{
	CodeBehind codeBehind = new CodeBehind(this); // “this” is LINQPad’s auto Context

	#region Get Customers (GetCustomers)
	string lastName = string.Empty;
	string phone = string.Empty;

	//	rule:	Both last name and phone number cannot be empty
	codeBehind.GetCustomers(lastName, phone);
	codeBehind.ErrorDetails.Dump("Both last name and phone number cannot be empty");
	
	//	rule: both the last name and phone must be valid
	lastName = "zzz";
	phone = "999999";
	codeBehind.GetCustomers(lastName, phone);
	codeBehind.ErrorDetails.Dump($"No customer with last name {lastName} & phone number of {phone}");

	//	rule: both the last name and phone were provided
	lastName = "s";
	phone = "558";
	codeBehind.GetCustomers(lastName, phone);
	codeBehind.Customers.Dump($"Pass - Valid last name {lastName} & phone number of {phone}");
	codeBehind.errorMessage.Dump();

	//	rule: last name was provided
	lastName = "s";
	phone = string.Empty;
	codeBehind.GetCustomers(lastName, phone);
	codeBehind.Customers.Dump($"Pass - Valid last name {lastName}");

	//	rule: phone was provided
	lastName = string.Empty;
	phone = "558";
	codeBehind.GetCustomers(lastName, phone);
	codeBehind.Customers.Dump($"Pass - Valid phone {phone}");

	#endregion

}

// ———— PART 2: Code Behind → Code Behind Method ————
// This region contains methods used to test the functionality
// of the application's business logic and ensure correctness.
// NOTE: This class functions as the code-behind for your Blazor pages
#region Code Behind Methods
public class CodeBehind(TypedDataContext context)
{
	#region Supporting Members (Do not modify)
	// exposes the collected error details
	public List<string> ErrorDetails => errorDetails;

	// Mock injection of the service into our code-behind.
	// You will need to refactor this for proper dependency injection.
	// NOTE: The TypedDataContext must be passed in.
	private readonly Library YourService = new Library(context);
	#endregion

	#region Fields from Blazor Page Code-Behind
	// feedback message to display to the user.
	private string feedbackMessage = string.Empty;
	// collected error details.
	private List<string> errorDetails = new();
	// general error message.
	public string errorMessage = string.Empty;
	#endregion

	//	in your UI, you may need to sway default! with new()
	public List<CustomerSearchView> Customers { get; set; } = new();

	public void GetCustomers(string lastName, string phone)
	{
		// clear the previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = string.Empty;

		//	wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetCustomers(lastName, phone);
			if (result.IsSuccess)
			{
				Customers = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			//	capture any exception message for display
			errorMessage = ex.Message;
		}
	}

}
#endregion

// ———— PART 3: Database Interaction Method → Service Library Method ————
//	This region contains support methods for testing
#region Methods
public class Library
{
	#region Data Context Setup
	// The LINQPad auto-generated TypedDataContext instance used to query and manipulate data.
	private readonly TypedDataContext _hogWildContext;

	// The TypedDataContext provided by LINQPad for database access.
	// Store the injected context for use in library methods
	// NOTE:  This constructor is simular to the constuctor in your service
	public Library(TypedDataContext context)
	{
		_hogWildContext = context
					?? throw new ArgumentNullException(nameof(context));
	}
	#endregion

	public Result<List<CustomerSearchView>> GetCustomers(string lastName, string phone)
	{
		//	Create a Result container that will hold either a 
		//		CustomerSerachView Objects on success or any accumulated errors on failure
		var result = new Result<List<CustomerSearchView>>();

		#region Business Rules
		//  These are processing rules that need to be satisfied
		//		for valid data

		//	rule:	Both last name and phone number cannot be empty
		//	rule:	RemoveFromViewFlag must be false (soft delete).  Found in .Where clause.
		if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(phone))
		{
			//	need to exit because we have nothing to search on
			return result.AddError(new Error("Missing Information",
								"Please provied either a last name and/or phone number"));
		}
		#endregion

		//  update for either empty lastname or phone with GUID
//		if (string.IsNullOrWhiteSpace(lastName))
//		{
//			lastName = Guid.NewGuid().ToString();
//		}
//
//		if (string.IsNullOrWhiteSpace(phone))
//		{
//			phone = Guid.NewGuid().ToString();
//		}


		//	filter rules
		//	1)	only apply lastname filter if supplied
		//	2)	only apply phone filter if supplied
		//	3)	always exclude removed records

		var customers = _hogWildContext.Customers
							.Where(c => (string.IsNullOrWhiteSpace(lastName)
								|| c.LastName.ToUpper().Contains(lastName.ToUpper())) // 1
							&& (string.IsNullOrWhiteSpace(phone)
							|| c.Phone.Contains(phone)) // 2
							&& !c.RemoveFromViewFlag // 3
							)
							.Select(c => new CustomerSearchView
							{
								CustomerID = c.CustomerID,
								FirstName = c.FirstName,
								LastName = c.LastName,
								City = c.City,
								Phone = c.Phone,
								Email = c.Email,
								StatusID = c.StatusID,
								TotalSales = (decimal?)c.Invoices.Sum(i => i.SubTotal + i.Tax) ?? 0
							})
						.OrderBy(c => c.LastName)
							.ToList();

		//	if no customers were found with either the last name or phone number
		if (customers == null || customers.Count() == 0)
		{
			//	need to exit because we did not find any customers
			return result.AddError(new Error("No Customers", "No customers were found"));
		}
		return result.WithValue(customers);
	}
}
#endregion

// ———— PART 4: View Models → Service Library View Model ————
//	This region includes the view models used to 
//	represent and structure data for the UI.
#region View Models
public class CustomerSearchView
{
	//  Customer ID
	public int CustomerID { get; set; }
	//  First name
	public string FirstName { get; set; }
	//  last name
	public string LastName { get; set; }
	//  city
	public string City { get; set; }
	//  contact phone number
	public string Phone { get; set; }
	//  email address
	public string Email { get; set; }
	//  status ID.  Status value will use a dropdown and the Lookup View Model
	public int StatusID { get; set; }
	//  Invoice.SubTotal +  Invoice.Tax	
	public decimal TotalSales { get; set; }
}

#endregion

//	This region includes support methods
#region Support Method
// Converts a list of error objects into their string representations.
public static List<string> GetErrorMessages(List<Error> errorMessage)
{
	// Initialize a new list to hold the extracted error messages
	List<string> errorList = new();

	// Iterate over each Error object in the incoming list
	foreach (var error in errorMessage)
	{
		// Convert the current Error to its string form and add it to errorList
		errorList.Add(error.ToString());
	}

	// Return the populated list of error message strings
	return errorList;
}
#endregion