using System;
using Newtonsoft.Json;
using SQLite;

namespace Steamboat.Mobile.Models
{
    public class EntityBase
    {
        [JsonProperty("id")]
        [PrimaryKey]
        public Guid Id { get; set; }
    }
}
