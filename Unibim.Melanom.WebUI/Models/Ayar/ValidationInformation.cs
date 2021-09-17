using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Unibim.Melanom.WebUI
{
    [DataContract]
    public class ValidationInformation : IEnumerable<ValidationItem>
    {
        [DataMember]
        private List<ValidationItem> errors = new List<ValidationItem>();

        public void AddError(string key, string errorMessage)
        {
            errors.Add(new ValidationItem() { Key = key, ValidationMessage = errorMessage });
        }

        IEnumerator<ValidationItem> IEnumerable<ValidationItem>.GetEnumerator()
        {
            return this.errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.errors.GetEnumerator();
        }
    }

    [DataContract]
    public class ValidationItem
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string ValidationMessage { get; set; }
    }
}
