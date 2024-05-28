using MongoDB.Driver;
using RallyAPI.Data;

namespace RallyAPI.Models.Services;

public class TrackService
{
	private readonly IMongoCollection<Track> _tracks;

	public TrackService(IDbAccess<Track> dbAccess)
	{
		_tracks = dbAccess.GetDbClient("Track");
	}

	public Track GetOne(string userRole, string userName, string name = "", string date = "", string location = "", string theme = "")
	{
		var claimsQuery = _tracks.AsQueryable().Where(t => t.RoleClaims.Contains(userRole) ||
													  t.UserClaims.Contains(userName));

		var searchQuery = claimsQuery.Where(t => t.Name == name ||
												t.Date.ToString() == date ||
												t.Location == location ||
												t.Theme == theme).FirstOrDefault();
		try
		{
			var id = searchQuery.Id;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return null;
		}
		return searchQuery;
	}

	public IEnumerable<Track> GetAll(string userRole, string userName)
	{
		var query = _tracks.AsQueryable().Where(t => t.RoleClaims.Contains(userRole) || t.UserClaims.Contains(userName));

		return query;
	}

	public Track SaveNewTrack(Track track)
	{
		_tracks.InsertOne(track);

		var query = _tracks.AsQueryable().Where(t => t.Date == track.Date).FirstOrDefault();

		return query;
	}

	public Track UpdateTrack(Track track)
	{
		track.Id = _tracks.AsQueryable().Where(t => t.Date == track.Date).FirstOrDefault()!.Id;

		var filter = Builders<Track>.Filter.Eq(t => t.Id, track.Id);

		_tracks.ReplaceOne(filter, track);

		return _tracks.Find(filter).FirstOrDefault();
	}

	public bool DeleteTrack(Track track)
	{
		track.Id = _tracks.AsQueryable().Where(t => t.Name == track.Name && t.Date == track.Date).FirstOrDefault()!.Id;

		var filter = Builders<Track>.Filter.Eq(t => t.Id, track.Id);

		var result = _tracks.DeleteOne(filter);

		return result.IsAcknowledged;
	}
}
