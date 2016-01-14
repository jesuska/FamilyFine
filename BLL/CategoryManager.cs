using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CategoryManager
    {
        private FinanceDB db = new FinanceDB();

        public List<Category> GetAllCategories()
        {
            List<Category> extendedCategories = db.Categories.OrderBy(t => t.Name).ToList();
            extendedCategories.Insert(0, new Category { Id = 0, Name = "Besorolatlan" });
            return extendedCategories;
        }
    }
}
