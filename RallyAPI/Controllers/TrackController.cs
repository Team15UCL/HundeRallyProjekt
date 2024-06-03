using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using RallyAPI.Models;
using RallyAPI.Models.Services;

namespace RallyAPI.Controllers;
[ApiController]
[Route("[Controller]")]
public class TrackController(TrackService trackService) : ControllerBase
{
	private static readonly string[] secrets = ["team15"];

	/// <summary>
	/// Searches database for a track by given name
	/// </summary>
	/// <param name="name"></param>
	/// <param name="date"></param>
	/// <param name="location"></param>
	/// <param name="theme"></param>
	/// <returns></returns>
	[HttpGet("FindOne")]
	public ActionResult<Track> GetOne(string name, string date = "", string location = "", string theme = "")
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var tokenstring = token.ToString().Remove(0, 7);

			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(tokenstring);

			bool a = decodedToken.TryGetValue("UserName", out string userName);
			bool b = decodedToken.TryGetValue("UserRole", out string userRole);

			if (a && b)
			{
				Track track = trackService.GetOne(userRole, userName, name, date, location, theme);

				if (track != null)
				{
					return track;
				}
			}

			return BadRequest("No Track Found");
		}

		return BadRequest("AuthToken not found");

	}

	/// <summary>
	/// Searches database for all tracks the user is authorized to see
	/// </summary>
	/// <returns></returns>
	[HttpGet("FindMany")]
	public ActionResult<IEnumerable<Track>> GetMany()
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var tokenstring = token.ToString().Remove(0, 7);

			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(tokenstring);

			bool a = decodedToken.TryGetValue("UserName", out string userName);
			bool b = decodedToken.TryGetValue("UserRole", out string userRole);

			if (a && b)
			{
				var tracks = trackService.GetAll(userRole, userName);
				if (tracks != null)
				{
					return tracks.ToList();
				}

				return BadRequest("No Tracks Found");
			}

			return BadRequest();
		}

		return BadRequest();
	}

	/// <summary>
	/// Inserts a track in the database
	/// </summary>
	/// <param name="track"></param>
	/// <returns></returns>
	[HttpPost(Name = "CreateTrack")]
	public ActionResult<Track> Post(Track track)
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var tokenstring = token.ToString().Remove(0, 7);

			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(tokenstring);

			bool a = decodedToken.TryGetValue("UserName", out string userName);
			bool b = decodedToken.TryGetValue("UserRole", out string userRole);

			if (a && b)
			{
				track.UserClaims.Add(userName);
				track.RoleClaims.Add(userRole);
				track.Date = DateTime.Now;

				return trackService.SaveNewTrack(track);
			}
		}

		return BadRequest("AuthToken not found!");
	}

	/// <summary>
	/// Updates a track in the database
	/// </summary>
	/// <param name="track"></param>
	/// <returns></returns>
	[HttpPut(Name = "UpdateTrack")]
	public ActionResult Put(Track track)
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var tokenstring = token.ToString().Remove(0, 7);

			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(tokenstring);

			bool a = decodedToken.TryGetValue("UserName", out string userName);
			bool b = decodedToken.TryGetValue("UserRole", out string userRole);

			if (a && b)
			{
				if (track.UserClaims.Contains(userName) || track.RoleClaims.Contains(userRole))
				{
					var updatedTrack = trackService.UpdateTrack(track);
					return Ok(updatedTrack);
				}

				return BadRequest();
			}

			return BadRequest();
		}

		return BadRequest();
	}

	/// <summary>
	/// Deletes a track in the database
	/// </summary>
	/// <param name="track"></param>
	/// <returns></returns>
	[HttpDelete(Name = "DeleteTrack")]
	public ActionResult Delete(Track track)
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var tokenstring = token.ToString().Remove(0, 7);

			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(tokenstring);

			bool a = decodedToken.TryGetValue("UserName", out string userName);
			bool b = decodedToken.TryGetValue("UserRole", out string userRole);

			if (a && b)
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

			return BadRequest();
		}

		return BadRequest();
	}
}
