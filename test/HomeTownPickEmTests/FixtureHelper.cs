using System.Linq;
using AutoFixture;

namespace HomeTownPickEm
{
    public class FixtureHelper
    {
        public static Fixture CreateDefaultFixture()
        {
            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture;
        }
    }
}