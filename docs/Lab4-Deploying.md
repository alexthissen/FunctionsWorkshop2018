# Lab 4 - Deploying Azure Functions

In this lab you will learn how to deploy your Function App from your development machine to Azure. You are going to start by doing a manual deployment from the Command-Line Interface and progress to Visual Studio Team Services using a release pipeline.

Goals for this lab: 
- [Deploy your Function App manually](#1)
- [Configuring application settings from Azure CLI](#2)
- [](#3)

## <a name="1"></a>1. Deploy your Function App manually

Use the Azure Function Core tools and its CLI to deploy the Function App you created in the previous lab. Open a console windows with administrator privileges and check whether the CLI is installed.
```
func --version
func --help
```
If the CLI is available, you are good to continue. Otherwise, refer to [Lab 0](Lab0-GettingStarted.md) to install the Azure Function Core tools.

Change the current directory of your command prompt to the folder of your Visual Studio solution. From there, you should be able to go into the bin\Release\netstandard2.0 publish folder if you have created a Release build from Visual Studio already. If not, perform a Release build now and go into the publish folder. 

From the command prompt login to Azure by running the command:
```
func azure login
```
Follow the instructions to login using a browser and the provided code. You should get a list of the available subscriptions. Make sure the correct subscription is active. You can switch with this command:
```
func azure subscriptions list
func azure subscriptions set <subscriptionguid>
```

After having selected the right subscription, you can deploy your application to the Function App in Azure with:
```
func azure functionapp list
func azure functionapp publish <FunctionsWorkshop2018>
```
where the ```list``` command returns a list of available Function Apps in the current subscription. You can choose the Function App you created in a previous lab, or create a new one. You will have to run an additional command  

```
Getting site publishing info...
Creating archive for current directory...
Uploading archive...
Upload completed successfully.
Syncing triggers...
```

Open the Azure portal and verify that the app was published successfully. Navigate to the Function App just published and check whether it is functioning correctly. Start by verifying the DumpHeadersFunction from the portal. Does it work, and if so, why? Fix your application if this function doesn't function correctly.

Proceed to the QRCodeGeneratorFunction. Check the resource group of your Function App to find the name of the Azure Storage resource. Drop a message in the queue and check if it is being processed. Before continuing, reason about what could be wrong.

You may have come to the conclusion that the application settings are not complete. The missing settings need to be deployed and should have the correct values.
Use the following command to retrieve all settings from the specified Function App.

```
func azure functionapp fetch-app-settings FunctionsWorkshop2018
```

This will write the settings from the portal to your ```local.settings.json``` file in the ```bin\Release\netstandard2.0``` folder. It will not overwrite your own local settings file in the root of the solution folder.
Edit the file for your local settings and add three new settings (if necessary) for the queues, tables and blob containers. 
- ```azurefunctions-queues```
- ```azurefunctions-tables```
- ```azurefunctions-blobs```

Copy the value of ```AzureWebJobsStorage``` to each of these settings and save your edits.

Next, publish your application again, using the flag to include the local settings, or just the local settings. Use the commands of your choice:

```
func azure functionapp publish FunctionsWorkshop2018 --publish-local-settings
func azure functionapp publish FunctionsWorkshop2018 --publish-settings-only
```

Verify that all functions are working now.

## <a name="2"></a>2. Configuring application settings from Azure CLI

In the previous part of this lab you should have noticed that the published application requires certain settings to be deployed. The Functions CLI does offer a way to publish all settings, but it requires downloading all settings and changes to a file. 
There in another way to publish settings and it uses the Azure CLI. This approach is more suitable for the next part of this lab to create an automated release pipeline.

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
This command will list the current settings of an Function App.
Next, run the command:

```
az functionapp config appsettings set --settings FUNCTIONS_EXTENSION_VERSION=beta azurefunctions-queues="..." azurefunctions-tables="..." azurefunctions-blobs="..."  --name FunctionsWorkshop2018 --resource-group FunctionsWorkshop2018
```
where the ... elipsis need to be replaced with the value of the connection string to the Azure Storage resource like before.
The individual settings are declared as space separated ```key=value``` pairs. This will change the application settings for the current runtime version to be 2.0 beta (later to be ~2) and also set the other settings.

Check that the settings are all present now in your Function App and that it still functions correctly.

## <a name="3"></a>3. Building your Function App in VSTS

With the application deployed manually including correct settings, you are in good shape to create a pipeline to automate build and release. 
Open the Visual Studio Team Services (VSTS) portal and navigate to the team project for the workshop.
You should see the Git repository under Code, Files. Navigate to the ```Build and Release``` tab and go to Builds.
Choose to create a new Build definition and pick the repository where your source is located. This should be under VSTS Git, but you might have chosen a different location for your code repository. For VSTS Git, pick the team project, Repository and branch to checkout for a build. Choose the ```ASP.NET Core``` template.

<img src="images/BuildPipeline.png" height="400"/>

Inspect each of the steps. To make this pipeline work you need to change some settings of the ```Publish``` task. Uncheck the ```Publish Web Projects``` checkbox and change the ```Path to the project(s)``` to be that of your Function App project, e.g. ```**/FunctionsWorkshop2018.csproj```.

After these changes you should be able to perform a successful build. To verify queue a new build from the ```Queue``` button at the top. 

## <a name="4"></a>4. Releasing your Function App to Azure

After a successful build you probably want to release your Function App to Azure. 

Create a new Release pipeline from the Releases tab. Choose the ```Azure App Service Deployment``` template, name your environment ```Production``` and find that your pipeline looks like similar to this:

<img src="images/ReleasePipeline.png" height="200"/>

Navigate to the Tasks of the pipeline by clicking the link in the Production environment that reads ```1 phase, 1 task```. At the top of the left pane there will be a block with the name of the environment. Once selected, you should see the most important settings to the right.
You need to select your own Azure subscription in the corresponding dropdown. If you haven't created an Azure Resource Manager service endpoint yet, you need to click the ```Manage``` link and create such a service endpoint. After that you can refresh the dropdown with the Refresh button and select your subscription. Next, select the ```Function App``` as the application type and your previously created Function App from the bottom dropdown.

Save the Release pipeline and queue a new release from the successful build you did previously. Verify that the release completes without errors and that the Function App works correctly.

You will need to be able to set the Application Settings of the Web App that hosts your Function App. You already did this from the Azure CLI before. This can also be done from a VSTS task during release.
Add a new ```Azure CLI``` task to the pipeline and point it to the same Azure subscription. Switch the script location to be inline script. Use the following inline script in this task:

```
az functionapp config appsettings set --settings FUNCTIONS_EXTENSION_VERSION=beta azurefunctions-queues=$(StorageConnection) azurefunctions-tables=$(StorageConnection) azurefunctions-blobs=$(StorageConnection) --name FunctionsWorkshop2018 --resource-group FunctionsWorkshop2018
```

and create a variable in the Release pipeline tab called ```Variables```. The variable should be named ```StorageConnection``` and have the same value as you assigned before using the Azure CLI script.

Save your Release pipeline and create a new release. Check that everything get deployed successfully and fix any errors.

## Wrapup
In this lab you have created a VSTS build and release pipeline to automate deploying your Function App in Azure.

Continue with [Lab 5 - ](Lab5-.md).




