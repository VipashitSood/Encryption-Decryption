using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Tm.Core.Infrastructure;
using Tm.Framework.Models;
using Tm.Framework.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Infrastructure
{
    public class CustomModelBinder : ComplexTypeModelBinder
    {
        public CustomModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders, ILoggerFactory loggerFactory)
       : base(propertyBinders, loggerFactory)
        {
        }
    }

    public class CustomModelBinderProvider : IModelBinderProvider
    {

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(BaseTmModel))
            {

                //create binders for all model properties
                var propertyBinders = context.Metadata.Properties
                    .ToDictionary(modelProperty => modelProperty, modelProperty => context.CreateBinder(modelProperty));

                // This causes the actual issue...for this model is not binding from ajax call
                // return new NopModelBinder(propertyBinders, EngineContext.Current.Resolve<ILoggerFactory>());  

                return null;
            }

            return null;
        }
    }

    public class CustomStartup : ITmStartup
    {
        public int Order => int.MaxValue;

        public void Configure(IApplicationBuilder application)
        {

        }

        public void ConfigureServices(IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {

            services.AddMvcCore(options => {
                // Removed Nop binder and add custom binder
                // Is this okay?
                options.ModelBinderProviders.RemoveType(typeof(TmModelBinderProvider));
                options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());

            });

        }

    }

}
