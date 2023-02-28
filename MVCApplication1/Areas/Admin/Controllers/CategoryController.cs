﻿using Microsoft.AspNetCore.Mvc;
using MVCApplication1.DataAccess.Data;
using MVCApplication1.DataAccess.Repository.IRepository;
using MVCApplication1.Models;

namespace MVCApplication1.Controllers;
[Area("Admin")]

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;


    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
        return View(objCategoryList);
    }

    //GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match Name");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match Name");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        //var category = _unitOfWork.Categories.Find(id);
        var category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        _unitOfWork.Category.Remove(category);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";

        return RedirectToAction("Index");
    }

}
