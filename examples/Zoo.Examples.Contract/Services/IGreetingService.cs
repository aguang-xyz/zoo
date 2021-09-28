using Zoo.Rpc.Abstractions.Attributes;

namespace Zoo.Examples.Contract.Services
{
    [Version("1.0.1")]
    public interface IGreetingService
    {
        string SayHi(string name);
    }
}