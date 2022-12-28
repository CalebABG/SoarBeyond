using AutoMapper;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Dto;

namespace SoarBeyond.Domain.EntityMapping;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        CreateMap<JournalEntity, Journal>()
            .ReverseMap();

        CreateMap<MomentEntity, Moment>()
            .ReverseMap();

        CreateMap<NoteEntity, Note>()
            .ReverseMap();

        CreateMap<ReflectionEntity, Reflection>()
            .ReverseMap();

        CreateMap<CategoryEntity, Category>()
            .ReverseMap();
    }
}