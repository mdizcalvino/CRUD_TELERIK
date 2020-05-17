using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Core
{
    public class Persona
    {
        [Required, StringLength(20)]
        public string Ejemplo { get; set; }

        public ICollection<Usuario> Usuario { get; set; }    }
}
