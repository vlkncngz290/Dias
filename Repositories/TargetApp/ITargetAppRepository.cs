namespace MonitorTargetApp.Repositories.TargetApp
{
    public interface ITargetAppRepository
    {
        public IEnumerable<Models.TargetApp> Index();
        public Models.TargetApp Details(int id);

        public void Create(Models.TargetApp targetApp);

        public Models.TargetApp Edit(int id,Models.TargetApp targetApp);

        public void Delete(int id);
    }
}
