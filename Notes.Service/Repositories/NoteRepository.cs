using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Notes.Service.DTO;
using Notes.Service.Entity;

public class NoteRepository: INoteRepository
{
    private readonly NotesDbContext _dbContext;
    private readonly IMapper _notesMapper;
    public NoteRepository(NotesDbContext dbContext,IMapper notesMapper)
    {
        _dbContext=dbContext;
        _notesMapper = notesMapper;
    }
    public async Task<List<NoteDTO>> GetNotesAsync(int page,int pageSize)
    {
        return await _dbContext.Notes
                        .AsNoTracking()
                        .OrderBy(x=>x.Title)
                        .Skip((page-1)*pageSize)
                        .Take(pageSize)
                        .ProjectTo<NoteDTO>(_notesMapper.ConfigurationProvider)
                        .ToListAsync();
    }
    public async Task CreateNewNote(Note note)
    {
        await _dbContext.Notes.AddAsync(note);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteNoteAsync(Guid id)
    {
        var note = await _dbContext.Notes.FindAsync(id);
        
        if(note!=null) _dbContext.Remove(note);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateNote(Note note)
    {
        
            var ex_note = await _dbContext.Notes.FindAsync(note.Id);
            if(ex_note!=null){
                ex_note.Body=note.Body;
                ex_note.Title=note.Title;
                await _dbContext.SaveChangesAsync();
            }
    }
    
}