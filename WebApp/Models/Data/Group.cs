using System;
using System.Collections.Generic;

namespace CloudWebService.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupDeviceRelationships = new HashSet<GroupDeviceRelationship>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<GroupDeviceRelationship> GroupDeviceRelationships { get; set; }
    }
}
