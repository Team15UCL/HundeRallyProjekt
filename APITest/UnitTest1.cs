using RallyAPI.Data;
using RallyAPI.Models;
using RallyAPI.Models.Services;

namespace APITest
{
	[TestClass]
	public class UnitTest1
	{
		private readonly TrackService _trackService;
		private readonly ExerciseService _exerciseService;

		public UnitTest1()
		{
			// Initialize dependencies in the constructor
			var dbTrack = new DbAccess<Track>();
			var dbExercise = new DbAccess<Exercise>();
			_trackService = new TrackService(dbTrack);
			_exerciseService = new ExerciseService(dbExercise);
		}

		[TestMethod]
		public void TestNoTrackWithNameInDb()
		{
			Track NoFoundTrack = _trackService.GetOne("NoRole", "NoUserName", "WrongTrackName");

			Assert.IsTrue(NoFoundTrack == null);
		}

		[TestMethod]
		public void TestGettingTrackFromDb()
		{
			string SearchName = "test0";
			string UserName = "Bob";
			string UserRole = "Instructor";

			Track FoundTrack = _trackService.GetOne(UserRole, UserName, SearchName);

			Assert.IsTrue(FoundTrack != null);
			Assert.AreEqual(FoundTrack.Name, SearchName);
			Assert.IsTrue(FoundTrack.UserClaims.Contains(UserName));
			Assert.IsTrue(FoundTrack.RoleClaims.Contains(UserRole));
		}

		[TestMethod]
		public void TestDeletingAndCreatingTrackInDb()
		{
			string SearchName = "test0";
			string UserName = "Bob";
			string UserRole = "Instructor";

			Track TrackToBeDeleted = _trackService.GetOne(UserRole, UserName, SearchName);

			_trackService.DeleteTrack(TrackToBeDeleted);

			Track NoFoundTrack = _trackService.GetOne(UserRole, UserName, SearchName);

			Assert.IsTrue(NoFoundTrack == null);

			_trackService.SaveNewTrack(TrackToBeDeleted);

			Track FoundTrack = _trackService.GetOne(UserRole, UserName, SearchName);

			Assert.AreEqual(FoundTrack.Name, TrackToBeDeleted.Name);
			Assert.AreEqual(FoundTrack.Date, TrackToBeDeleted.Date);
		}

		[TestMethod]
		public void TestUpdateTrackNameInDb()
		{
			string SearchName = "test0";
			string UserName = "Bob";
			string UserRole = "Instructor";

			Track TrackToBeUpdated = _trackService.GetOne(UserRole, UserName, SearchName);

			TrackToBeUpdated.Name = "ændret";

			_trackService.UpdateTrack(TrackToBeUpdated);

			Track UpdatedTrackFromDb = _trackService.GetOne(UserRole, UserName, TrackToBeUpdated.Name);
			Assert.AreEqual(UpdatedTrackFromDb.Name, "ændret");
			Assert.AreEqual(UpdatedTrackFromDb.Id, TrackToBeUpdated.Id);

			TrackToBeUpdated.Name = SearchName;

			Track RevertedTrack = _trackService.UpdateTrack(TrackToBeUpdated);
			Assert.AreEqual(RevertedTrack.Name, SearchName);
		}

		[TestMethod]
		public void TestGetAllExercisesFromDb()
		{
			var exercises = _exerciseService.GetAllExercises();

			Assert.IsTrue(exercises != null);
			Assert.IsTrue(exercises.Any(e => e.Name == "Øvelse3"));
		}
	}
}