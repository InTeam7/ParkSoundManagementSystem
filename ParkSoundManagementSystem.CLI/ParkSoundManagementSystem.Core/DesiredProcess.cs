using System;

namespace ParkSoundManagementSystem.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class DesiredProcess : IComparable<DesiredProcess>
    {
        public DesiredProcess(string name, int pId, long memory)
        {
            Name = name;
            PId = pId;
            Memory = memory;
        }
        public DesiredProcess(string name, int pId)
        {
            Name = name;
            PId = pId;
        }
        public string Name { get; set; }
        public int PId { get; set; }
        public long Memory { get; set; }

        public int CompareTo(DesiredProcess other)
        {
            return Memory.CompareTo(other.Memory);
        }
    }
}
