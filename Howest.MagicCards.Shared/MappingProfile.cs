using AutoMapper;
using Howest.MagicCards.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Card, CardDto>();
        }
    }
}
