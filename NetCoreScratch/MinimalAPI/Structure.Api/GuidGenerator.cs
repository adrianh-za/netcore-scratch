﻿namespace Structure.Api;

public class GuidGenerator
{
    public string GenerateGuid() => Guid.NewGuid().ToString();
}