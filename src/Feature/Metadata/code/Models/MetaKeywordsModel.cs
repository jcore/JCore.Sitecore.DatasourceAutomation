using System.Collections.Generic;
using System.Text;

namespace Car.Feature.Metadata.Models
{
    public class MetaKeywordsModel
    {
        public IEnumerable<string> Keywords { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            var result = string.Join(",", this.Keywords);
            return result;
        }
    }
}