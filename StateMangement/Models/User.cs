using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StateMangement.Models
{
    public class User
    {
        [Required]
        [RegularExpression(@"^[A-Z{1}]+[a-zA-z{1,30}]+$", ErrorMessage = "Please start your name with a capital letter and have only letters.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z{1}]+[a-zA-z{1,30}]+$", ErrorMessage = "Please enter a valid name.")]
        public string LastName { get; set; }

        //[Required]
        //[RegularExpression(@"^([A- Z])\w +\b[\w\.-] +@[\w\.-]+\.\w{2,4}\b+$", ErrorMessage = "That is an invalid email address.")]
        public string Email { get; set; }


        public string Password { get; set; }
        public string Age { get; set; }

       


        public User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public User() { }
    }
}