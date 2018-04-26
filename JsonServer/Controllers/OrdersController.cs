


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using JsonServer.Models;
using JsonServer.Services;
using JsonServer.Repositories;
using JsonServer.Extensions;


namespace JsonServer
{
    public class OrdersController : Controller
    {

        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<Order>, Repository<Order>>();
        //container.RegisterType<IOrderService, OrderService>();

        //private Appdata db = new Appdata();
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService, IUnitOfWorkAsync unitOfWork)
        {
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }

        // GET: Orders/OrderIndex
        public ActionResult OrderIndex()
        {

            var order = _orderService.Queryable().AsQueryable();
            return View(order);
        }

        // Get :Orders/OrderPageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int pagenum = offset / limit + 1;
            var orderE = _orderService.Query(new OrderQuery().WithAnySearch(search)).OrderBy(n => n.OrderBy(sort, order)).SelectPage(pagenum, limit, out int totalCount);
            var rows = orderE.Select(n => new { n.Id,  n.Orderkey,  n.Supplier, n.Qty,  n.Unitprice,  n.Amount }).ToList();
            var pagelist = new { total = totalCount,  rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }


        // GET: Orders/OrderDetails/5
        public ActionResult OrderDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _orderService.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


        // GET: Orders/OrderCreate
        public ActionResult OrderCreate()
        {
            Order order = new Order();
            //set default value
            return View(order);
        }

        // POST: Orders/OrderCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult OrderCreate([Bind(Include = "Id,Orderkey,Supplier,Qty,Unitprice,Amount")] Order order)
        {
            if (ModelState.IsValid)
            {
                _orderService.Insert(order);
                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Order record");
                return RedirectToAction("OrderIndex");
            }

            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(order);
        }

        // GET: Orders/OrderEdit/5
        public ActionResult OrderEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _orderService.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: OrdersOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult OrderEdit([Bind(Include = "Id,Orderkey,Supplier,Qty,Unitprice,Amount")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.ObjectState = ObjectState.Modified;
                _orderService.Update(order);

                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Order record");
                return RedirectToAction("OrderIndex");
            }
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(order);
        }

        // GET: Orders/OrderDelete/5
        public ActionResult OrderDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _orderService.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/OrderDelete/5
        [HttpPost, ActionName("OrderDelete")]
        //[ValidateAntiForgeryToken]
        public ActionResult OrderDeleteConfirmed(string id)
        {
            Order order = _orderService.Find(id);
            _orderService.Delete(order);
            _unitOfWork.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Order record");
            return RedirectToAction("OrderIndex");
        }






        private void DisplaySuccessMessage(string msgText)
        {
            TempData["SuccessMessage"] = msgText;
        }

        private void DisplayErrorMessage()
        {
            TempData["ErrorMessage"] = "Save changes was unsuccessful.";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
