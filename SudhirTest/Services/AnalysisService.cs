using SudhirTest.Data;
using SudhirTest.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudhirTest.Services
{
    public interface IAnalysisService
    {
        dynamic LoadBhavCopy(object data);
        List<Instrument> GetInstrument();
    }
    public class AnalysisService : IAnalysisService
    {
        public readonly ApplicationDbContext _context;
        public AnalysisService(ApplicationDbContext context)
        {
            _context = context;
        }
        public dynamic LoadBhavCopy(object data)
        {
            throw new NotImplementedException();
        }
        public List<Instrument> GetInstrument()
        {
            return _context.Instrument.ToList();
        }
    }
}
