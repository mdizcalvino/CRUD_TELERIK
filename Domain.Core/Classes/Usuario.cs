using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core
{
   public  class Usuario
    {
        public int MyProperty { get; set; }
        public ICollection<Persona> Persona { get; set; }
    }
}
  