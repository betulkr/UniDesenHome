using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unibim.Data.Model;

namespace Unibim.Melanom.WebUI.Models.EntityModel
{
    public class SepetUrunsViewModel
    {
        public SepetUrun sepetUrun { get; set; }
        public List<SepetUrunOzellik> sepetUrunOzelliks { get; set; }
    }
}