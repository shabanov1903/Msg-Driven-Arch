using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications
{
    [Flags]
    public enum Accepted
    {
        Rejected = 0,
        Kitchen = 1,
        Booking = 2,
        All = Kitchen | Booking
    }
}
