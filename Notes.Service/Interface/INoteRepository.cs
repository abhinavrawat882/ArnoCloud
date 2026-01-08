using Notes.Service.DTO;
using Notes.Service.Entity;

public interface INoteRepository
{
    // I changed 'internal' to 'public' so the Interface can be used by the Service
    Task<List<NoteDTO>> GetNotesAsync(int page, int pageSize);
    Task DeleteNoteAsync(Guid id);
    Task UpdateNote(Note note);
    Task CreateNewNote(Note note);
}