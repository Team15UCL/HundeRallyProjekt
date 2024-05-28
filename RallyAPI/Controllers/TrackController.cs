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

	[HttpGet("FindOne")]
	public ActionResult<Track> GetOne(string name, string date = "", string location = "", string theme = "")
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(token);

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

	[HttpGet("FindMany")]
	public ActionResult<IEnumerable<Track>> GetMany()
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(token);

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

	[HttpPost(Name = "CreateTrack")]
	public ActionResult<Track> Post(Track track)
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(token);

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

	[HttpPut(Name = "UpdateTrack")]
	public ActionResult Put(Track track)
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(token);

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

	[HttpDelete(Name = "DeleteTrack")]
	public ActionResult Delete(Track track)
	{
		if (Request.Headers.TryGetValue("Authorization", out var token))
		{
			var decodedToken = JwtBuilder.Create()
				.WithAlgorithm(new HMACSHA256Algorithm())
				.WithSecret(secrets)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(token);

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
