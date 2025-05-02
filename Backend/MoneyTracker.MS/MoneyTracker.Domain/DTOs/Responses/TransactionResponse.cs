namespace MoneyTracker.Domain.DTOs;

using Newtonsoft.Json;

public class TransactionResponse
{
    [JsonProperty("status", Order = 1)]
    public bool Status { get; set; }

    [JsonProperty("message", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }
}