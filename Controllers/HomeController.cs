using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using crudelicious.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace crudelicious.Controllers;

public class HomeController : Controller
{
    
    private readonly ILogger<HomeController> _logger;
    private MyContext _context; 

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        // When our HomeController is instantiated, it will fill in _context with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        _context = context;  
    }

    public IActionResult Index()
    {
        List<Dish> AllDishes = _context.Dishes.ToList();
        return View(AllDishes);    
        
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Route("AgregarComida")]
    public IActionResult AgregarComida(){
        return View("Agregarcomida");
    }    
    [HttpPost("dishes/create")]
    public IActionResult Crearcomida(Dish _dish){

        if(ModelState.IsValid){
            Console.WriteLine(_dish);
            _context.Add(_dish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }else{
        return View("AgregarComida");
        }

    }
[Route("/dishes/{id}/edit")]
    public IActionResult Edit(int id){
    
        Dish ? _dish  = _context.Dishes.FirstOrDefault(d => d.DishId == id);
        return View(_dish);
    }

    // {MonsterId} comes from asp-route-MonsterId
[HttpPost("dishes/{DishId}/update")]
// We are passing down both the Monster from our form and the ID from our route parameter
public IActionResult Updatedish(Dish newDish, int DishId)
{ 
    Dish? OldDish = _context.Dishes.FirstOrDefault(i => i.DishId == DishId);
    if(ModelState.IsValid)
    {
        // 4. Overwrite the old version with the new version
    	// Yes, this has to be done one attribute at a time
    	OldDish.Name = newDish.Name;
        OldDish.Chef = newDish.Chef;
        OldDish.Tastiness = newDish.Tastiness;
        OldDish.Calories = newDish.Calories;
        OldDish.Description = newDish.Description;
    	// You updated it, so update the UpdatedAt field!
        OldDish.UpdatedAt = DateTime.Now;
    	// 5. Save your changes
        _context.SaveChanges();
    	// 6. Redirect to an appropriate page
        return RedirectToAction("Index");
    } else {
    	// 3.5. If it does not pass validations, show error messages
    	// Be sure to pass the form back in so you don't lose your changes
        // It should be the old version so we can keep the ID
        return RedirectToAction("Show", OldDish);
    }
}

[Route("dishes/{id}/show")]
    public IActionResult Show(int id){
        Dish ? _dish  = _context.Dishes.FirstOrDefault(d => d.DishId == id);
        return View(_dish);
    }


    [Route("dishes/{id}/destroy")]
    public IActionResult Destroy(int id){
        Dish ? _dish  = _context.Dishes.FirstOrDefault(d => d.DishId == id);
        _context.Remove(_dish);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

}












