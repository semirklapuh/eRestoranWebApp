﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Zaposlenik
    {
        [Key]
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adresa { get; set; }
        public int RadnoMjestoId { get; set; }
        [ForeignKey("RadnoMjestoId")]
        public RadnoMjesto RadnoMjesto { get; set; }

        public int GradId { get; set; }
        [ForeignKey("GradId")]
        public Grad Grad { get; set; }


        public int KorisnickiNalogId { get; set; }
        [ForeignKey("KorisnickiNalogId")]
        public KorisnickiNalog KorisnickiNalog { get; set; }



    }
}
