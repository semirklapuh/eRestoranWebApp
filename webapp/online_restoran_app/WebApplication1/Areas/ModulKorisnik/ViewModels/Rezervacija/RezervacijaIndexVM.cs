using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Areas.ModulKorisnik.ViewModels.Rezervacija
{
    public class RezervacijaIndexVM
    {
        public List<Row> Rows { get; set; }
        public class Row
        {
            public int RezervacijaId { get; set; }
            public DateTime DatumEvidencije { get; set; }
            public DateTime DatumRezervacije { get; set; }
            public string Naziv { get; set; }

            public string ImePrezime { get; set; }

            public int BrojMjesta { get; set; }
            public int? KoriscnikiNalogId { get; internal set; }
        }
    }
}
