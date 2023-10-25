using LibrarySystem.Constants.Enum;
using LibrarySystem.Entities;
using System;
using LibrarySystem.Managers.Authors.Dtos;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Managers.Books.Dtos
{
    [AutoMapTo(typeof(Book))]
    public class BookDto : EntityDto<long>
    {
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
        public AuthorDto Author { get; set; }
    }

    [AutoMapTo(typeof(Book))]
    public class CreateBookDto 
    {
        [Required]
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }

        public long AuthorId { get; set; }
    }

    [AutoMapTo(typeof(Book))]
    public class UpdateBookDto : EntityDto<long>
    {
        [Required]
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }

        public long AuthorId { get; set; }
    }

}
