using System;
using System.Collections.Generic;

namespace Modelos.Modelos
{
    public partial class GanttResourceAssignments
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int ResourceId { get; set; }
        public decimal Units { get; set; }
    }
}
