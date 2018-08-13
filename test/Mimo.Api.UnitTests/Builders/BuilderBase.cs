namespace Mimo.Api.UnitTests.Builders
{
    public class BuilderBase<T>
    {
        protected T ObjectToBuild;

        public T Build()
        {
            return ObjectToBuild;
        }
    }
}