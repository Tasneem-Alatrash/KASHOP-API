using System;

namespace KASHOP.DAL.DTO.Request;

public class RegisterRequest
{
    public string Email { get; set;}
    public string password { get; set;}
    public string UserName { get; set;}
    public string FullName { get; set;} 
    public string PhoneNumber { get; set; }
}
