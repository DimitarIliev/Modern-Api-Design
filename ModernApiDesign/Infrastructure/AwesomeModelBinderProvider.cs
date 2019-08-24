using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ModernApiDesign.Infrastructure.AwesomeModelBinder;

namespace ModernApiDesign.Infrastructure
{
    public class AwesomeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(EmotionalPhotoDto))
            {
                return new BinderTypeModelBinder(typeof(AwesomeModelBinder));
            }

            return null;
        }
    }
}
