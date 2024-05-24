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
        public void TestMethod1()
        {
            Track NoFoundTrack = _trackService.GetOne("NoRole", "NoUserName", "TrackName");

            Assert.IsTrue(NoFoundTrack == null);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Track FoundTrack = _trackService.GetOne(userRole: "Instructor", "Aske", "awesome");

            Assert.IsTrue(FoundTrack != null);
            Assert.AreEqual(FoundTrack.Name, "awesome");
        }

        [TestMethod]
        public void TestMethod3()
        {
            Track TrackToBeDeleted = _trackService.GetOne(userRole: "Instructor", "Aske", "awesome");

            _trackService.DeleteTrack(TrackToBeDeleted);

            Track NoFoundTrack = _trackService.GetOne(userRole: "Instructor", "Aske", "awesome");

            Assert.IsTrue(NoFoundTrack == null);

            _trackService.SaveNewTrack(TrackToBeDeleted);

            Track FoundTrack = _trackService.GetOne(userRole: "Instructor", "Aske", "awesome");

            Assert.AreEqual(FoundTrack.Name, TrackToBeDeleted.Name);
            Assert.AreEqual(FoundTrack.Date, TrackToBeDeleted.Date);

        }

        [TestMethod]
        public void TestMethod4()
        {
            var exercises = _exerciseService.GetAllExercises();

            Assert.IsTrue(exercises != null);
            Assert.IsTrue(exercises.Any(e => e.Name == "Øvelse3"));
        }
    }
}