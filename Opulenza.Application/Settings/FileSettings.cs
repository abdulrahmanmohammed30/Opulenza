﻿namespace Opulenza.Application.Settings;

public class FileSettings
{
    public required int MaxFileSize { get; set; }
    public required string[] AllowedExtensions { get; set; }
}
