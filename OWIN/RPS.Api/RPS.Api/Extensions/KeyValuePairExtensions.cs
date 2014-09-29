using System.Collections.Generic;

namespace RPS.Api.Extensions
{
    public static class KeyValuePairExtensions
    {
         public static bool IsDefault<T, T2>(this KeyValuePair<T, T2> self)
         {
             return self.Equals(default(KeyValuePair<T, T2>));
         }
    }
}