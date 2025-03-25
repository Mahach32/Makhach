using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

// ���������� ����� IdentityServer
builder.Services.AddIdentityServer()
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = "client_id",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets =
            {
                new Secret("client_secret".Sha256())
            },
            AllowedScopes = { "api1" }
        }
    })
    .AddInMemoryApiResources(new List<ApiResource>
    {
        new ApiResource("api1", "My API")
    })
    .AddDeveloperSigningCredential(); // ����������� ��� ����������, �������� �� �������� ���������� � ������� �����

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:<port>/"; // �������� <port> �� ��� ����
        options.Audience = "api1";
    });

var app = builder.Build();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();