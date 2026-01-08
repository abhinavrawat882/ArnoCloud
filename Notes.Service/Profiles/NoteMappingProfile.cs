using AutoMapper;
using Notes.Service.DTO;
using Notes.Service.Entity;

namespace Notes.Service.Profiles
{
    // This class can be Internal because only this project needs to know HOW to map.
    internal class NoteMappingProfile : Profile
    {
        public NoteMappingProfile()
        {
            // 1. Map Creation DTO -> Entity
            // usage: _mapper.Map<Note>(createDto);
            CreateMap<NoteDTO, Note>();
            //.ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Content));
                
            
            // 2. Map Entity -> Response DTO
            // usage: _mapper.Map<NoteDto>(noteEntity);
            // ReverseMap() creates the map in the other direction automatically!
            CreateMap<Note, NoteDTO>().ReverseMap();
            //.ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Content));

            CreateMap<CreateNoteDTO, Note>();
        }
    }
}