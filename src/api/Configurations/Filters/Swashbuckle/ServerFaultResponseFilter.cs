using System.Collections.Generic;
using API.Domains.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Configurations.Filters.Swashbuckle
{
    public class ServerFaultResponseFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var schema = new OpenApiSchema() 
            {
                Reference = new OpenApiReference() 
                {
                    Type = ReferenceType.Schema,
                    Id = "ServerFault"
                }
            };

            var content = new Dictionary<string, OpenApiMediaType>
            {
                { 
                    "text/plain", new OpenApiMediaType() 
                    { 
                        Schema = schema
                    }
                },
                { 
                    "application/json", new OpenApiMediaType() 
                    { 
                        Schema = schema
                    }
                },
                { 
                    "text/json", new OpenApiMediaType() 
                    { 
                        Schema = schema
                    }
                },
            };

            operation.Responses.Add("500", new OpenApiResponse 
            { 
                Description = "Our server failed to fulfill an apparently valid request",
                Content = content
            });

            // operation.Responses.Add("502", new OpenApiResponse 
            // { 
            //     Description = "Our server was acting as a gateway or proxy and received an invalid response from the upstream server",
            //     Content = content
            // });

            // operation.Responses.Add("504", new OpenApiResponse 
            // { 
            //     Description = "Our server was acting as a gateway or proxy and did not receive a timely response from the upstream server",
            //     Content = content
            // });
        }
    }
}