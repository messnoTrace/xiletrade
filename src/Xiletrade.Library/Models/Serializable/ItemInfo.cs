﻿using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Xiletrade.Library.Models.Serializable;

[DataContract]
public sealed class ItemInfo
{
    [DataMember(Name = "currency")]
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [DataMember(Name = "amount")]
    [JsonPropertyName("amount")]
    public double Amount { get; set; } = 0;

    [DataMember(Name = "stock")]
    [JsonPropertyName("stock")]
    public int Stock { get; set; } = 0;

    [DataMember(Name = "whisper")]
    [JsonPropertyName("whisper")]
    public string Whisper { get; set; } = string.Empty;
}
