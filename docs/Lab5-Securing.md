# Lab 5 - Securing your Azure Functions

In this lab you will learn how to secure your functions in a Function App. You are going to start by using key based access to functions, and then progress to a manual deployment from the Command-Line Interface and progress to Visual Studio Team Services using a release pipeline.

Goals for this lab: 
- [Adding function level security](#1)
- [Using social identity authentication providers](#2)

## <a name="1"></a>1. Adding function level security

Azure Functions has 5 different modes for securing a HTTP trigger based function using authorization levels: ```Anonymous```, ```Function```, ```Admin```, ```User``` and ```System```. First, you will have a look at the ```Function``` level.

Open your Visual Studio solution and find the two HTTP trigger functions in the Function App. Change the ```HttpTrigger``` attribute to have a value of ```AuthorizationLevel.Function``` as the first parameter.

Run your application locally. Can you access the functions while running in the local host?

Commit and push your source code to the Git repo and start a new build and consequent release. After the release has completed, try to use the original URL to access the ```DumpHeadersFunction``` from the browser:
```
https://functionsworkshop2018.azurewebsites.net/api/dumpheadersfunction
```
This shouldn't work anymore. Navigate to the portal and find the Function App and the ```DumpHeadersFunction```.  There will be three different keys. One at the function level called ```default``` and two Host keys for all functions: ```_master``` and ```default```. Notice how the ```_master``` key cannot be revoked. The function level ```default``` is required to use in the URL to invoke the function. It requires a query string parameter ```code``` equal to the key.

Compose the final URL by appending ```?code=``` with your default function key included at the end. Try to execute the function again.

## <a name="2"></a>2. Using social identity authentication providers

Since a Function App is running as a Azure Web App we can leverage all capabilities of a Web App for our solution. In this next part you will configure your Function App to use authentication from social identity providers.
Go to your Function App in the portal and select the ```Platform Features``` tab. Follow the link to ```Authentication / Authorization``` and enable the ```App Service Authentication```. Next, select "Microsoft account" as your first authentication provider. Since it is not configured you need to take some additional steps. You can find detailed information on how to set up the provider at the top of the blade that opened. In short, take the following steps:

1. Navigate to http://go.microsoft.com/fwlink/p/?LinkId=262039 to be redirected to the Application Registration Portal of the Microsoft Account developer portal.
2. Under converged applications, click "Add an app". Give your app a name corresponding to the Function App, such as ```FunctionsWorkshop2018```. Click Create.
3. In the next screen, copy the Application Id and store it. 
4. Generate a password and store it as well.
5. Add a new platform and choose ```Web```. As the Redirect URL, use the URL of the root of the Function App and append: 
```
.auth/login/microsoftaccount/callback
``` 
6. Save the application.
7. In the blade for the MSA authentication provider enter the Application Id as Client Id and set the Client Secret to the generated password from step 4.
8. Save the provider configuration and also save the authentication settings for the Function App.

Change the function's authorization level to be of ```AuthorizationLevel.User```. Redeploy your Function App using your pipelines.
	
After this you should be good to try it out. Navigate to the root of your Function App, e.g. ```http://functionsworkshop2018.azurewebsites.net``` and notice how you need to authenticate with your Microsoft account and give consent to have the function app view profile information.
Note that the MSA provider might require a little more time to become active.

## <a name="3"></a>3. If you have time left...

Here are some ideas to spend some additional time on securing your Function App:
- Configure one of the other social identity authentication providers.
- Read up on the difference between function and host keys, plus the two other two authorization levels of ```Admin``` and ```System```. What are those levels meant for? Which keys can you use and which not?
- Turn on the Managed Service Identity (MSI) for the Function App from the Platform features. Try to read some secrets from the Azure Key Vault. You will have to create that vault and give the MSI of the Function App read and list access to the secrets. With a NuGet package you should be able to read secrets by only passing the URL of the Key Vault.

## Wrapup
In this lab you have secured your Function App by introducing key based function level authorization levels and by integrating with one or more authentication providers. 

Continue with [Lab 6 - Durable Functions](Lab6-DurableFunctions.md).