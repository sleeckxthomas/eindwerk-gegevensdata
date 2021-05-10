using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eindopdracht
{
    public class project
    {
        [Key]
        public int project_id { get; set; }
        public string naam { get; set; }
        public float uur { get; set; }

        public override string ToString()
        {
            return $"Naam project: {naam}. Aantal uren beschikbaar: {uur}";
        }
    }
}
