﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SwaggerSchema(ReadOnly = true)]
        public DateTime create_date { get; set; }

        [Required]
        [StringLength(100)]
        public string firstname { get; set; }

        [Required]
        [StringLength(100)]
        public string lastname { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        [Required]
        [StringLength(255)]
        public string password { get; set; }

        public string? token { get; set; }

        public string? refresh_token { get; set; }

        public DateTime? refresh_token_expiry_time { get; set; }

    }
}