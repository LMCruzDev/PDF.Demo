using System;
using System.Threading.Tasks;
using PDF.Demo.Api.Models;

namespace PDF.Demo.Api.Business.Abstractions
{
    public interface IFormsApiClient
    {
        Task<FormResult> GetById(Guid id);
    }
}
