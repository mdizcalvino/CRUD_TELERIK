using AutoMapper;
using Modelos.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos
{
    [AutoMap(typeof(Categories), ReverseMap = true)]
    public class CategoryDto
    {
       

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
       
    }
}
