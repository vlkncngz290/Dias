using Microsoft.EntityFrameworkCore;
using MonitorTargetApp.Data;

namespace MonitorTargetApp.Repositories.TargetApp
{
    public class TargetAppRepository : ITargetAppRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TargetAppRepository> _logger;

        public TargetAppRepository(ApplicationDbContext context, ILogger<TargetAppRepository> logger)
        {
            _context = context;
        }

        public Models.TargetApp Details(int id)
        {
            if (id == null)
            {
                return null;
            }

            var targetApp = _context.TargetApp
                .FirstOrDefault(m => m.Id == id);
            if (targetApp == null)
            {
                return null;
            }
            return targetApp;
        }

        public IEnumerable<Models.TargetApp> Index()
        {
            List<Models.TargetApp> targetApps = new List<Models.TargetApp>();
            targetApps = _context.TargetApp.ToList();
            return targetApps;
        }

        public void Create(Models.TargetApp targetApp)
        {
            _context.Add(targetApp);
            _context.SaveChanges();
        }

        public Models.TargetApp Edit(int id, Models.TargetApp targetApp)
        {
            try
            {
                _context.Update(targetApp);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TargetAppExists(targetApp.Id))
                {
                    return null;
                }
                else
                {
                    _logger.LogError("\n\n\nError Message:\n" + ex.Message.ToString() + "\n\n Target App Model Exists but cant update");
                }
            }
            return targetApp;
        }

        public void Delete(int id)
        {
            var targetApp = _context.TargetApp.Find(id);
            _context.Remove(targetApp);
            _context.SaveChanges();
        }

        private bool TargetAppExists(int id)
        {
            return _context.TargetApp.Any(e => e.Id == id);
        }
    }
}
