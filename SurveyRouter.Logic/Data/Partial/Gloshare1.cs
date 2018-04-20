using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyRouter.Logic.Data
{
    public partial class GloshareDbContext
    {
        public List<RouterContactInsertsReturnModel> ExecuteRouterContactInserts(int daysInterval, out int numberInserted)
        {
            int procResult;
            List<RouterContactInsertsReturnModel> contacts =RouterContactInserts(daysInterval, out procResult);
            numberInserted = procResult;
            return contacts;
        }
    }
}
