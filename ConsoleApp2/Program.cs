using SimpleOrmApplication.Dapper;
using SimpleOrmApplication.Linq2Db;

Console.WriteLine("Hello, World!");

//await Linq2DbExperiments.UserCrud();
//await Linq2DbExperiments.OrderCrud();
//await Linq2DbExperiments.DeferredExecution();

//await DapperExperiments.UserCrud();
await DapperExperiments.OrderCrud();
await DapperExperiments.Join();
