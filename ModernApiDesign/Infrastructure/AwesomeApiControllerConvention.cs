using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModernApiDesign.Infrastructure
{
    public class AwesomeApiControllerConvention: IApplicationModelConvention
    {        
        public void Apply(ApplicationModel application)
        {
            var controllers = Assembly.GetExecutingAssembly()
               .GetExportedTypes().Where(x => x.Name.EndsWith("Api"));
            
            foreach (var controller in controllers)
            {
                var controllerName = controller.Name.Replace("Api", "");

                var model = new ControllerModel(controller.GetTypeInfo(), controller.GetCustomAttributes().ToArray());
                model.ControllerName = controllerName;
                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel()
                    {
                        Template = $"{controller.Namespace.Replace(".", "/")}/{controllerName}"
                    }
                });

                foreach (var action in controller.GetMethods().Where(x => x.ReturnType == typeof(IActionResult)))
                {
                    var actionModel = new ActionModel(action, new object[] { new HttpGetAttribute() })
                    {
                        ActionName = action.Name
                    };
                    actionModel.Selectors.Add(new SelectorModel());
                    model.Actions.Add(actionModel);
                }
                application.Controllers.Add(controller);
            }
        }
    }
}
