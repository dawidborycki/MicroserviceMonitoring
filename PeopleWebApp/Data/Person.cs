using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PeopleWebApp.Data
{
    public class Person
    {        
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [DisplayName("First name")]
        [Required]        
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [DisplayName("Birth date")]
        public DateTime BirthDate { get; set; }

        
        public int Age
        {
            get
            {
                var dayDiff = (DateTime.Now - BirthDate).TotalDays;

                return Convert.ToInt32(dayDiff / 365);
            }
        }
    }
}
