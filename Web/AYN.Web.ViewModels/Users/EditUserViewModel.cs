using System.Collections.Generic;

namespace AYN.Web.ViewModels.Users
{
    public class EditUserViewModel
    {
        public EditUserGeneralInfoViewModel EditUserGeneralInfoViewModel { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Towns { get; set; }
    }
}
