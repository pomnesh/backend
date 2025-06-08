using Microsoft.AspNetCore.Mvc.Filters;

namespace Pomnesh.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SkipJwtMiddlewareAttribute : Attribute, IFilterMetadata
{
} 