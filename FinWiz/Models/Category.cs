﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinWiz.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; } = "";


        [Column(TypeName = "nvarchar(20)")]
        public string Type { get; set; } = "Expense";

        [NotMapped]
        public string? TitleWithIcon
        {
            get
            {
                return this.Icon + " " + this.Title;
            }
        }
    }
}
