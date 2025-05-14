using System.Data;
using Application.Constants;
using Data.Context;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.ReadModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly FileUploaderContext _context;
    private readonly DbSet<Product> _dbSet;

    public ProductRepository(FileUploaderContext context)
    {
        _context = context;
        _dbSet = context.Products;
    }

    public async Task<IList<ProductWithExchange>> GetProductsWithExchange
        (string? name, DateOnly? expiration, uint page, uint pageSize, string orderBy, bool descendingOrder, CancellationToken cancellationToken)
    {   
        // ORDER
        var orderDirection = descendingOrder ? "DESC" : "ASC";

        // CONDITION
        var whereClauses = new List<string> { "1 = 1" }; 
        var sqlParams = new List<SqlParameter>();

        if (!string.IsNullOrWhiteSpace(name))
        {
            whereClauses.Add("Name LIKE @name");
            sqlParams.Add(new SqlParameter("@name", $"{name}%"));
        }

        if (expiration.HasValue)
        {
            whereClauses.Add("EXPIRATION = @expiration");
            sqlParams.Add(new SqlParameter("@expiration", expiration.Value));
        }

        // PAGINATION
        var offset = page * pageSize; 
        sqlParams.Add(new SqlParameter("@offset", (int)offset));
        sqlParams.Add(new SqlParameter("@pageSize", (int)pageSize));

        string sql = $@"
            WITH FILTERED_PRODUCTS AS (
                SELECT 
                    JOB_ID,
                    ID,
                    NAME,
                    PRICE,
                    EXPIRATION
                FROM PRODUCT WITH(NOLOCK)
                WHERE {string.Join(" AND ", whereClauses)}
                ORDER BY {orderBy} {orderDirection}
                OFFSET @offset ROWS
                FETCH NEXT @pageSize ROWS ONLY
            )
            SELECT 
                fp.ID,
                fp.NAME,
                fp.EXPIRATION,
                fp.PRICE AS PriceInUsd,
                MAX(CASE WHEN e.CURRENCY_CODE = 'BRL' THEN ROUND(fp.Price * e.RATE_TO_USD,2) END) AS PriceInBrl,
                MAX(CASE WHEN e.CURRENCY_CODE = 'EUR' THEN ROUND(fp.Price * e.RATE_TO_USD,2) END) AS PriceInEur,
                MAX(CASE WHEN e.CURRENCY_CODE = 'GBP' THEN ROUND(fp.Price * e.RATE_TO_USD,2) END) AS PriceInGbp,
                MAX(CASE WHEN e.CURRENCY_CODE = 'JPY' THEN ROUND(fp.Price * e.RATE_TO_USD,2) END) AS PriceInJpy,
                MAX(CASE WHEN e.CURRENCY_CODE = 'ARS' THEN ROUND(fp.Price * e.RATE_TO_USD,2) END) AS PriceInArs
            FROM FILTERED_PRODUCTS fp JOIN EXCHANGE e ON fp.JOB_ID = e.JOB_ID
            GROUP BY 
                fp.JOB_ID, 
                fp.ID, 
                fp.NAME, 
                fp.PRICE, 
                fp.EXPIRATION
        ";

        if (cancellationToken.IsCancellationRequested)
            return new List<ProductWithExchange>();

        var results = await _context.Database
            .SqlQueryRaw<ProductWithExchange>(sql, sqlParams.ToArray())
            .ToListAsync();
        return results;
    }

    public async Task CreateBatch(IList<Product> products)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            connection.Open();
            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = "PRODUCT",
                BatchSize = AppConstants.BATCH_SIZE,
                BulkCopyTimeout = 60
            };

            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("NAME", typeof(string));
            table.Columns.Add("PRICE", typeof(decimal));
            table.Columns.Add("EXPIRATION", typeof(DateOnly));
            table.Columns.Add("JOB_ID", typeof(int));

            var dataRows = new DataRow[products.Count()];

            for (var i = 0; i < products.Count(); i++)
            {
                var product = products[i];
                var id = Random.Shared.Next(1, int.MaxValue);
                dataRows[i] = table.Rows.Add(id, product.Name, product.Price, product.Expiration, product.JobId);
            }

            await bulkCopy.WriteToServerAsync(dataRows);
        }
    }

    public async Task DeleteAllFromJob(int jobId)
    {
        await _dbSet.Where(x => x.JobId == jobId).ExecuteDeleteAsync();
    }

}
