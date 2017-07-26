using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationApplication.Data
{
    public enum Status
    {
        Pending,
        Approved,
        Rejected
    }

    public class ApplicationRespository
    {
        private string _connectionString;

        public ApplicationRespository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddApplication(Application application)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                context.Applications.InsertOnSubmit(application);
                context.SubmitChanges();
            }
        }

        public IEnumerable<Application> GetUserApplications(int userId)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Application>(a => a.Category);
                context.LoadOptions = loadOptions;
                return context.Applications.Where(u => u.UserId == userId).ToList();
            }
        }

        public IEnumerable<Application> GetApplications(int? categoryId)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Application>(a => a.Category);
                loadOptions.LoadWith<Application>(a => a.User);
                context.LoadOptions = loadOptions;
                return context.Applications.Where(a => !categoryId.HasValue || a.CategoryId == categoryId.Value);
            }
        }

        public IEnumerable<Application> GetPendingApplications(int? categoryId)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Application>(a => a.Category);
                loadOptions.LoadWith<Application>(a => a.User);
                context.LoadOptions = loadOptions;
                //if (categoryId == null)
                //{
                //    return context.Applications.Where(a => a.IsApproved == null).ToList();
                //}
                //return context.Applications.Where(a => a.IsApproved == null && a.CategoryId == categoryId.Value);
                return context.Applications.Where(a => a.IsApproved == null && (categoryId.HasValue ? a.CategoryId == categoryId : true)).ToList();
            }
        }


        public void UpdateApplicationStatus(int applicationId, bool isApproved)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Applications SET IsApproved = {0} WHERE Id = {1}", isApproved,
                    applicationId);
            }
        }

    }
}
