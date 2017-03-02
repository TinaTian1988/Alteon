using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Alteon.Core.Enum;

namespace Alteon.Web.Framework.Mvc
{
    public sealed class AlteonJsonResult : ActionResult
    {
        public ResultStatus status { get; set; }

        public object data { get; set; }

        public string message { get; set; }

        public int draw { get; set; }

        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }

        public string search { get; set; }
        public AlteonJsonResult(ResultStatus status, string message = "", object data = null)
        {
            this.status = status;
            this.message = message;
            this.data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = "application/json";

            var jsonObj = new JsonObject
            {
                Status = this.status,
                Message = this.message,
                Data = this.data
            };

            try
            {

                response.Write(Core.Common.JsonConverter.SerializeObject(jsonObj));
            }
            catch
            {
                response.Write(Core.Common.JsonConverter.SerializeObject(new JsonObject
                {
                    Status = ResultStatus.Fail,
                    Message = "-9999"
                }));
            }
        }

        /// <summary>
        /// Json Serializer Template
        /// </summary>
        internal class JsonObject
        {
            public ResultStatus Status { get; set; }

            public object Data { get; set; }

            public string Message { get; set; }
        }
    }
}
