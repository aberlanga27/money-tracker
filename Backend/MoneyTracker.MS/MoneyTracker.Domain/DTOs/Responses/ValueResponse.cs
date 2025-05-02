namespace MoneyTracker.Domain.DTOs;

using Newtonsoft.Json;

public class ValueResponse<T> : TransactionResponse
{
    [JsonProperty("response", Order = 3, NullValueHandling = NullValueHandling.Ignore)]
    public T? Response { get; set; }
}