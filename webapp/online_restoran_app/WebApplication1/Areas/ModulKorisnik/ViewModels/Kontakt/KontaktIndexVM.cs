using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Areas.ModulKorisnik.ViewModels.Kontakt
{
    public class KontaktIndexVM
    {
       public string Narucilac { get; set; }

       public string Email { get; set; }

        public string Poruka { get; set; }
    }
}
