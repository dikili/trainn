using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ohce.Models;

namespace Ohce
{
    public class OhceEngine
    {
        private readonly DateTime _date;
        private string _name;
        private bool _isAlive;

        public OhceEngine(string name = "", DateTime date = new DateTime())
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

            if (string.IsNullOrEmpty(query))
            {
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
}
