using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.UI.Web.Internet.Settings;

namespace Northwind.Store.UI.Web.Internet.Controllers
{
    public class CartController : Controller
    {
        private readonly NWContext _db;
        private readonly SessionSettings _ss;
        private readonly RequestSettings _rs;

        public CartController(NWContext db, SessionSettings ss)
        {
            _db = db;
            _ss = ss;
            _rs = new RequestSettings(this);
        }

        public IActionResult Index()
        {
            var productId = TempData[nameof(Product.ProductId)];
            var productName = TempData[nameof(Product.ProductName)];
            //TempData.Keep(nameof(Product.ProductName));
            //var productName = TempData.Peek(nameof(Product.ProductName));
            //var productAdded = _rs.ProductAdded;

            ViewBag.productAdded = _rs.ProductAdded;
            //TempData.Keep();

            return View(_ss.Cart);
        }

        public ActionResult Add(int? id)
        {

            if (id.HasValue)
            {
                #region Session
                var p = _db.Products.Find(id);
                var cart = _ss.Cart;

                cart.Items.Add(p);

                _ss.Cart = cart;
                #endregion

                #region TempData
                TempData[nameof(Product.ProductId)] = p.ProductId;
                TempData[nameof(Product.ProductName)] = p.ProductName;

                _rs.ProductAdded = p;
                #endregion
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = _ss.Cart;
            List<int> itemToRemove = new();
            Order order = new Order();
            foreach (var item in cart.Items)
            {
                order.OrderDetails.Add(new OrderDetail() {ProductId = item.ProductId});
                itemToRemove.Add(item.ProductId);
            }
            
            _db.Add(order);
             await _db.SaveChangesAsync();
             foreach (var id in itemToRemove)
             {
                cart.Items.RemoveAll(p => p.ProductId==id);
             }


            return RedirectToAction("Index");
        }
    }
}
