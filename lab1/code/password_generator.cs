// criteria 
// Use the entire first name.
// Append 3 characters from the last name (if the last name is shorter than 3 characters, use all of it).
// Append 3 digits extracted from the registration number (digits are extracted in the order they appear; if fewer than 3 digits exist, the program warns and exits).
// Append 2 characters from the favorite movie (if the movie string is shorter than 2 characters, use all of it).
// Append 1 character from the favorite food (if empty, the program warns and exits).
using System;
using System.Linq;
namespace PasswordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string firstName = "fahad";
            string lastName = "Qasim";
            string registrationNumber = "sp22-bcs-034";
            string favMovie = "Intersteller";
            string favFood = "Dampukh";
            string part1 = firstName;
            string part2 = lastName.Length >= 3 ? lastName.Substring(0, 3) : lastName;
            string digits = new string(registrationNumber.Where(char.IsDigit).ToArray());
            if (digits.Length < 3)
            {
                Console.WriteLine("Error: Registration number does not contain at least 3 digits.");
                return;
            }
            string part3 = digits.Substring(0, 3);
            string part4 = favMovie.Length >= 2 ? favMovie.Substring(0, 2) : favMovie;
            if (string.IsNullOrEmpty(favFood))
            {
                Console.WriteLine("Error: Favorite food must contain at least 1 character.");
                return;
            }
            string part5 = favFood.Substring(0, 1);
            string password = part1 + part2 + part3 + part4 + part5;

            Console.WriteLine("Generated Password: " + password);
        }
    }
}
// output:
// Generated Password: fahadQas220InD