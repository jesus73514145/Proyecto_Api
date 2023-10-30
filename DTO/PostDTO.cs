using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration.UserSecrets;
namespace Proyecto_Api.DTO
{
    public class PostDTO
    {
        [JsonProperty("userId")]
        public int? userId { get; set; }

        [JsonProperty("id")]
        public int? id { get; set; }

        [JsonProperty("title")]
        public string? title { get; set; }

        [JsonProperty("body")]
        public string? body { get; set; }

    }
}