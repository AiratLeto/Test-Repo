using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cars.Api.Web.Swagger
{
	/// <summary>
	/// Конфиг сваггера
	/// </summary>
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		/// <inheritdoc/>
		public void Configure(SwaggerGenOptions options)
		{
			options.DescribeAllParametersInCamelCase();
			options.EnableAnnotations();

			options.IncludeXmlCommentsIfExists(typeof(Startup).Assembly);
			options.IncludeXmlCommentsIfExists(typeof(Core.Entry).Assembly);
			options.IncludeXmlCommentsIfExists(typeof(Data.MsSql.Entry).Assembly);
		}
	}
}
