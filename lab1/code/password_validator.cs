using System;
using System.Text.RegularExpressions;

namespace PasswordValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            string pattern = @"^(?=.{1,12}$)"               
                           + @"(?=(?:.*[2034]){2,})"         
                           + @"(?=.*[A-Z])"                 
                           + @"(?=(?:.*[a-z]){2,})"          
                           + @"(?=(?:.*[^A-Za-z0-9]){2,})"    
                           + @"(?=(?:.*[fahdqsim]){4,})"      
                           + @"[A-Za-z].*$";                
            string[] passwords = {
                "A2@b@fahd",    
                "A12a@#fahd",   
                "Bbcdef@2",     
                "F@aH2@dqs",    
                "NoSpecials123",
                "start@#$Fahd", 
                "A2@fad2#h"     
            };
            foreach (string password in passwords)
            {
                bool isValid = Regex.IsMatch(password, pattern);
                Console.WriteLine($"Password: {password} is {(isValid ? "Valid" : "Invalid")}");
            }
        }
    }
}
// output :
/*
Password: A2@b@fahd is Invalid
Password: A12a@#fahd is Invalid
Password: Bbcdef@2 is Invalid
Password: F@aH2@dqs is Invalid
Password: NoSpecials123 is Invalid
Password: start@#$Fahd is Invalid
Password: A2@fad2#h is Valid
*/ 