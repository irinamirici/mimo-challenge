using AutoMapper;
using Mimo.Api.Dtos;
using Mimo.Persistence.Entities;

namespace Mimo.Api.Mapping
{
    public class PersistenceToDtoProfile : Profile
    {
        public PersistenceToDtoProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<Course, CourseWithChaptersDto>()
                .ForMember(dest => dest.Chapters, opt => opt.MapFrom(x => x.Chapters));
            CreateMap<Chapter, ChapterDto>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(x => x.Course.Id));

            CreateMap<Chapter, ChapterWithLessonsDto>()
               .ForMember(dest => dest.CourseId, opt => opt.MapFrom(x => x.Course.Id))
               .ForMember(dest => dest.Lessons, opt => opt.MapFrom(x => x.Lessons));

            CreateMap<Lesson, LessonDto>()
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(x => x.Chapter.Id));

            CreateMap<UserAchievement, UserAchievementDto>()
                .ForMember(dest => dest.AchievementId, opt => opt.MapFrom(x => x.Achievement.Id))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(x => x.Course))
                .ForMember(dest => dest.AchievementType, opt => opt.MapFrom(x => x.Achievement.Type))
                .ForMember(dest => dest.AchievementName, opt => opt.MapFrom(x => x.Achievement.Name))
                .ForMember(dest => dest.Target, opt => opt.MapFrom(x => x.Achievement.Target));
        }
    }
}