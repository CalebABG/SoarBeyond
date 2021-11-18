using AutoMapper;
using SoarBeyond.Data.Entities;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.EntityMapping;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        CreateMap<JournalEntity, Journal>()
            .ReverseMap();

        CreateMap<JournalEntryEntity, JournalEntry>()
            .ReverseMap();

        CreateMap<ThoughtEntity, Thought>()
            .ReverseMap();
    }
}