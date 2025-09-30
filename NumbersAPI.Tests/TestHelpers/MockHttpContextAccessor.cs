using Microsoft.AspNetCore.Http;
using Moq;

namespace NumbersAPI.Tests.TestHelpers
{
    public static class MockHttpContextAccessor
    {
        public static Mock<IHttpContextAccessor> CreateMock()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new Mock<ISession>();
            var sessionData = new Dictionary<string, byte[]>();

            // Mock session behavior
            mockSession.Setup(s => s.SetString(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((key, value) =>
                {
                    sessionData[key] = System.Text.Encoding.UTF8.GetBytes(value);
                });

            mockSession.Setup(s => s.GetString(It.IsAny<string>()))
                .Returns<string>(key =>
                {
                    if (sessionData.TryGetValue(key, out var value))
                    {
                        return System.Text.Encoding.UTF8.GetString(value);
                    }
                    return null;
                });

            mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
            mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

            return mockHttpContextAccessor;
        }
    }
}
