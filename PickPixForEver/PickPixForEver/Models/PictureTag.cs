﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PickPixForEver.Models
{
    public class PictureTag
    {
        public int PictureId { get; set; }
        public int TagId { get; set; }
        public Picture Picture { get; set; }
        public Tag Tag { get; set; }
    }
}
