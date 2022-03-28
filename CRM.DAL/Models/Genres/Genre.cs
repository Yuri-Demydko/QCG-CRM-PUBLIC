using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.DAL.Models.Genres
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Title { get; set; }
    }
}