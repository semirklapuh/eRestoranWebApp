using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Areas.ModulKorisnik.ViewModels.Narudzba
{
    public class NarudzbaDodajStavkeVM
    {
        public List<SelectListItem> Jela { get; set; }

        public int JelaId { get; set; }


        public int Kolicina { get; set; }
        public int NarudzbaId { get; set; }
    }
}
