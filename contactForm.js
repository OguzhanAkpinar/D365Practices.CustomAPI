if (typeof (D365Practice) == 'undefined') {
    D365Practice = {}
}

D365Practice.ContactForm = (function () {
    var setMarketingPermissions = function (formExecutionContext) {
        var formContext = formExecutionContext.getFormContext();
        var isPrioritized = formContext.getAttribute("cr31a_isprioritized").getValue();
        if (isPrioritized) {
            var input = {
                new_contactRef: {
                    "contactid": "f5973462-768e-eb11-b1ac-000d3ae92b46",
                    "@odata.type": "Microsoft.Dynamics.CRM.contact"
                },
                new_allowemail: true,
                new_allowsms: true
            };
            var req = new XMLHttpRequest();
            req.open("POST", Xrm.Page.context.getClientUrl() +
                "/api/data/v9.2/new_setmarketingpermisssions", false);
            addHeaders(req);
            req.onreadystatechange = function () {
                if (this.readyState === 4) {
                    this.onreadystatechange = null;
                    if (this.status === 204) {
                        result = JSON.parse(this.responseText);
                    } else {
                        Xrm.Utility.alertDialog(this.statusText);
                    }
                }
            };
            req.send(JSON.stringify(input));
        }
    }

    var getMarketingPermissions = function (formExecutionContext) {
        debugger;
        var formContext = formExecutionContext.getFormContext();
        var req = new XMLHttpRequest();
        req.open("GET", Xrm.Page.context.getClientUrl() +
            "/api/data/v9.2/new_getmarketingpermissions(new_contactid=f5973462-768e-eb11-b1ac-000d3ae92b46)", false);
        addHeaders(req);
        req.onreadystatechange = function () {
            if (this.readyState === 4) {
                this.onreadystatechange = null;
                if (this.status === 200) {
                    result = JSON.parse(this.responseText);
                    if (result.new_allowemail || result.new_allowsms) {
                        formContext.getControl("cr31a_isprioritized").setDisabled(false);
                    } else {
                        formContext.getControl("cr31a_isprioritized").setDisabled(true);
                    }
                } else {
                    Xrm.Utility.alertDialog(this.statusText);
                }
            }
        };
        req.send();
    }

    var addHeaders = function (req) {
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
    }

    return {
        GetMarketingPermissions: getMarketingPermissions,
        SetMarketingPermissions: setMarketingPermissions
    }
})()