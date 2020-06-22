using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.EF;

namespace WebApplication1.Controllers
{
    public class AjaxController : Controller
    {
        private MyContext _context;

        public AjaxController(MyContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
           

            return PartialView();
        }
    }
}