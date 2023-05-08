﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Models
{
    public class Artist
    {
        public long Id { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
