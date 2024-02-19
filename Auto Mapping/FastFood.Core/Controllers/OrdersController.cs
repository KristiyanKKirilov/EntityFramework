namespace FastFood.Core.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = _context.Items.Select(x => x.Id).ToList(),
                Employees = _context.Employees.Select(x => x.Id).ToList(),
            };

            return View(viewOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderInputModel model)
        {
            if(!ModelState.IsValid)
            {
                RedirectToAction("Error", "Home");
            }

            var order = new Order
            {                
                Customer = model.Customer,
                EmployeeId = model.EmployeeId,
                DateTime = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("All");
        }

        public async Task<IActionResult> All()
        {
            var orders = await _context.Orders               
                .Select(o => new OrderAllViewModel
                {
                    OrderId = o.Id,
                    Customer = o.Customer,
                    Employee = o.Employee.Name,
                    DateTime = o.DateTime.ToString("dd/MM/yyyy hh:mm")
                })
                .ToListAsync();

            return View(orders);
        }
    }
}
