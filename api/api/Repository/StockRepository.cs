using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context;

    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<Stock>> GetAllAsync(StockQueryObject query)
    {
        var stocks = _context.Stocks.Include(stock => stock.Comments).ThenInclude(comment => comment.AppUser).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(stock => stock.Symbol.Contains(query.Symbol));
        }
        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(stock => stock.CompanyName.Contains(query.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDescending
                    ? stocks.OrderByDescending(stock => stock.Symbol)
                    : stocks.OrderBy(stock => stock.Symbol);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        // Here we are using Offset pagination - may have shortcomings with queries that return large amounts of data
        // see (https://learn.microsoft.com/en-us/ef/core/querying/pagination)
        // consider using Keyset pagination
        return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.Include(stock => stock.Comments).FirstOrDefaultAsync(stock => stock.Id == id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(stock => stock.Symbol == symbol);
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
        if (stockModel == null)
        {
            return null;
        }

        stockModel.Symbol = stockDto.Symbol;
        stockModel.CompanyName = stockDto.CompanyName;
        stockModel.Purchase = stockDto.Purchase;
        stockModel.LastDividend = stockDto.LastDividend;
        stockModel.Industry = stockDto.Industry;
        stockModel.MarketCap = stockDto.MarketCap;

        await _context.SaveChangesAsync();
        return stockModel;
    }   

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
        if (stockModel == null)
        {
            return null;
        }

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<bool> StockExistsAsync(int id)
    {
        return await _context.Stocks.AnyAsync(stock => stock.Id == id);
    }
    
}