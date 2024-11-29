using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HALLMiniKPay.Domain.Models
{
    public class BaseResponseModel
    {
        public string RespCode { get; set; }
        public string RespDesp { get; set; }
        public EnumRespType RespType { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsError { get { return !IsSuccess;  } }
        public static BaseResponseModel Success(string respCode, string respDesp)
        {
            return new BaseResponseModel()
            {
                IsSuccess = true,
                RespCode = respCode,
                RespDesp = respDesp,
                RespType = EnumRespType.Success
            };
        }
        public static BaseResponseModel ValidationError(string respCode, string respDesp)
        {
            return new BaseResponseModel()
            {
                IsSuccess = false,
                RespCode = respCode,
                RespDesp = respDesp,
                RespType = EnumRespType.ValidationError
            };
        }
        public static BaseResponseModel SystemError(string respCode, string respDesp)
        {
            return new BaseResponseModel()
            {
                IsSuccess = false,
                RespCode = respCode,
                RespDesp = respDesp,
                RespType = EnumRespType.SystemError
            };
        }
    }
    public enum EnumRespType
    {
        None,
        Success,
        ValidationError,
        SystemError 
    }
}
