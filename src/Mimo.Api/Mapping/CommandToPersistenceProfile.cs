using AutoMapper;
using Mimo.Api.Commands;
using Mimo.Persistence.Entities;

namespace Mimo.Api.Mapping
{
    public class CommandToPersistenceProfile : Profile
    {
        public CommandToPersistenceProfile()
        {
            CreateMap<CreateCourseCommand, Course>()
                .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(v => false))
                .ForMember(dest => dest.Chapters, opt => opt.Ignore());
            CreateMap<UpdateCourseCommand, Course>()
                .ForMember(dest => dest.IsPublished, opt => opt.Ignore())
                .ForMember(dest => dest.Chapters, opt => opt.Ignore());

            CreateMap<CreateChapterCommand, Chapter>()
                .ForMember(d => d.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Lessons, opt => opt.Ignore());
            CreateMap<UpdateChapterCommand, Chapter>()
                .ForMember(d => d.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Lessons, opt => opt.Ignore());

            CreateMap<CreateLessonCommand, Lesson>()
               .ForMember(d => d.Chapter, opt => opt.Ignore());

            CreateMap<UpdateLessonCommand, Lesson>()
               .ForMember(d => d.Chapter, opt => opt.Ignore());
        }
    }
}