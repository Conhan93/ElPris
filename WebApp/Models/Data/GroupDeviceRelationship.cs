using System;
using System.Collections.Generic;

namespace CloudWebService.Models
{
    public partial class GroupDeviceRelationship
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public int? DeviceId { get; set; }

        public virtual Device? Device { get; set; }
        public virtual Group? Group { get; set; }
    }
}
