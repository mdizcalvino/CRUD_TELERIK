using System;
using System.Collections.Generic;

namespace Modelos.Modelos
{
    public partial class Weather
    {
        public int Id { get; set; }
        public string Station { get; set; }
        public DateTime Date { get; set; }
        public decimal Tmax { get; set; }
        public decimal Tmin { get; set; }
        public decimal Wind { get; set; }
        public decimal? Gust { get; set; }
        public decimal Rain { get; set; }
        public decimal? Snow { get; set; }
        public string Events { get; set; }
    }
}
