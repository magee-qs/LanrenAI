using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Filters
{
    public class LowerCaseContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {

            var properties = base.CreateProperties(type, memberSerialization);
            foreach (var property in properties)
            {
                property.PropertyName = Char.ToLowerInvariant(property.PropertyName[0]) + property.PropertyName.Substring(1);
            }

            return properties;
        }
    }
}
