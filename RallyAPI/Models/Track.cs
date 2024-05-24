using MongoDB.Bson;

namespace RallyAPI.Models;

public class Track
{
	public ObjectId Id { get; set; }
	public string? Name { get; set; }
	public string? Theme { get; set; }
	public string? Location { get; set; }
	public DateTime Date { get; set; }
	public List<string> UserClaims { get; set; } = [];
	public List<string> RoleClaims { get; set; } = [];
	public List<Node>? Nodes { get; set; }
}
