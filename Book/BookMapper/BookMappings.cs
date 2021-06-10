using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.Dtos;
using Book.Models;

namespace Book.BookMapper
{
    public class BookMappings : Profile
    {
        public BookMappings()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
        }
    }
}
