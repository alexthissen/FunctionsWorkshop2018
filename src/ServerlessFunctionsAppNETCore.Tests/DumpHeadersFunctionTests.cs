using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ServerlessFunctionsAppNETCore.Tests
{
    [TestClass]
    public class DumpHeadersFunctionTests
    {
        [TestMethod]
        public void GivenRequestHasHeaders_WhenRunIsCalled_ResponseShouldReflectHeaderValues()
        {
            // Arrange
            var log = new Mock<ILogger>();
            var request = new Mock<HttpRequest>();
            var headers = new HeaderDictionary();
            headers.Add("custom", "AzureFunctions");
            request.Setup(r => r.Headers).Returns(headers);

            // Act
            var response = DumpHeadersFunction.Run(request.Object, log.Object);

            // Assert
            var resultObject = (OkObjectResult)response;
            Assert.AreEqual("custom='AzureFunctions',", resultObject.Value);
        }
    }
}
