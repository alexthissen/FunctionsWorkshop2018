# Lab 0 - Getting started

This lab is going to let you prepare your development environment for the rest of the labs in the workshop. Not all steps are required. The more you can prepare, the better the experience during the workshop.

Goals for this lab: 
- [Prepare development laptop](#1)
- [Download required and optional tooling](#2)
- [Clone Git repository for lab code](#3)
 
## <a name="1"></a>1. Prepare your development laptop
Make sure that your laptop is up-to-date with the latest security patches. This workshop is specific towards Windows as the operating system for your machine. The labs can also be done on Linux, although this can be a bit more challenging.

## <a name="2"></a>2. Download required and optional tooling
First, you will need to have a development IDE installed. The most preferable IDE is [Visual Studio 2017](https://www.visualstudio.com/vs/) if you are running the Windows operating system.

You may want to consider installing [Visual Studio Code](https://code.visualstudio.com/) in the following cases:
- Your development machine is running OSX or a Linux distribution as your operating system.
- You want to have a light-weight IDE or use an alternative to Visual Studio 2017.
Please note that the development experience will be different from using Visual Studio 2017. The workshop assumes you have the latter. 

> Download and install [Visual Studio 2017 or Code](https://www.visualstudio.com/downloads/)

Second, you are going to need the Azure Functions tooling on your development machine. Instructions for installing the tooling can be found [here]((https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs/). 

The following optional tools are recommended, but not required.

- [GitHub Desktop](https://desktop.github.com/) for Git Shell and Git repository utilities
- [Postman](https://www.getpostman.com/) for issuing HTTP(S) requests to your functions.
- [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/) to interact with local and Azure-based storage resources.
- [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) for local development using queues, blobs and tables.

## <a name="3"></a>3. Clone Git repository for lab code
The workshop uses a Git repository for this documentation and the completed labs. 

Clone the repository to your development machine:
- Create a folder for the source code, e.g. `C:\Sources\FunctionsWorkshop2018`.
- Open a command prompt from that folder
- Clone the Git repository for the workshop files:

```
// Git address will be made available soon!
git clone https://github.com/alexthissen/functionsworkshop2018.git
```
- Set an environment variable to the root of the cloned repository from PowerShell:
```
$env:workshop = 'C:\Sources\FunctionsWorkshop2018'
```

## Wrapup
You have prepared your laptop and cloud environment to be ready for the next labs. Any issues you may have can probably be resolved during the labs. Ask your fellow attendees or the proctor to help you, if you cannot solve the issues.

Continue with [Lab 1 - Azure Functions 101](Lab1-Functions101.md).