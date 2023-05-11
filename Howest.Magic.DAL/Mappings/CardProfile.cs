using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared
{
    public class CardProfile: Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardDto>();
        }
    }
}
