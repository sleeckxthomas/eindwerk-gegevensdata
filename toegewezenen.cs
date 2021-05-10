using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eindopdracht
{
    public class toegewezenen
    {
        [Key]
        public int toegewezen_id { get; set; }
        public int wetenschapper_id { get; set; }
        public int project_id { get; set; }
    }
}
