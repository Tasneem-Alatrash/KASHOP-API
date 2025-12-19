using System;


namespace KASHOP.DAL.DTO.Response;

public class LoginResponse : BaseResponse
{
    public string? AccessToken { get; set; }
}
