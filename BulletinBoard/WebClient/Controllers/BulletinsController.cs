using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers;

public class BulletinsController : Controller
{
    public BulletinsController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}