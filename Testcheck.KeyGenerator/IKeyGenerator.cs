using System;
using System.Collections.Generic;


namespace TAlex.Testcheck.KeyGenerator
{
    public interface IKeyGenerator
    {
        object Generate(IDictionary<string, string> inputs);
    }
}
