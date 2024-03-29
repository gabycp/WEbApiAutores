﻿using System.ComponentModel.DataAnnotations;

namespace WEbApiAutores.DTOs
{
    public class LibroCreacionDTO
    {
        [StringLength(maximumLength: 250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresId { get; set; }
    }
}
