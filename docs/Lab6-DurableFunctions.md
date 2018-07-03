# Lab 6 - Durable Functions

In this lab you will learn how to program durable functions using the Durable Extensions Framework.

Goals for this lab: 
- [A durable Hello World function](#1)
- [](#2)
- [](#3)

## <a name="1"></a>1. A durable Hello World function

The first thing you will do to get acquianted with Durable Functions is to create and execute a Hello World version.

Add a new function to the project and this time select the ```Durable Functions Orchecstration``` template.

This should add a single file to the project. Spend some time examining the contents of that file.

> How many functions do you see inside the single file?
> What is the purpose of each of the functions in the orchestration?

Run your project and you should see the new functions appear. Copy and paste the URL of the new function with _HttpStart in the name. The function will start the orchestration and return JSON data to your browser. It contains information about the instance of the orchestration you have just started, including some URLs to check the status. Copy and paste ```statusQueryGetUri``` to a new browser tab and check the status. 

## <a name="2"></a>2. Building an orchestration and activities

Next you will build an orchestration that will accept new high scores, which must be stored in a storage table, as well as generate a QR code for each new high score. 

Begin by adding two folders to your project: 
1. Activities
2. Orchestrations

Add a new class to the Activities folder and name it ```QRCodeGeneratorActivity```. Implement it with the following code, which is common from the QRCodeGeneratorFunction mostly:
```
[FunctionName(nameof(QRCodeGeneratorActivity))]
public static async Task<string> Run([ActivityTrigger] HighScore score,
    ILogger logger,
    [Blob("azurefunctions-qrcode-images/{rand-guid}",
        FileAccess.ReadWrite,
        Connection = "azurefunctions-blobs")] CloudBlockBlob blob)
{
    logger.LogInformation($"Generating QR Code for high score {score.Score} by {score.Nickname}");

    QRCodeGenerator generator = new QRCodeGenerator();
    QRCodeData data = generator.CreateQrCode($"{score.Nickname} scored {score.Score}", QRCodeGenerator.ECCLevel.H);
    QRCode code = new QRCode(data);

    using (var stream = await blob.OpenWriteAsync())
    {
        Bitmap bitmap = code.GetGraphic(20, Color.Black, Color.White, true);
        bitmap.Save(stream, ImageFormat.Png);
    }
    return blob.StorageUri.PrimaryUri.AbsoluteUri;
}

```

Also add a new class to the Orchestrations folder, and name it ```RegisterNewHighScoreOrchestration```.
Implement it with the following code:
```
[FunctionName(nameof(RegisterNewHighScoreOrchestration))]
public static async Task<string> RunOrchestrator(
    [OrchestrationTrigger] DurableOrchestrationContextBase context, 
    ILogger log)
{
    var score = context.GetInput<HighScore>();

    var blobUri = await context.CallActivityAsync<string>(
        nameof(QRCodeGeneratorActivity),
        score);
    return blobUri;
}
```

Next, add a model class ```HighScore``` for holding the data from the HTTP Request.
```
public class HighScore
{
    public string Nickname { get; set; }
    public int Score { get; set; }
}
```

Finally, add a Function that trigger the start of the orchestration at the root of the project.
```
[FunctionName("RegisterNewHighScore_HttpStart")]
public static async Task<HttpResponseMessage> HttpStart(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
    [OrchestrationClient]DurableOrchestrationClient starter,
    ILogger logger)
{
    // Function input comes from the request content.
    HighScore score = new HighScore() { Nickname = "LX360", Score = 1337 };

    string instanceId = await starter.StartNewAsync(nameof(RegisterNewHighScoreOrchestration), score);

    logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

    return await starter.WaitForCompletionOrCreateCheckStatusResponseAsync(
        req, instanceId, TimeSpan.FromSeconds(5));
}
```

For now, the data for the ```HighScore``` object is hardwired, but you can fix that later.

Compile the project, fix any errors and start the project. Navigate to the new URL for the starter function and watch what happens. 

> Perhaps something goes wrong and you restarted the Function App. If not, imagine what would happen if the QRCodeGeneratorActivity gets retried.
> 
> What causes the problem?

## <a name="3"></a>3. Imperative bindings

The problem lies in the fact that a Durable Function and any of its orchestrations and activities may get retried. It should be idempotent and give the same results every time it is run or retried. The ```{rand-guid}``` expression causes a new blob object to be created every time. This needs to be fixed.

Remove the argument

## Wrapup
In this lab you have secured your Function App by introducing key based function level authorization levels and by integrating with one or more authentication providers. 
