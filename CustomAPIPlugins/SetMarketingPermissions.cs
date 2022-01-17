using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CustomAPIPlugins
{
    public class SetMarketingPermissions : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            EntityReference contactRef = context.InputParameters["new_contactRef"] as EntityReference;
            bool allowSMS = (bool)context.InputParameters["new_allowsms"];
            bool allowEmail = (bool)context.InputParameters["new_allowemail"];

            QueryExpression query = new QueryExpression("cr31a_marketingpermissions");
            query.ColumnSet = new ColumnSet("cr31a_allowsms", "cr31a_allowemail");
            query.Criteria.AddCondition(new ConditionExpression("cr31a_contactid", ConditionOperator.Equal, contactRef.Id));
            EntityCollection result = service.RetrieveMultiple(query);

            Entity marketingPermissions = new Entity("cr31a_marketingpermissions");
            marketingPermissions["cr31a_allowemail"] = allowEmail;
            marketingPermissions["cr31a_allowsms"] = allowSMS;
            if (result.Entities.Count > 0)
            {
                marketingPermissions.Id = result.Entities[0].Id;
                service.Update(marketingPermissions);
            }
            else
            {
                service.Create(marketingPermissions);
            }
        }
    }
}
