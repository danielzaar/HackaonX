using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackathonX.DB.Model;
using Microsoft.EntityFrameworkCore;

namespace HackathonX.DB.Repositories
{
    public class QuestionnaireRepository : IDisposable
    {
        private HackathonXContext m_DbContext;

        public QuestionnaireRepository(HackathonXContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public void Dispose()
        {
            m_DbContext.Dispose();
        }

        public async Task<IEnumerable<Question>> GetQuestionnaire(int takeTop = 10)
        {
            var t = await m_DbContext.Questions
                .Include(x => x.Answers).ToListAsync();
            return null;
        }

        //private async IList<Question> buisnessLogic()
        //{
        //    var something = await m_DbContext.Questions.GroupBy(x => x.Score).Select(y => new { k = y.Key, v = y.OrderBy(r => Guid.NewGuid()).Take(3) }).ToListAsync();
        //    return t;
        //}
    }
}
