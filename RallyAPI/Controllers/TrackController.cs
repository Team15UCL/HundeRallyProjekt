using Microsoft.AspNetCore.Mvc;
using RallyAPI.Models;
using RallyAPI.Models.Services;

namespace RallyAPI.Controllers;
[ApiController]
[Route("[Controller]")]
public class TrackController(TrackService trackService) : ControllerBase
{
	[HttpGet("FindOne")]
	public ActionResult<Track> GetOne(string userName, string userRole, string name = "", string date = "", string location = "", string theme = "")
	{
		Track track = trackService.GetOne(userRole, userName, name, date, location, theme);

		if (track != null)
		{
			return track;
		}

		return BadRequest("No Track Found");
	}

	[HttpGet("FindMany")]
	public ActionResult<IEnumerable<Track>> GetMany(string userName, string userRole)
	{
		var tracks = trackService.GetAll(userRole, userName);

		if (tracks != null)
		{
			return tracks.ToList();
		}

		return BadRequest("No Tracks Found");
	}

	[HttpPost(Name = "CreateTrack")]
	public Track Post(string userName, string userRole, Track track)
	{
		track.UserClaims.Add(userName);
		track.RoleClaims.Add(userRole);
		track.Date = DateTime.Now;

		return trackService.SaveNewTrack(track);
	}

	[HttpPut(Name = "UpdateTrack")]
	public ActionResult Put(string userName, string userRole, Track track)
	{
		if (track.UserClaims.Contains(userName) || track.RoleClaims.Contains(userRole))
		{
			var updatedTrack = trackService.UpdateTrack(track);
			return Ok(updatedTrack);
		}
		return BadRequest();
	}

	[HttpDelete(Name = "DeleteTrack")]
	public ActionResult Delete(string userName, string userRole, Track track)
	{
		if (track.UserClaims.Contains(userName) || track.RoleClaims.Contains(userRole))
		{
			var result = trackService.DeleteTrack(track);

			if (result == true)
			{
				return Ok();
			}
			return BadRequest();
		}

		return BadRequest();
	}
}
