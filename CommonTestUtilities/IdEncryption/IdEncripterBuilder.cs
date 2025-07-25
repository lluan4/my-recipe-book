using Sqids;

namespace CommonTestUtilities.IdEncryption;
public class IdEncripterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "LbNCQzTuDfKj1m6q09dEox7XRPiJIhMV4tlBFSAypUH823rwencG5kvagOZYsW"
        });
    }
}