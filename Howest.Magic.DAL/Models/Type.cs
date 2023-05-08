﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Models
{
    public class Type
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CardType { get; set; }

        public virtual ICollection<CardType> CardTypes { get; set; } = new List<CardType>();
    }
}
