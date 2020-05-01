using AutoMapper;
using Modelos.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos
{

    [AutoMap(typeof(OrderDetails), ReverseMap = true)]
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }

        //public virtual Orders Order { get; set; }
        public virtual ProductCboDto Product { get; set; }
    }
}
