using System;
using System.Collections.Generic;

namespace CloudWebService.Models
{
    public partial class Device
    {
        public Device()
        {
            GroupDeviceRelationships = new HashSet<GroupDeviceRelationship>();
            Measurements = new HashSet<Measurement>();
        }

        public int Id { get; set; }
        public string? Adress { get; set; }
        public string? Alias { get; set; }

        public virtual ICollection<GroupDeviceRelationship> GroupDeviceRelationships { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; }
    }
}
