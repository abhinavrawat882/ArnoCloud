using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public record JwtOption{
    public string Key{get; init;}
    public string Issuer{get; init;}
    public string Audience{get;init;}
    }