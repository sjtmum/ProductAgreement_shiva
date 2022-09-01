using AgMg_SandeepTrivedi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgMg_SandeepTrivedi.Data
{
    public class DBCommoncs
    {
        private readonly ApplicationDbContext _context;

        public DBCommoncs(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductGroup>> GetProductGroupList(bool onlyActive)
        {
            List<ProductGroup> prodGroups = await Task.Run(() => onlyActive == true ?
                                           _context.ProductGroups.Where(g => g.Active == true).ToList()
                                           : _context.ProductGroups.ToList());
            return prodGroups;
            //ProductController.Create
                //DBCommoncs dBCommoncs = new DBCommoncs(_context);
                //ViewBag.prodGroupList = new SelectList(dBCommoncs.GetProductGroupList(true), "Id", "GroupDescription");
            // Product.Create.cshtml
            // @Html.DropDownList("ProductGroupId", (ViewBag.prodGroupList as SelectList), new { @class = "col-3 form-control" })

        }
        public async Task<IEnumerable<Product>> GetProductList(int prodGroupId, bool onlyActive)
        {
            List<Product> prodList = await Task.Run(() => onlyActive == true ?
                                           _context.Products.Where(g => g.ProductGroupId == prodGroupId && g.Active == true).ToList()
                                           : _context.Products.Where(g => g.ProductGroupId == prodGroupId).ToList());
            return prodList;
            //ProductController.Create
            //DBCommoncs dBCommoncs = new DBCommoncs(_context);
            //ViewBag.prodGroupList = new SelectList(dBCommoncs.GetProductGroupList(true), "Id", "GroupDescription");
            // Product.Create.cshtml
            // @Html.DropDownList("ProductGroupId", (ViewBag.prodGroupList as SelectList), new { @class = "col-3 form-control" })

        }
        public async Task<IEnumerable<Product>> GetProducts(bool onlyActive = false)
        {
             var prods =  await Task.Run(() =>
            (from pd in (onlyActive == true ? _context.Products.Where (g=>g.Active==true) : _context.Products)
                                  join pg in _context.ProductGroups on new { c = pd.ProductGroupId } equals new { c = pg.Id }
                                  select new Product()
                                  {
                                      Id = pd.Id,
                                      ProductGroupId = pg.Id,
                                      ProductDescription = pd.ProductDescription,
                                      ProductGroupDesc = pg.GroupDescription,
                                      Price = pd.Price,
                                      Active = pd.Active,
                                      ProductNumber = pd.ProductNumber,
                                      UserId = pd.UserId
                                  }));
            
            return prods.OrderBy(x => x.Id);
        }
        public async Task<IEnumerable<Agreement>> GetAgreements(bool onlyActive = false)
        {
            var agreements = await Task.Run(() =>
          (from ag in (onlyActive == true ? _context.Agreements.Where(g => g.Active == true) : _context.Agreements)
           join pd in _context.Products  on new { c = ag.ProductId } equals new { c = pd.Id }
           join pg in _context.ProductGroups on new { c = ag.ProductGroupId } equals new { c = pg.Id }
           select new Agreement()
           {
               Id = ag.Id,
               ProductGroupId = pg.Id,
               ProductGroupDesc = pg.GroupDescription,
               ProductId = pd.Id,
               ProductDesc = pd.ProductDescription,
               ProductPrice = ag.ProductPrice,
               NewPrice = ag.NewPrice,
               EffectiveDate = ag.EffectiveDate,
               ExpirationDate = ag.ExpirationDate,
               Active = ag.Active,
               UserId = pd.UserId,
           }));
            return agreements; //.OrderBy(x => x.Id);
        }
    }
}
