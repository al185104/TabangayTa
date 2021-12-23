using System;
using System.Collections.Generic;
using System.Text;

namespace TabangayTa.Models.Resp
{
    public class Response
    {
        public string status { get; set; }
        public string id { get; set; }
    }

    public class ResourceResponseModel
    {
        public Response response { get; set; }
    }
}
