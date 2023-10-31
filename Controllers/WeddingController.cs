using System.Diagnostics;
using System.Linq;
using System.Timers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Weddingplaner.Models;

namespace Weddingplaner.Controllers;

public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;
    private MyContext _context; 
    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;    
    }


[HttpGet]
[Route("weddings")]
[SessionCheck]

    public IActionResult Index()
    {
         List<Wedding>_wedding = _context.Weddings.Include(w => w.Asistents).ThenInclude(u => u.Usuario).ToList();
         List<WeddingViewModel>ListWeddingViewModel = new List<WeddingViewModel>();
         foreach ( Wedding _we in _wedding){
            WeddingViewModel _weddingviewmodel = new WeddingViewModel
        {
            WedderOne = _we.WedderOne,
            WedderTwo = _we.WedderTwo,
            WeddingDate = _we.WeddingDate,
            NumAsistentes = _we.Asistents.Count,
            CreatorId = _we.UsuarioId,
            ListaAsistentes = _we.Asistents,
            WeddingId = _we.WeddingId

        };
        ListWeddingViewModel.Add(_weddingviewmodel);
         }
         
        return View(ListWeddingViewModel);
    }
[HttpPost]
[Route("Delete/{WeddingId}")]
    public IActionResult Delete(int WeddingId){
  

    // Verificar si el registro existe
    var wedding = _context.Weddings.FirstOrDefault(w => w.WeddingId == WeddingId);


    if (wedding != null)
    {

        
        // Realizar la eliminación
        var weddingRegistrations = _context.weddingregistrations.Where(w => w.WeddingId == WeddingId).ToList();
        foreach (var registration in weddingRegistrations)
        {
            _context.weddingregistrations.Remove(registration);
        }

        _context.Remove(wedding);
        _context.SaveChanges(); // Guardar cambios en la base de datos

        return RedirectToAction("Index");
    }
    else
    {
         Console.WriteLine(" ***********************ES NULO WEDDING");

    
        return RedirectToAction("Index");
    }
}

[HttpPost]
[Route("Unregister/{WeddingId}")]
    public IActionResult Unregister(int WeddingId){
  
int id = (int) HttpContext.Session.GetInt32("UUID");
    // Verificar si el registro existe
    var weddingregistration = _context.weddingregistrations.FirstOrDefault(w => w.WeddingId == WeddingId && w.UsuarioId == id);


    if (weddingregistration != null)
    {

        
        // Realizar la eliminación
      

        _context.Remove(weddingregistration);
        _context.SaveChanges(); // Guardar cambios en la base de datos

        return RedirectToAction("Index");
    }
    else
    {
         Console.WriteLine(" ***********************ES NULO WEDDING");

    
        return RedirectToAction("Index");
    }
}

[HttpPost]
[Route("Register/{WeddingId}")]
    public IActionResult Register(int WeddingId){
int id = (int) HttpContext.Session.GetInt32("UUID");
    // Verificar si el registro existe
    Weddingregistration _weddingregistration = new Weddingregistration();
    _weddingregistration.UsuarioId = id;
    _weddingregistration.WeddingId = WeddingId;
    _context.Add(_weddingregistration);
    _context.SaveChanges();
    return RedirectToAction("Index");
}



[Route("Weddings/{id}")]
[SessionCheck]
    public IActionResult Viewwedding(int id){
        Console.WriteLine("el numero q se obtuvo fue "+ id);
        //Wedding ? _wedding = _context.Weddings.FirstOrDefault(u => u.WeddingId == id);
        List<Wedding> ListWedding = new List<Wedding>();
        var _wedding = _context.Weddings
    .Include(w => w.Asistents)
        .ThenInclude(a => a.Usuario) // Asegúrate de cargar la relación Usuario
    .FirstOrDefault(w => w.WeddingId == id);


            int asistentes = _wedding.Asistents.Count();
            // Obtén la lista de asistentes y su cantidad
            var listasistentes = _wedding.Asistents;

            foreach( var asistente in listasistentes){
                Console.WriteLine(asistente.Usuario.FirstName);
            }
        
        WeddingViewModel _weddingviewmodel = new WeddingViewModel
        {
            WedderOne = _wedding.WedderOne,
            WedderTwo = _wedding.WedderTwo,
            WeddingDate = _wedding.WeddingDate,
            NumAsistentes = asistentes,
            CreatorId = _wedding.UsuarioId,
            ListaAsistentes = listasistentes,
            WeddingId = _wedding.WeddingId

        };
        
             
                                        
        return View("Viewwedding",_weddingviewmodel);

    }

[Route("Weddings/new")]
[SessionCheck]
public IActionResult New(){
    return View("Newwedding");
}

public IActionResult Create(Wedding _wedding){
    Console.WriteLine(_wedding.Address);

    if(ModelState.IsValid){
        Console.WriteLine("es valido!!!!");
        int id = (int) HttpContext.Session.GetInt32("UUID");
         _wedding.UsuarioId =  id;
        _context.Add(_wedding);
        _context.SaveChanges();
        int weddingid = _context.Weddings.Where( u => u.UsuarioId == id).OrderByDescending(w => w.WeddingId).Select(w => w.WeddingId).FirstOrDefault();

        Console.WriteLine("El id es " + weddingid);
        return View("Viewwedding", weddingid);
    }else{
        return View("Newwedding", _wedding);
    }
    
    
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

[HttpGet]
[Route("Logout")]
    public IActionResult Logout(){
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

// Name this anything you want with the word "Attribute" at the end
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UUID");
        // Check to see if we got back null
        if(userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}





}
