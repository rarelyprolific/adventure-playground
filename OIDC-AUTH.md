# Secure an ASP.NET Core application using OIDC

## Identity Provider - Keycloak

https://www.keycloak.org/getting-started/getting-started-docker

```shell
docker run -p 8510:8080 --name keyclock-idp -d -e KC_BOOTSTRAP_ADMIN_USERNAME=admin -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:26.0.5 start-dev
```

### Create realm

1. Go to admin console at http://localhost:8510/admin
2. Create realm "mordor"

### Create user

1. Create user "demoadmin" (Demonstration Administrator)
2. Set password for "demoadmin".
3. Test login via http://localhost:8510/realms/mordor/account

### Register application

1. Select "mordor" realm" in http://localhost:8510/admin
2. Select Clients, then "Create client"
3. Select "OpenID Connect" and set Client ID to "admin-site"
4. Select "Standard flow" (this enables the OIDC "Authorisation Code Flow")
5. Redirect URI: https://localhost:8530/signin-oidc

## ASP.NET Core application

```
dotnet new sln -n AdminSite
dotnet new webapi -n AdminSite -o AdminSite --no-openapi --use-controllers
dotnet sln add AdminSite/AdminSite.csproj
```

```
dotnet add package Duende.AccessTokenManagement.OpenIdConnect --version 3.0.1
```

### Code changes

Ensure application is running on `https://localhost:8530` via launchSettings.json.

Add `[Authorize]` attribute to controller to require authorisation.

Add to `Program.cs`:

```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "AdminSite";
    options.Cookie.SameSite = SameSiteMode.Lax;
})
.AddOpenIdConnect(options =>
{
    options.Authority = "http://localhost:8510/realms/mordor";
    options.ClientId = "admin-site";
    options.ClientSecret = "[generate-key-in-keycloak-keys-section-for-client]";
    options.ResponseType = "code";

    options.RequireHttpsMetadata = false;
});
```
