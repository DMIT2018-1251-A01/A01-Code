using BYSResults;
using LinqToWebSystem.DAL;
using LinqToWebSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToWebSystem.BLL
{
    public class ArtistService
    {
        //  linq to web context
        private readonly LinqToWebContext _linqToWebContext;

        //  Constructor for the ArtistService class.
        internal ArtistService(LinqToWebContext linqToWebContext)
        {
            //  Initialize the _hogWildContext fiekd with the provieded HogWildContext instance.
            _linqToWebContext = linqToWebContext;
        }

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
            var artist = _linqToWebContext.Artists
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
            var artists = _linqToWebContext.Artists
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
}
