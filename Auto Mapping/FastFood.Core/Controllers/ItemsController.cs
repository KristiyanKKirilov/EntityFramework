﻿namespace FastFood.Core.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ViewModels.Items;

    public class ItemsController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;

        public ItemsController(FastFoodContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Create()
        {
            //Manual mapping
            //var categories = await _context.Categories
            //    .Select(c => new CreateItemViewModel
            //    {
            //        CategoryId = c.Id,
            //        Name = c.Name
            //    })
            //    .ToArrayAsync();
            var categories = await _context.Categories
                .ProjectTo<CreateItemViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItemInputModel model)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Error", "Home");
            }
            var newItem = new Item
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                Price = model.Price
            };

            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("All");
        }

        public async Task<IActionResult> All()
        {
            var items = await _context.Items
                .ProjectTo<ItemsAllViewModels>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return View(items);
        }
    }
}
