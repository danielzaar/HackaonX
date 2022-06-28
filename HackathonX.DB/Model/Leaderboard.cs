using System;
using System.Collections.Generic;

namespace HackathonX.DB.Model
{
    public partial class Leaderboard
    {
        public Guid UserId { get; set; }
        public float Score { get; set; }
        public float Time { get; set; }
        public byte[] Timestamp { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
