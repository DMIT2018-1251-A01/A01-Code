//#nullable disable
using HogWildSystem.DAL;
using HogWildSystem.ViewModels;

namespace HogWildSystem.BLL
{
    public class WorkingVersionsService
    {
        #region Fields
        //  hog wild context
        private readonly HogWildContext _hogWildContext;
        #endregion

        //  constructor for the WorkingVersionsService class
        internal WorkingVersionsService(HogWildContext hogWildContext)
        {
            //  Initialize the _hogWildContext field with the provided HogWildContext instance.
            _hogWildContext = hogWildContext;
        }

        //  This method retrieves the working version of the resource
        public WorkingVersionsView? GetWorkingVersion()
        {
            return _hogWildContext.WorkingVersions
                .Select(w => new WorkingVersionsView
                    {
                        VersionId = w.VersionId,
                        Major = w.Major,
                        Minor = w.Minor,
                        Build = w.Build,
                        Revision = w.Revision,
                        AsOfDate = w.AsOfDate,
                        Comments = w.Comments
                    }
                ).FirstOrDefault();// ?? new WorkingVersionsView();
        }
    }
}
