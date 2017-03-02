using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Model.Propaganda
{
    public class AdverExtension:Propaganda_Advertising
    {
        public IList<Propaganda_AdverContent> Content { get; set; }
    }
}
