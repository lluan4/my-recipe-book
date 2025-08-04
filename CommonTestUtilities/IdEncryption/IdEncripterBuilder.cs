using Sqids;

namespace CommonTestUtilities.IdEncryption;
public class IdEncripterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "rOvmjkATPy2xsp8WbouJKBZw9Q1ftVNCYSgDi5GqH4hlacIU7FnEeMdRzX63L0"
        });
    }
}