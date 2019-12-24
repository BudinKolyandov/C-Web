﻿using System;

namespace IRunes.Models
{
    public class Track
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }

    }
}