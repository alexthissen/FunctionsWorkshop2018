# Lab 3 - Unit testing Azure Functions

In this lab you will create a couple of unit tests for your Azure Functions. This allows you to test your functions without having to actually host and call them. Unit tests for Azure Functions are a bit different from regular unit tests, but still comparable.

Goals for this lab: 
- [Unit testing HTTP triggered functions](#1)
- [](#2)
- [](#3)

## <a name="1"></a>1. Unit testing HTTP triggered functions



## <a name="4"></a>4. If you have time left...

Should you be finished early, you can continue to write some tests for the other Azure Functions ```ImportHighScoreFunction``` and ```QRCodeGeneratorFunction```. This will require some work to create mocks and isolate your function's logic. Also, you can look at how to refactor the implementation of your function to simplify the overall structure and unit tests. 

## Wrapup
In this lab you have created your first two unit tests for the HTTP trigger based functions. In the next lab these unit tests will be executed by a build pipeline.

Continue with [Lab 4 - Deploying Azure Functions](Lab4-Deploying.md).
