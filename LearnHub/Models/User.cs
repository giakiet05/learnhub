using LearnHub.Models;
using System.Collections.Generic;
using System;

public class User
{
    [Key]
    public string Username { get; set; }  // Primary Key
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Ethnicity { get; set; }
    public string Religion { get; set; }
    public string IDNumber { get; set; }

    public ICollection<Student> Students { get; set; }
    public ICollection<Teacher> Teachers { get; set; }
}
