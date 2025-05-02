namespace MoneyTracker.Domain.DTOs;

using Newtonsoft.Json;

public class PaginationResponse<T> : ValueResponse<T>
{
    [JsonProperty("totalRecords", Order = 4, NullValueHandling = NullValueHandling.Ignore)]
    public int TotalRecords { get; set; }
}