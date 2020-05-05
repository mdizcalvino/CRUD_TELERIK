using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.HttpServices
{
    public interface IGenericController<T> where T : class
    {       
        Task<ActionResult<gridDto<T>>> Get([DataSourceRequest]DataSourceRequest request);
        Task<ActionResult> Post([DataSourceRequest]DataSourceRequest request, T entityDto);
        Task<ActionResult> Put([DataSourceRequest] DataSourceRequest request, T entityDto, string id);
        Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, T entityDto, string id);
    }

    //public class GenericController<T> : IGenericController<T> where T : class
    //{
    //    public int Controlador { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    //    public Task<ActionResult<gridDto<T>>> Get([DataSourceRequest] DataSourceRequest request)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<ActionResult> Post([DataSourceRequest] DataSourceRequest request, T tDto)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<ActionResult> Put([DataSourceRequest] DataSourceRequest request, T tDto, int id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, T tDto, int id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
