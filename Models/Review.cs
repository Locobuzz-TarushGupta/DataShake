using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataShakeApiLocobuzz.Models
{

    public class Response
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? date { get; set; }
        public string? comment { get; set; }
    }

    public class Review
    {
        public long id { get; set; }
        public string? name { get; set; }
        public string? date { get; set; }
        public float? rating_value { get; set; }
        public string? review_text { get; set; }
        public string? url { get; set; }
        public string? profile_picture { get; set; }
        public string? location { get; set; }
        public string? review_title { get; set; }
        public bool? verified_order { get; set; }
        public string? language_code { get; set; }
        public string? reviewer_title { get; set; }
        public string? unique_id { get; set; }
        public string? meta_data { get; set; }
        public Response? response { get; set; }
    }

    public class Root
    {
        public bool success { get; set; }
        public int status { get; set; }
        public int job_id { get; set; }
        public string? source_url { get; set; }
        public string? source_name { get; set; }
        public object? place_id { get; set; }
        public object? external_identifier { get; set; }
        public string? meta_data { get; set; }
        public string? unique_id { get; set; }
        public int? review_count { get; set; }
        public float? average_rating { get; set; }
        public string? last_crawl { get; set; }
        public string? crawl_status { get; set; }
        public float? percentage_complete { get; set; }
        public int? result_count { get; set; }
        public int? credits_used { get; set; }
        public object? from_date { get; set; }
        public object? blocks { get; set; }
        public List<Review>? reviews { get; set; }
    }

    public class ProfileData
    {
        public bool success { get; set; }
        public int status { get; set; }
        public int job_id { get; set; }
        public string? source_url { get; set; }
        public string? source_name { get; set; }
        public object? place_id { get; set; }
        public object? external_identifier { get; set; }
        public object? meta_data { get; set; }
        public object? unique_id { get; set; }
        public int? review_count { get; set; }
        public double? average_rating { get; set; }
        public string? last_crawl { get; set; }
        public string? crawl_status { get; set; }
        public float? percentage_complete { get; set; }
        public int? result_count { get; set; }
        public int? credits_used { get; set; }
        public object? from_date { get; set; }
        public object? blocks { get; set; }
    }

}
