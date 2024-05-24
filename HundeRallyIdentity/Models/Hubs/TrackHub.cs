using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace HundeRallyIdentity.Models.Hubs;

public class TrackHub : Hub
{
	public async Task SendMessage(string message)
	{
		await Clients.All.SendAsync("ReceiveMessage", Context.User!.FindFirst(ClaimTypes.Name)!.Value, message);
	}
	public async Task SendMessageToEveryoneElse(string user, string message)
	{
		await Clients.AllExcept(Context.ConnectionId).SendAsync("ConnectionMessage", user, message);
	}

	public override async Task OnConnectedAsync()
	{
		await SendMessageToEveryoneElse(Context.User!.FindFirst(ClaimTypes.Name)!.Value, "er ankommet i chatten!");
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		await SendMessageToEveryoneElse(Context.User!.FindFirst(ClaimTypes.Name)!.Value, "har forladt chatten!");
		await base.OnDisconnectedAsync(exception);
	}

	public async Task NodeCreated(string url, decimal x, decimal y, decimal rotation, string borderColor)
	{
		await Clients.AllExcept(Context.ConnectionId).SendAsync("NodeCreated", url, x, y, rotation, borderColor);
	}

	public async Task NodeMoved(int id, string url, decimal x, decimal y)
	{
		await Clients.AllExcept(Context.ConnectionId).SendAsync("NodeMoved", id, url, x, y);
	}
}
