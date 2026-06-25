using System.Net;
using System.Net.Http;
using System.Text;
using BandHub.Bff.Features.Accounts.Login;
using BandHub.Bff.Integrations.AuthService;
using FluentAssertions;
using FeatureLoginRequest = BandHub.Bff.Features.Accounts.Login.LoginRequest;

namespace BandHub.Bff.UnitTests.Features.Accounts.Login;

public class LoginHandlerTests
{
    private static readonly string ValidPayload = """
        {
          "accountId":"11111111-1111-1111-1111-111111111111",
          "name":"John",
          "email":"john@example.com",
          "accountType":"User",
          "acessToken":"fake-token",
          "acessTokenExpiraEm":"2026-06-25T12:00:00Z",
          "refreshToken":"fake-refresh",
          "refreshTokenExpiraEm":"2026-07-02T12:00:00Z"
        }
        """;

    [Fact]
    public async Task HandleAsync_ShouldForwardRequestDataAndCancellationToken_ToAuthServiceClient()
    {
        HttpRequestMessage? capturedRequest = null;
        CancellationToken capturedCancellationToken = CancellationToken.None;

        var httpHandler = new StubHttpMessageHandler((request, cancellationToken) =>
        {
            capturedRequest = request;
            capturedCancellationToken = cancellationToken;

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(ValidPayload, Encoding.UTF8, "application/json")
            };
        });

        var httpClient = new HttpClient(httpHandler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var authServiceClient = new AuthServiceClient(httpClient);
        var handler = new LoginHandler(authServiceClient);
        var cancellationTokenSource = new CancellationTokenSource();

        await handler.HandleAsync(new FeatureLoginRequest("john@example.com", "password123"), cancellationTokenSource.Token);

        capturedRequest.Should().NotBeNull();
        capturedRequest!.Method.Should().Be(HttpMethod.Post);
        capturedRequest.RequestUri!.PathAndQuery.Should().Be("/auth/login");

        var requestBody = await capturedRequest.Content!.ReadAsStringAsync();
        requestBody.Should().Contain("\"email\":\"john@example.com\"");
        requestBody.Should().Contain("\"password\":\"password123\"");

        capturedCancellationToken.CanBeCanceled.Should().BeTrue();
        capturedCancellationToken.IsCancellationRequested.Should().BeFalse();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnResponse_WhenAuthServiceReturnsSuccess()
    {
        var httpHandler = new StubHttpMessageHandler((_, _) =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(ValidPayload, Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(httpHandler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var authServiceClient = new AuthServiceClient(httpClient);
        var handler = new LoginHandler(authServiceClient);

        var response = await handler.HandleAsync(new FeatureLoginRequest("john@example.com", "password123"), CancellationToken.None);

        response.Name.Should().Be("John");
        response.Email.Should().Be("john@example.com");
        response.AccountType.Should().Be("User");
        response.AcessToken.Should().Be("fake-token");
        response.RefreshToken.Should().Be("fake-refresh");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowHttpRequestException_WhenAuthServiceReturnsUnauthorized()
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

        var authServiceClient = new AuthServiceClient(httpClient);
        var handler = new LoginHandler(authServiceClient);

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
