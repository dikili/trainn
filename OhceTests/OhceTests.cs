using System.Data;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Ohce;

namespace OhceTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void When_Name_Date_Are_Provided_Ohce_CanStart()
        {
            //Arrange
            var ohceEngine = new OhceEngine("userName", DateTime.Now);

            //Act
            var ohceResponse = ohceEngine.Execute();

            //Assert
            Assert.IsNotNull(ohceResponse);
            Assert.IsTrue(ohceResponse.IsAlive);
        }

        [Test]
        public void When_Name_Is_NOT_Provided_Ohce_CanNOTStart()
        {
            //Arrange
            var ohceEngine = new OhceEngine();
            //Act
            var ohceResponse = ohceEngine.Execute("my query");
            //Assert
            Assert.IsEmpty(ohceResponse.Message);
            Assert.IsFalse(ohceResponse.IsAlive);
        }

        [Test]
        public void MorningGreets_With_Name_StartsCorrectly()
        {
            //Arrange
            var ts = new TimeSpan(10, 30, 0);
            var dateTimeSet = DateTime.Today.Add(ts);
            var userName = "Elon";
            var ohceEngine = new OhceEngine(userName, dateTimeSet);

            //Act
            var ohceResponse = ohceEngine.Execute();

            //Assert
            StringAssert.AreEqualIgnoringCase( $"¡Buenos días {userName}!", ohceResponse.Message);
        }

        [Test]
        public void NightGreets_With_Name_StartsCorrectly()
        {
            //Arrange
            var ts = new TimeSpan(21, 55, 0);
            var dateTimeSet = DateTime.Today.Add(ts);
            var userName = "Elon";
            var ohceEngine = new OhceEngine(userName, dateTimeSet);

            //Act
            var ohceResponse = ohceEngine.Execute();

            //Assert
            StringAssert.AreEqualIgnoringCase( $"¡Buenas noches {userName}!", ohceResponse.Message);
        }

        [Test]
        public void AfternoonGreets_With_Name_StartsCorrectly()
        {
            //Arrange
            var ts = new TimeSpan(12, 55, 0);
            var dateTimeSet = DateTime.Today.Add(ts);
            var userName = "Elon";
            var ohceEngine = new OhceEngine(userName, dateTimeSet);

            //Act
            var ohceResponse = ohceEngine.Execute();

            //Assert
            StringAssert.AreEqualIgnoringCase($"¡Buenas tardes {userName}!", ohceResponse.Message);
        }

        [Test]
        public void When_Input_Is_Palindrome_Ohce_CanRecognize()
        {
            //Arrange
            var ohceEngine = new OhceEngine("userName", DateTime.Now);

            //Act
            var ohceResponse = ohceEngine.Execute("madam");

            //Assert
            StringAssert.AreEqualIgnoringCase($"madam ¡Bonita palabra!", ohceResponse.Message);
        }

        [Test]
        public void Ohce_Can_Reverse_Pharases()
        {
            //Arrange
            var ohceEngine = new OhceEngine("userName", DateTime.Now);

            //Act
            var ohceResponse = ohceEngine.Execute("Hello");

            //Assert
            StringAssert.AreEqualIgnoringCase($"olleH", ohceResponse.Message);
        }

        [Test]
        public void Multiple_Word_Phrases_Can_BeReversed()
        {
            //Arrange
            var ohceEngine = new OhceEngine("userName", DateTime.Now);

            //Act
            var ohceResponse = ohceEngine.Execute("Hello Table Now");

            //Assert
            StringAssert.AreEqualIgnoringCase($"woN elbaT olleH", ohceResponse.Message);
        }

        [Test]
        public void Ohce_Can_Stop()
        {
            //Arrange
            var userName = "Elon";
            var ohceEngine = new OhceEngine(userName);
            //Act
            var ohceResponse = ohceEngine.Execute("Stop!");
            //Assert
            StringAssert.AreEqualIgnoringCase( $"Adios {userName}!", ohceResponse.Message);
        }

    }
}