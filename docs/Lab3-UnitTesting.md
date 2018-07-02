# Lab 3 - Unit testing Azure Functions

In this lab you will create a couple of unit tests for your Azure Functions. This allows you to test your functions without having to actually host and call them. Unit tests for Azure Functions are a bit different from regular unit tests, but still comparable.

Goals for this lab: 
- [Unit testing Http triggered functions](#1)
- [Unit testing POST Http Triggers](#2)

## <a name="1"></a>1. Unit testing HTTP triggered functions

To setup unit testing you need to add a Unit Test project to your solution. Yuo can choose your preferred unit test framework, such as MSTest or XUnit. Prepare the test project by taking the following steps:

1. Add a project reference to the Function App project.
2. Include NuGet package reference to Moq. Alternatively, you can select your own mocking framework.

This lab assumes you are using MSTest and Moq.
You will create a unit test for the DumpHeadersFunction first. 

> Take a moment to look at the signature of that function. Forget about the attributes you see and focus on the bare signature. 
>
> Which arguments do you need to pass?

Next, change the name of the precreated unit test to be like this:
```
[TestMethod]
public void GivenRequestHasHeaders_WhenRunIsCalled_ResponseShouldReflectHeaderValues()
``` 

Add an Arrange section for mocking the ```HttpRequest``` object.
```
// Arrange
MockTraceWriter log = new MockTraceWriter();
var request = new Mock<HttpRequest>();
var headers = new HeaderDictionary();
headers.Add("custom", "AzureFunctions");
request.Setup(r => r.Headers).Returns(headers);
```
This will create a ```HttpRequest``` mock that pretends to have 1 header ```custom```.

Create a new file ```MockTraceWriter.cs``` and add code to have a dummy implementation of the ```TraceWriter``` class.
```
public class MockTraceWriter : TraceWriter
{
    List<TraceEvent> events = new List<TraceEvent>();
    public MockTraceWriter() : base(TraceLevel.Info) { }
    public override void Trace(TraceEvent traceEvent)
    {
        events.Add(traceEvent);
    }
}
```

Continue to add the Act part of the unit test:
```
// Act
var response = DumpHeadersFunction.Run(request.Object, log);
```

Finally, implement the Assert part with this code:
```
// Assert
var resultObject = (OkObjectResult)response;
Assert.AreEqual("custom='AzureFunctions',", resultObject.Value);
```
Execute the unit test from the Test Explorer. If all goes well it should execute correctly and give a green result. Fix any errors if they occur.

## <a name="2"></a>2. Unit testing POST Http Triggers

Testing a Http Trigger based Function is a little trickier. It involves more work to setting up the mock ```HttpRequest``` object.

Add a new unit test method to the project:
```
[TestMethod]
public async Task GivenRequestHasInvalidScore_WhenRunIsCalled_BadRequestResponseShouldBeReturned()
```

Define a readonly string for the player name and give it a value for your favorite nickname.
```
public readonly string playerName = "LX360";
```

Inside the unit test the Arrange part will now look like the following: 
```
// Arrange
ILogger log = new Mock<ILogger>().Object;
var request = new Mock<HttpRequest>();

// Strictly not necessary
request.Setup(req => req.Method).Returns("POST");
request.Setup(req => req.Path).Returns("/api/HighScores/player/");

// Setup content of body
string body = "not1337";
var stream = new MemoryStream();
var writer = new StreamWriter(stream);
writer.Write(body);
writer.Flush();
stream.Position = 0;
request.Setup(req => req.Body).Returns(stream);
```

> Notice how the setup of the body is being performed. 
> 
> Why is it not necessary to setup the request's ```Method``` and ```Path``` properties?

Next, include the Act in the unit test:
```
// Act
var response = await HighScoreFunction.Run(request.Object, playerName, log);
var resultObject = response as BadRequestObjectResult;
```

And finally, again the asserts of the test.
```
// Assert
Assert.IsNotNull(resultObject, "Result object should be of type BadRequestObjectResult");
Assert.AreEqual<int>((int)HttpStatusCode.BadRequest, resultObject.StatusCode.Value);
Assert.AreEqual("Received invalid nickname and/or score!", resultObject.Value);
```

Execute both tests again. Both should succeed. Make sure both pass.

## <a name="3"></a>3. If you have time left...

Should you be finished early, you can continue to write some tests for the other Azure Functions ```ImportHighScoreFunction``` and ```QRCodeGeneratorFunction```. This will require some work to create mocks and isolate your function's logic. Also, you can look at how to refactor the implementation of your function to simplify the overall structure and unit tests. 

## Wrapup
In this lab you have created your first two unit tests for the HTTP trigger based functions. In the next lab these unit tests will be executed by a build pipeline.

Continue with [Lab 4 - Deploying Azure Functions](Lab4-Deploying.md).
