﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Banners")]
    public class Banner : BaseModel
    {
        [Key]
        public int BAN_ID { get; set; }
        public required string Title { get; set; }
        public required string Image { get; set; }
        public string? Url { get; set; }
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
