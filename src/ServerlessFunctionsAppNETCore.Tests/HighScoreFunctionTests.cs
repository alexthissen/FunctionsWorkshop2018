using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Http.Internal;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace ServerlessFunctionsAppNETCore.Tests
{
    [TestClass]
    public class HighScoreFunctionTests
    {
        public readonly string playerName = "LX360";

        [TestMethod]
        public async Task GivenRequestHasInvalidScore_WhenRunIsCalled_BadRequestResponseShouldBeReturned()
        {
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

            // Act
            var response = await HighScoreFunction.Run(request.Object, playerName, log);
            var resultObject = response as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(resultObject, "Result object should be of type BadRequestObjectResult");
            Assert.AreEqual<int>((int)HttpStatusCode.BadRequest, resultObject.StatusCode.Value);
            Assert.AreEqual("Received invalid nickname and/or score!", resultObject.Value);
        }
    }
}
