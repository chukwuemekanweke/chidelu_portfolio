using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBackendBoilerPlate.Models;

namespace WebBackendBoilerPlate.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
   

    public class BaseController : ControllerBase
    {
        // GET: api/values

        [NonAction]
        public override OkObjectResult Ok(object value)
        {
            return base.Ok(new
            {
                Status = 1,
                Message = "",
                Data = value
            });
        }

        [NonAction]
        public OkObjectResult OkBase(object value)
        {
            return base.Ok(value);
        }

        [NonAction]

        public OkObjectResult Ok(object value, string message = "", ResponseStatus status = ResponseStatus.OK)
        {
            return base.Ok(new
            {
                Status = status,
                Message = message,
                Data = value
            });
        }

        [NonAction]

        public override BadRequestObjectResult BadRequest(object value)
        {
            return base.BadRequest(new
            {
                Status = 1,
                Message = "Successful Transaction",
                Data = value
            });
        }

        [NonAction]

        public BadRequestObjectResult BadRequest(object value, string message = "", ResponseStatus status = ResponseStatus.FATAL_ERROR)
        {
            return base.BadRequest(new
            {
                Status = status,
                Message = message,
                Data = value
            });
        }



    }


}