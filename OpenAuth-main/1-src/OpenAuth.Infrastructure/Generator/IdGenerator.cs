using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace OpenAuth
{
    public class IdGenerator
    {
        public static void SetIdGenerator(IdGeneratorOptions options)
        {
            YitIdHelper.SetIdGenerator(options);
        }

        public static void SetIdGenerator()
        {
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions());
        }

        public static long NextId()
        {
            return YitIdHelper.NextId();
        }
    }
}
