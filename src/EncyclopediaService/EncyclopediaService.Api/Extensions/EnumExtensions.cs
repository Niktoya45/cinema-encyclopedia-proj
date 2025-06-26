using Shared.CinemaDataService.Models.Flags;
using System.Security.Policy;

namespace EncyclopediaService.Api.Extensions
{
    public static class EnumExtensions
    {
        public static IList<string> BreakJobsFlag(this Job ev) 
        {
            IList<string> res = new List<string>();

            uint evi = (uint)ev;
            uint evk = 1;

            while (evi != 0)
            {
                if(evi%2 == 1)
                { 
                    res.Add(((Job)evk).ToString());
                }
                evi >>= 1;
                evk  *= 2;
            }

            return res;
        }

        public static IList<string> BreakGenresFlag(this Genre ev)
        {
            IList<string> res = new List<string>();

            uint evi = (uint)ev;
            uint evk = 1;

            while (evi != 0)
            {
                if (evi % 2 == 1)
                {
                    res.Add(((Genre)evk).ToString());
                }
                evi >>= 1;
                evk *= 2;
            }

            return res;
        }
    }
}
