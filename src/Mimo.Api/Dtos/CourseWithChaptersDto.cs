using System.Collections.Generic;

namespace Mimo.Api.Dtos
{
    public class CourseWithChaptersDto : CourseDto
    {
        public IList<ChapterDto> Chapters { get; set; }
    }
}
