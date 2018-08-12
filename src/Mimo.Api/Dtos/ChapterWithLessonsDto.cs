using System.Collections.Generic;

namespace Mimo.Api.Dtos
{
    public class ChapterWithLessonsDto:ChapterDto
    {

        public IList<LessonDto> Lessons { get; set; }
    }
}
