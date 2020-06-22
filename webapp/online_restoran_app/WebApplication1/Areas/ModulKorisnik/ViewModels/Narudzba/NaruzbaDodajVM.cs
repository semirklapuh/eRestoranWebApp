using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Areas.ModulKorisnik.ViewModels.Narudzba
{
    public class NaruzbaDodajVM
    {
        public int NarucilacId { get; set; }
        public string ImePrezime { get; set; }
        public string Adresa { get; set; }
        public string DatumNarudzbe { get; set; }
        public string Telefon { get; set; }
        public int SatusDostaveId { get; set; }
        public string StatusDostave { get; set; }

        public int NarudzbaId { get; set; }
    }
}
