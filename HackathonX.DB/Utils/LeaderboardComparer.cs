using System.Diagnostics.CodeAnalysis;
using HackathonX.DB.Model;

namespace HackathonX.DB.Utils
{
    internal class LeaderboardComparer : IEqualityComparer<Leaderboard>
    {
        public bool Equals(Leaderboard? x, Leaderboard? y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.UserId == y.UserId)
                return true;
            else
                return false;
        }

        public int GetHashCode([DisallowNull] Leaderboard obj)
        {
            Guid hCode = obj.UserId;
            return hCode.GetHashCode();
        }
    }
}
