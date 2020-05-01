using AutoMapper;
using Modelos.Modelos;

namespace Services.Dtos
{
    [AutoMap(typeof(Customers), ReverseMap = true )]
   
    public class CustomersDto
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
       


    }

    [AutoMap(typeof(Customers), ReverseMap = true)]
    public class CustomersDtoUpdate
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }



    }

    [AutoMap(typeof(Customers), ReverseMap = true)]
    public class CustomersCboDto
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
    
    }
}
