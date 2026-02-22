using System.Reflection.Metadata.Ecma335;

namespace Workspace.Service.Entites;

public class Workspace
{
    public required Guid Id {get;set;}
    public required string Name{get;set;}
    public string? Description{get;set;}
}