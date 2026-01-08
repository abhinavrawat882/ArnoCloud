namespace Notes.Service.DTO;
public record CreateNoteDTO{
    public string? Title {get;init;}
    public string? Body {get;init;}
}