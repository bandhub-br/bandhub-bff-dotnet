using System.Net;
using System.Net.Http;
using System.Text;
using BandHub.Bff.Features.Accounts.Login;
using BandHub.Bff.Integrations.UserService;
using FluentAssertions;
using FeatureLoginRequest = BandHub.Bff.Features.Accounts.Login.LoginRequest;

namespace BandHub.Bff.UnitTests.Features.Accounts.Login;

public class LoginHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldForwardRequestDataAndCancellationToken_ToUserServiceClient()
    {
        HttpRequestMessage? capturedRequest = null;
        CancellationToken capturedCancellationToken = CancellationToken.None;

        var httpHandler = new StubHttpMessageHandler((request, cancellationToken) =>
        {
            capturedRequest = request;
            capturedCancellationToken = cancellationToken;

            var payload = """
            {
              "accountId":"11111111-1111-1111-1111-111111111111",
              "name":"John",
              "email":"john@example.com",
              "accountType":"User"
            }
            """;

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };
        });

        var httpClient = new HttpClient(httpHandler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var userServiceClient = new UserServiceClient(httpClient);
        var handler = new LoginHandler(userServiceClient);
        var cancellationTokenSource = new CancellationTokenSource();

        await handler.HandleAsync(new FeatureLoginRequest("john@example.com", "password123"), cancellationTokenSource.Token);

        capturedRequest.Should().NotBeNull();
        capturedRequest!.Method.Should().Be(HttpMethod.Post);
        capturedRequest.RequestUri!.PathAndQuery.Should().Be("/accounts/login");

        var requestBody = await capturedRequest.Content!.ReadAsStringAsync();
        requestBody.Should().Contain("\"email\":\"john@example.com\"");
        requestBody.Should().Contain("\"password\":\"password123\"");

        capturedCancellationToken.CanBeCanceled.Should().BeTrue();
        capturedCancellationToken.IsCancellationRequested.Should().BeFalse();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnResponse_WhenUserServiceReturnsSuccess()
    {
        var httpHandler = new StubHttpMessageHandler((_, _) =>
        {
            var payload = """
            {
              "accountId":"11111111-1111-1111-1111-111111111111",
              "name":"John",
              "email":"john@example.com",
              "accountType":"User"
            }
            """;

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };
        });

        var httpClient = new HttpClient(httpHandler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var userServiceClient = new UserServiceClient(httpClient);
        var handler = new LoginHandler(userServiceClient);

        var response = await handler.HandleAsync(new FeatureLoginRequest("john@example.com", "password123"), CancellationToken.None);

        response.Name.Should().Be("John");
        response.Email.Should().Be("john@example.com");
        response.AccountType.Should().Be("User");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowHttpRequestException_WhenUserServiceReturnsUnauthorized()
    {
        var httpHandler = new StubHttpMessageHandler((_, _) =>
            new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("{\"message\":\"Credencias Inválidas.\"}", Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(httpHandler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var userServiceClient = new UserServiceClient(httpClient);
        var handler = new LoginHandler(userServiceClient);

        var act = async () => await handler.HandleAsync(new FeatureLoginRequest("john@example.com", "wrong"), CancellationToken.None);

        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*Credencias Inválidas.*");
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> _responseFactory;

        public StubHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> responseFactory)
        {
            _responseFactory = responseFactory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseFactory(request, cancellationToken));
        }
    }
}
