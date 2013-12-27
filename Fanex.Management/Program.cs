
namespace Fanex.Management
{
    using Fanex.ManagementSP.Client;
    using Fanex.ManagementSP.Client.Services.Impl;

    class Program
    {
        static void Main(string[] args)
        {
            ServerService cls = new ServerService();
            cls.GetAll();

            DatabaseService data = new DatabaseService();
            data.GetAll();

            StoreProcedureService store = new StoreProcedureService();
            store.GetByIssueId(1);

        }
    }
}
