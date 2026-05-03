using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ToDoList.Api.Tests.Support;
using ToDoList.Contracts.Auth.Commands.Login;
using ToDoList.Contracts.Todos.Commands.Create;
using ToDoList.Contracts.Todos.Queries.GetMine;

namespace ToDoList.Api.Tests;

public sealed class TodoApiIntegrationTests
{
    private static readonly JsonSerializerOptions CamelJson =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private static Task<HttpResponseMessage> PostJson<T>(HttpClient client, string url, T body) =>
        client.PostAsync(url, JsonContent.Create(body, options: CamelJson));

    private static Task<HttpResponseMessage> PatchJson<T>(HttpClient client, string url, T body)
    {
        var msg = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Content = JsonContent.Create(body, options: CamelJson),
        };
        return client.SendAsync(msg);
    }

    [Fact]
    public async Task Get_todos_without_token_returns_Unauthorized()
    {
        using var factory = new ApiWebApplicationFactory();
        using var client = factory.CreateClient();

        using var resp = await client.GetAsync("/api/todos");

        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    [Fact]
    public async Task Register_conflict_login_crud_own_todos()
    {
        using var factory = new ApiWebApplicationFactory();
        using var client = factory.CreateClient();

        using var first = await PostJson(client, "/api/auth/register", new { username = "alice", password = "password1" });
        Assert.Equal(HttpStatusCode.Created, first.StatusCode);

        using var duplicate = await PostJson(client, "/api/auth/register", new { username = "alice", password = "password1" });
        Assert.Equal(HttpStatusCode.Conflict, duplicate.StatusCode);

        using var login = await PostJson(client, "/api/auth/login", new { username = "alice", password = "password1" });

        login.EnsureSuccessStatusCode();

        var tokenBody = await login.Content.ReadFromJsonAsync<LoginResponse>(CamelJson);
        Assert.NotNull(tokenBody);
        Assert.False(string.IsNullOrWhiteSpace(tokenBody!.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenBody.AccessToken);

        using var createResp = await PostJson(client, "/api/todos", new { title = "Ship it" });
        createResp.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.Created, createResp.StatusCode);

        var created = await createResp.Content.ReadFromJsonAsync<CreateTodoDto>(CamelJson);
        Assert.NotNull(created);

        using var list = await client.GetAsync("/api/todos");

        list.EnsureSuccessStatusCode();

        var todos = await list.Content.ReadFromJsonAsync<List<TodoDto>>(CamelJson);
        Assert.NotNull(todos);
        Assert.Single(todos!);

        using var patch = await PatchJson(client, $"/api/todos/{created!.Id}/status", new { isCompleted = true });

        patch.EnsureSuccessStatusCode();

        using var deleted = await client.DeleteAsync($"/api/todos/{created.Id}");

        deleted.EnsureSuccessStatusCode();

        using var after = await client.GetAsync("/api/todos");

        after.EnsureSuccessStatusCode();

        var remaining = await after.Content.ReadFromJsonAsync<List<TodoDto>>(CamelJson);
        Assert.NotNull(remaining);
        Assert.Empty(remaining);
    }

    [Fact]
    public async Task User_cannot_mutate_other_users_todo()
    {
        using var factory = new ApiWebApplicationFactory();

        using var alice = factory.CreateClient();
        using var regAlice = await PostJson(alice, "/api/auth/register", new { username = "owner", password = "password1" });
        regAlice.EnsureSuccessStatusCode();

        using var loginAlice =
            await PostJson(alice, "/api/auth/login", new { username = "owner", password = "password1" });

        loginAlice.EnsureSuccessStatusCode();

        var aliceToken = await loginAlice.Content.ReadFromJsonAsync<LoginResponse>(CamelJson);
        alice.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", aliceToken!.AccessToken);

        using var create = await PostJson(alice, "/api/todos", new { title = "Alice task" });

        create.EnsureSuccessStatusCode();

        var created = await create.Content.ReadFromJsonAsync<CreateTodoDto>(CamelJson);
        Assert.NotNull(created);

        using var eve = factory.CreateClient();

        using var regEve = await PostJson(eve, "/api/auth/register", new { username = "intruder", password = "password1" });

        regEve.EnsureSuccessStatusCode();

        using var loginEve = await PostJson(eve, "/api/auth/login", new { username = "intruder", password = "password1" });

        loginEve.EnsureSuccessStatusCode();

        var eveToken = await loginEve.Content.ReadFromJsonAsync<LoginResponse>(CamelJson);
        eve.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", eveToken!.AccessToken);

        using var forbiddenPatch =
            await PatchJson(eve, $"/api/todos/{created!.Id}/status", new { isCompleted = true });

        Assert.Equal(HttpStatusCode.Forbidden, forbiddenPatch.StatusCode);

        using var forbiddenDelete = await eve.DeleteAsync($"/api/todos/{created.Id}");
        Assert.Equal(HttpStatusCode.Forbidden, forbiddenDelete.StatusCode);
    }

}
