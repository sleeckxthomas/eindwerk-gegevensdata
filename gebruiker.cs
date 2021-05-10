using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace eindopdracht
{
    public class gebruiker
    {
        [Key]
        public int gebruiker_id { get; set; }
        public string naam { get; set; }
        public string paswoord { get; set; }
    }
}
