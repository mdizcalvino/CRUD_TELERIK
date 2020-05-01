using System;
using System.Collections.Generic;

namespace Modelos.Modelos
{
    public partial class GanttDependencies
    {
        public int Id { get; set; }
        public int PredecessorId { get; set; }
        public int SuccessorId { get; set; }
        public int Type { get; set; }
    }
}
