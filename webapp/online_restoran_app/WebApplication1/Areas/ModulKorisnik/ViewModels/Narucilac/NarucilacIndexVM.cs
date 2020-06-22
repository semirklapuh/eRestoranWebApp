using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Areas.ModulKorisnik.ViewModels.Narucilac
{
    public class NarucilacIndexVM
    {
        public int NarucilacId { get; internal set; }
        public string ImePrezime { get; internal set; }
        public string KorisnickoIme { get; internal set; }
        public string Email { get; internal set; }
        public string Telefon { get; internal set; }
        public string Adresa { get; internal set; }
        public string Lozinka { get; internal set; }
        public int KorisnickiNalogId { get; internal set; }
        public string GradNaziv { get; internal set; }
        public int GradId { get; internal set; }
    }
}
