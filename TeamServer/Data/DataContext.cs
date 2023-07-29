using Microsoft.EntityFrameworkCore;
using TeamServer.Models;
using TeamServer.Models.Agents;
using TeamServer.Models.Listeners;

namespace TeamServer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Listener> Listeners => Set<Listener>();
        public DbSet<ListenerType> ListenerTypes => Set<ListenerType>();
        public DbSet<Agent> Agents => Set<Agent>();
        public DbSet<AgentTask> AgentTasks => Set<AgentTask>();
        public DbSet<AgentTaskResult> AgentTasksResult => Set<AgentTaskResult>();
        public DbSet<Implant> Implants => Set<Implant>();

    }
}
