using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.HttpServices
{
    public interface IGenericApiController<T> where T : class
    {
        [HttpGet]
        Task<ActionResult<gridDto<T>>> GetApi([DataSourceRequest]DataSourceRequest request);
        [HttpPost]
        Task<ActionResult<T>> PostApi(T entityDto);
        [HttpPut("{id}")]
        Task<ActionResult<T>> PutApi(T entityDto, string id);
        [HttpDelete("{id}")]
        Task<ActionResult<T>> DeleteApi(string id);
    }

    public class GenericApiController : IGenericApiController<OrderDto>
    {
        public Task<ActionResult<OrderDto>> DeleteApi(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<gridDto<OrderDto>>> GetApi([DataSourceRequest] DataSourceRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<OrderDto>> PostApi(OrderDto entityDto)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<OrderDto>> PutApi(OrderDto entityDto, string id)
        {
            throw new NotImplementedException();
        }
    }
}
