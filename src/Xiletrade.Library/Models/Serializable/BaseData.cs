﻿using System.Runtime.Serialization;

namespace Xiletrade.Library.Models.Serializable;

[DataContract]
public sealed class BaseData
{
    [DataMember(Name = "result")]
    public BaseResult[] Result { get; set; } = null;
}