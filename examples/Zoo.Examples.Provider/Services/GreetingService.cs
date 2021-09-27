using Zoo.Examples.Contract.Services;

namespace Zoo.Examples.Provider.Services
{
    public class GreetingService : IGreetingService
    {
        public string SayHi(string name)
        {
            return $"Hi, {name}!";
        }
    }
}