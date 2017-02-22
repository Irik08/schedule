using System.Collections.Generic;

namespace schedule.Models
{
    public class AddressTimeSlot
    {
        public AddressTimeSlot(List<string> addressSlots, List<string> addressZones, List<string> timeSlots)
        {
            TimeSlots = timeSlots;
            AddressSlots = addressSlots;
            AddressZones = addressZones;
        }

        public List<string> TimeSlots { get; private set; }

        public List<string> AddressSlots { get; private set; }

        public List<string> AddressZones { get; private set; }
    }
}
