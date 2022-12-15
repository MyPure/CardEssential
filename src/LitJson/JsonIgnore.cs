using System;

namespace LitJson;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class JsonIgnore : Attribute
{

}
