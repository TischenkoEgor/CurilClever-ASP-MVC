using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.ViewModels;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Remotion.Linq.Parsing.Structure;
using Microsoft.EntityFrameworkCore.Query;

namespace CurilClever2.Models
{
  public class CleverDBContext : DbContext
  {
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderComment> OrderComments { get; set; }
    public DbSet<ClientComment> ClientComments { get; set; }
    public DbSet<CaptureModel> CaptureModels { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Subscribe> Subscribes { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<VkUserID> vkUserIDs { get; set; }

    public CleverDBContext(DbContextOptions<CleverDBContext> options)
           : base(options)
    {
      Database.EnsureCreated();
    }
  }
  public static class IQueryableExtensions
  {
    private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

    private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

    private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");

    private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");

    private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

    private static readonly FieldInfo QueryCompilationContextFactoryField = typeof(CleverDBContext).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompilationContextFactory");

    public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
    {
      if (!(query is EntityQueryable<TEntity>) && !(query is InternalDbSet<TEntity>))
      {
        throw new ArgumentException("Invalid query");
      }

      var queryCompiler = (IQueryCompiler)QueryCompilerField.GetValue(query.Provider);
      var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
      var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
      var queryModel = parser.GetParsedQuery(query.Expression);
      var database = DataBaseField.GetValue(queryCompiler);
      var queryCompilationContextFactory = (IQueryCompilationContextFactory)QueryCompilationContextFactoryField.GetValue(database);
      var queryCompilationContext = queryCompilationContextFactory.Create(false);
      var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
      modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
      var sql = modelVisitor.Queries.First().ToString();

      return sql;
    }
  } 
}
