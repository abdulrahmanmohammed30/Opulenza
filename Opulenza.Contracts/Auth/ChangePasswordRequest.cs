﻿namespace Opulenza.Contracts.Auth;

public class ChangePasswordRequest
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}