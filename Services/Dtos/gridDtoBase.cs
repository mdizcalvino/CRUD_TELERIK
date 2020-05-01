using Kendo.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos
{
   

    public class gridDtoBase
    {
        
        public int Total { get; set; }
        public IEnumerable<AggregateResult> AggregateResults { get; set; }
        public object Errors { get; set; }
    }

    public class gridDto<T> : gridDtoBase
    {
        public IList<T> Data { get; set; }

    }
}
