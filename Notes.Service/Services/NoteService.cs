using System.Dynamic;
using AutoMapper;
using Notes.Service.DTO;
using Notes.Service.Entity;

namespace Notes.Service.Services;

public class NoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _notesMapper;
    public NoteService(INoteRepository noteRepository,IMapper notesMapper)
    {
        _noteRepository = noteRepository;
        _notesMapper=notesMapper;
    }
    public async Task<List<NoteDTO>> GetNotes(
        int page,
        int pageSize
    )
    {
        
        return await _noteRepository.GetNotesAsync(page,pageSize);
    }
    public async Task DeleteNote(Guid id)
    {
        await _noteRepository.DeleteNoteAsync(id);
    }
    public async Task UpdateNote(NoteDTO note)
    {
        var noteEntity = _notesMapper.Map<Note>(note);
        await _noteRepository.UpdateNote(noteEntity);
    }
    public async Task AddNewNoteAsync(CreateNoteDTO note)
    {
        var mappedNote = _notesMapper.Map<Note>(note);
        await _noteRepository.CreateNewNote(mappedNote);
    }
}