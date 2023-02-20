using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataShakeApiLocobuzz.Models
{
    internal class LocobuzzResponse
    {
        public LocobuzzResponse(bool _success, string _message, object _data)
        {
            Success = _success; Message = _message; Data = _data;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Data);

        }
    }

    class BulkUrl
    {
        public string Url { get; set; }
    }

    class BulkUrlResponse
    {
        public string url { get; set; }
        public bool success { get; set; }
        public int job_id { get; set; }
        public int status { get; set; }
        public string message { get; set; }
    }

    class wrapperUrl
    {
        public string url { get; set; }
        public string brandId { get; set; }
    }

    class wrapperUrls
    {
        public List<string> urls { get; set; }
    }

    class Urls
    {
        public List<wrapperUrls> brandUrls { get; set; }
    }
}
