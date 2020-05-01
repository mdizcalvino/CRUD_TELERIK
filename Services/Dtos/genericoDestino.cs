using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos
{
    public static class genericoDestino<T>
    {
        public static IList<T> data { get; set; }
    }
}
