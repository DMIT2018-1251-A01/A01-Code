<Query Kind="Program">
  <Connection>
    <ID>e0a87a77-277f-494c-93a7-51c2205344d2</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook-2025</Database>
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

	#region Get Artist (GetArtist)
	int id = 0;
	// 	fail
	//	Rule:  artistID must be valid
	codeBehind.GetArtist(id);
	codeBehind.ErrorDetails.Dump($"ArtistID must be valid. Not id {id}");

	//	fail
	//	if not artist were found with the artist id provided
	id = 10000;
	codeBehind.GetArtist(id);
	codeBehind.ErrorDetails.Dump($"No Artist for ID {id}");

	//	pass
	id = 1;
	codeBehind.GetArtist(id);
	codeBehind.Artist.Dump($"Pass - Valid ID {id}");
	#endregion

	#region Get Artists (GetArtists)
	string name = string.Empty;
	// 	fail
	//	Rule:  name cannot be empty
	codeBehind.GetArtists(name);
	codeBehind.ErrorDetails.Dump($"Name must be valid. Name value is '{name}'");

	//	fail
	//	if no artists were found with the artist name provided
	name = "abc";
	codeBehind.GetArtists(name);
	codeBehind.ErrorDetails.Dump($"No Artists for name '{name}'");

	//	pass
	name = "abb";
	codeBehind.GetArtists(name);
	codeBehind.Artists.Dump($"Pass - Valid name {name}");
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
	private string errorMessage = string.Empty;
	#endregion

	// artist view returned by the service using GetArtist()
	public ArtistEditView Artist = new();
	
	public List<ArtistEditView> Artists = new();

	//	get artist methods
	public void GetArtist(int artistID)
	{
		//	clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = string.Empty;

		//	wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetArtist(artistID);
			if (result.IsSuccess)
			{
				Artist = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any excetion message for display
			errorMessage = ex.Message;
		}
	}

	//	get artist methods
	public void GetArtists(string name)
	{
		//	clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = string.Empty;

		//	wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetArtists(name);
			if (result.IsSuccess)
			{
				Artists = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any excetion message for display
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

	public Result<ArtistEditView> GetArtist(int artistID)
	{
		//  Create a Result continer that will hold eithe a 
		//  ArtistEditView object or success or any accumlated errors on failure
		var result = new Result<ArtistEditView>();

		#region Business Logic and Parameter Exceptions
		//	Business Rules
		// 	These are processing rules that need to be satisfied
		//		for valid data
		//		Rule:  artistID must be valid

		if (artistID == 0)
		{
			result.AddError(new Error("Missing Information", "Please provide a valid artist ID"));
			//	need to exit because we have nothing to search for
			return result;
		}
		#endregion
		//  artist that meet our criteria
		var artist = _hogWildContext.Artists
					.Where(a => a.ArtistId == artistID)
					.Select(a => new ArtistEditView
					{
						ArtistID = a.ArtistId,
						Name = a.Name
					}).FirstOrDefault();

		// if no artist were found with the artist id provied
		if (artist == null)
		{
			result.AddError(new Error("No Artist", $"No artist was found for with ID: {artistID}"));
			//	need to exit because we will not be able to add a null artist
			//	to the result if there are any errors
			return result;
		}
		//	return the result
		return result.WithValue(artist);
	}

	public Result<List<ArtistEditView>> GetArtists(string name)
	{
		//  Create a Result continer that will hold eithe a 
		//  ArtistEditView object or success or any accumlated errors on failure
		var result = new Result<List<ArtistEditView>>();

		#region Business Logic and Parameter Exceptions
		//	Business Rules
		// 	These are processing rules that need to be satisfied
		//		for valid data
		//		Rule:  artist name cannot be empty


		if (string.IsNullOrWhiteSpace(name))
		{
			result.AddError(new Error("Missing Information", "Please provide a valid artist name"));
			//	need to exit because we have nothing to search for
			return result;
		}
		#endregion

		//  artist that meet our criteria
		var artists = _hogWildContext.Artists
					.Where(a => a.Name.ToUpper().Contains(name.ToUpper()))
					.Select(a => new ArtistEditView
					{
						ArtistID = a.ArtistId,
						Name = a.Name
					}).ToList();

		// if no artist were found with the artist id provied
		if (artists.Count() == 0)
		{
			result.AddError(new Error("No Artists", $"No artists was found for with the name of: {name}"));
			//	need to exit because we will not be able to add a empty artist list
			//	to the result if there are any errors
			return result;
		}
		//	return the result
		return result.WithValue(artists);

	}


}
#endregion

// ———— PART 4: View Models → Service Library View Model ————
//	This region includes the view models used to 
//	represent and structure data for the UI.
#region View Models
public class ArtistEditView
{
	public int ArtistID { get; set; }
	public string Name { get; set; }
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