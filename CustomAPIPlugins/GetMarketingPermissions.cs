using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CustomAPIPlugins
{
    public class GetMarketingPermissions : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            Guid contactId = (Guid)context.InputParameters["new_contactid"];
            QueryExpression query = new QueryExpression("cr31a_marketingpermissions");
            query.ColumnSet = new ColumnSet("cr31a_allowsms", "cr31a_allowemail");
            query.Criteria.AddCondition(new ConditionExpression("cr31a_contactid", ConditionOperator.Equal, contactId));
            EntityCollection result = service.RetrieveMultiple(query);

            bool? allowSMS = null, allowEmail = null;

            if (result.Entities.Count > 0)
            {
                if (result.Entities[0].Attributes.Contains("cr31a_allowsms"))
                    allowSMS = (bool)result.Entities[0].Attributes["cr31a_allowsms"];
                if (result.Entities[0].Attributes.Contains("cr31a_allowemail"))
                    allowEmail = (bool)result.Entities[0].Attributes["cr31a_allowemail"];
            }
            context.OutputParameters["new_allowemail"] = allowEmail;
            context.OutputParameters["new_allowsms"] = allowSMS;
        }
    }
}
