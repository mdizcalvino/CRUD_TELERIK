using AutoMapper;
using Modelos.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Services.Dtos
{
    [AutoMap(typeof(Orders), ReverseMap = true)]
    public class OrderDto
    {

        public int OrderId { get; set; }
        public string CustomerId { get; set; }

        //[UIHint("GridForeignKey")]
        public int? EmployeeId { get; set; }
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        public DateTime? RequiredDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true )]
        public DateTime? ShippedDate { get; set; }

        [DataType("Integer")]
        public int? ShipVia { get; set; }

        [DataType("Number")]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public decimal? Freight { get; set; }
        //public string ShipName { get; set; }
        //public string ShipAddress { get; set; }
        //public string ShipCity { get; set; }
        //public string ShipRegion { get; set; }
        //public string ShipPostalCode { get; set; }
        //public string ShipCountry { get; set; }

        [IgnoreMap]
        //[NotMapped]
        public virtual CustomersCboDto Customer { get; set; }
        [IgnoreMap]
        //[UIHint("GridForeignKey")]
        public virtual EmployeCboDto Employee { get; set; }
        //public virtual Shippers ShipViaNavigation { get; set; }
        //public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
