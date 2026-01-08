namespace Notes.Service.DTO;
public record NoteDTO{
    public Guid? Id {get;init;}
    public string? Title {get;init;}
    public string? Body {get;init;}
}