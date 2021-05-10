using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eindopdracht
{
    public class wetenschapper
    {
        [Key]
        public int wetenschapper_id { get; set; }
        public string naam { get; set; }

        public override string ToString()
        {
            return $"{naam}";
        }
    }
}
