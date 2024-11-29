using HALLMiniKPay.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HALLMiniKPay.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult Execute(object model)
        {
            JObject jObj = JObject.Parse(JsonConvert.SerializeObject(model));
            if(jObj.ContainsKey("Response"))
            {
                BaseResponseModel baseResponseModel = JsonConvert.DeserializeObject<BaseResponseModel>(jObj["Response"]!.ToString()!)!;

                if (baseResponseModel.RespType == EnumRespType.ValidationError)
                    return BadRequest(model);

                if (baseResponseModel.RespType == EnumRespType.SystemError)
                    return StatusCode(500, model);

                return Ok(model);
            }
            return StatusCode(500, "invalid response model\nplease add 'BaseResponseModel' to your Model");
        }
    }
}
