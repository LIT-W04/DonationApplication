using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationApplication.Data
{
    public class CategoryRepository
    {
        private string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Category> GetCategories(bool includeApplications)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                if (includeApplications)
                {
                    var loadOptions = new DataLoadOptions();
                    loadOptions.LoadWith<Category>(c => c.Applications);
                    context.LoadOptions = loadOptions;
                }
                return context.Categories.ToList();
            }
        }

        public void Add(Category category)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                context.Categories.InsertOnSubmit(category);
                context.SubmitChanges();
            }
        }

        public void Update(Category category)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                context.Categories.Attach(category);
                context.Refresh(RefreshMode.KeepCurrentValues, category);
                context.SubmitChanges();
            }
        }

        public void Delete(int categoryId)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                context.ExecuteCommand("DELETE FROM Categories WHERE Id = {0}", categoryId);
            }
        }
    }
}
