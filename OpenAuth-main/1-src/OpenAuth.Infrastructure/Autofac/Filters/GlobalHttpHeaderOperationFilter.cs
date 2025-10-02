using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Filters
{
    public class GlobalHttpHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            var actionAttrs = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var isAnony = actionAttrs.Any(a => a.GetType() == typeof(AllowAnonymousAttribute));

            operation.Parameters.Add(new OpenApiParameter
            {
                Required = false,
                Name = CommonConstant.X_Access_Token,
                In = ParameterLocation.Header,
                Description = "用户token"
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Required = false,
                Name = CommonConstant.X_WebId,
                In = ParameterLocation.Header,
                Description = "用户WebId"
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Required = false,
                Name = CommonConstant.X_Tenant_Id,
                In = ParameterLocation.Header,
                Description = "租户id"
            });
        }
    }
}
