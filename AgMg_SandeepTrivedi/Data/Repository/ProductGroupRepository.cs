using AgMg_SandeepTrivedi.Models;
using System.Collections.Generic;
using System.Linq;

namespace AgMg_SandeepTrivedi.Data.Repository
{
    public class ProductGroupRepository : Repository<ProductGroup>
    {
        private readonly ApplicationDbContext context;
        public ProductGroupRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public IEnumerable<ProductGroup> GetProductGroupList(bool onlyActive)
        {
            List<ProductGroup> prodGroups = onlyActive == true ?
                                           context.ProductGroups.Where(g => g.Active == true).ToList()
                                           : context.ProductGroups.ToList();
            return prodGroups;
        }
    }
}
