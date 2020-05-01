using AutoMapper;
using Modelos.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Dtos
{
    [AutoMap(typeof(Products), ReverseMap = true)]
    public class ProductDto
    {
       

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        //public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
       
        public bool Discontinued { get; set; }

        [UIHint("ProductsCategoriesCombo")]
        public virtual CategoryDto Category { get; set; }
        public virtual Suppliers Supplier { get; set; }
        
    }

    [AutoMap(typeof(Products), ReverseMap = true)]
    public class ProductCboDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

    }
}
