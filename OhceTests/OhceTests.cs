using System.Data;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

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

    public class OhceEngine
    {
        private readonly DateTime _date;
        private string _name;
        private bool _isAlive;

        public OhceEngine(string name ="", DateTime date = new DateTime())
        {
            _date = date;
            _name = name;
            _isAlive = !string.IsNullOrEmpty(name);
        }

        public OhceResponse Execute(string query = "")
        {
            if (string.IsNullOrEmpty(_name)) return new OhceResponse { IsAlive = false, Message = string.Empty };
            if (query.Contains("Stop!"))
            {
                return new OhceResponse
                {
                    IsAlive = false,
                    Message = "Adios " + _name + "!"
                };
            }

            var ohceResponse = new OhceResponse
            {
                IsAlive = true
            };

            if (string.IsNullOrEmpty(query)){
                var hourOfTheDay = Convert.ToInt32(_date.ToShortTimeString().Split(":").First());

                ohceResponse.IsAlive = true;

                ohceResponse.Message = hourOfTheDay switch
                {
                    >= 12 and < 20 => "¡Buenas tardes " + _name + "!",
                    >= 6 and < 12 => "¡Buenos días " + _name + "!",
                    _ => "¡Buenas noches " + _name + "!"
                };

            }
            else
            {
                if (isPalindrome(query))
                {
                    ohceResponse.Message = $"{query} ¡Bonita palabra!";
                }
                else
                {
                    string[] words = query.Split(' ');
                    Array.Reverse(words);
                    string reversedPhrase = "";
                    foreach (string word in words)
                    {
                       
                        char[] charArray = word.ToCharArray();
                        Array.Reverse(charArray);
                        reversedPhrase += new string(charArray) + " ";
                    }
                    ohceResponse.Message = reversedPhrase.Trim();

                }
            }
            return ohceResponse;
        }

        private bool isPalindrome(string query)
        {
            var cleanedWord = new string(Array.FindAll(query.ToLower().ToCharArray(), char.IsLetterOrDigit));

            var left = 0;
            var right = cleanedWord.Length - 1;

            while (left < right)
            {
                if (cleanedWord[left] != cleanedWord[right])
                    return false;

                left++;
                right--;
            }

            return true;
        }
    }

    public class OhceResponse
    {
        public bool IsAlive { get; set; }
        public string Message { get; set; }
    }
}