namespace Mimo.Api.Commands
{
    public class DeleteChapterCommand
    {
        public int ChapterId { get; }

        public DeleteChapterCommand(int chapterId)
        {
            ChapterId = chapterId;
        }
    }
}
