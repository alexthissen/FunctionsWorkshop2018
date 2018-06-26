# Lab 2 - Azure Functions in Visual Studio 2017

In this lab you will ... 

Goals for this lab: 
- [](#1)
- [](#2)
- [](#3)

## <a name="1"></a>1. Configuring application settings from CLI
First, make sure that you have successfully logged in using the Azure CLI tooling. Run the following commands and select the appropriate subscription from the list.
```
az login
az account list
az account set --subscription <your subscription guid>
```

You can use the Azure CLI to retrieve a list of application settings of the Function App, and delete, change or create these. 

```
az functionapp config appsettings list --name FunctionsWorkshop2018 --resource-group FunctionsWorkshop2018
```
will list the current settings of an Function App.
Next, run the command 
```
az functionapp config appsettings set --settings FUNCTIONS_EXTENSION_VERSION=beta BaseURL=http://www.xpirit.com --name FunctionsWorkshop2018 --resource-group FunctionsWorkshop2018
```
in order to change the application setting for the current runtime version to be 2.0 beta (later to be ~2). The individual settings are declared as space separated ```key=value``` pairs. 