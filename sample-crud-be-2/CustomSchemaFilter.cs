using Microsoft.OpenApi.Models;
using sample_crud_be_2.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class CustomSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {



        if (context.Type == typeof(Author) || context.Type == typeof(Book) || context.Type == typeof(Category))
        {
            schema.Type = "object";
        }
    }

}
