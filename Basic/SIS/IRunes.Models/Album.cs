using System;
using System.Collections.Generic;

namespace IRunes.Models
{
    public class Album
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price { get; set; }

        public ICollection<Track> Tracks { get; set; } = new List<Track>();

    }
}
