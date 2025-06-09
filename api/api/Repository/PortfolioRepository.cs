using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly ApplicationDBContext _context;

    public PortfolioRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stock>> GetUserPortfolio(AppUser user)
    {
        return await _context.Portfolios.Where(portfolio => portfolio.AppUserId == user.Id).Select(stock => new Stock
        {
            Id = stock.StockId,
            Symbol = stock.Stock.Symbol,
            CompanyName = stock.Stock.CompanyName,
            LastDividend = stock.Stock.LastDividend,
            Purchase = stock.Stock.Purchase,
            MarketCap = stock.Stock.MarketCap,
            Industry = stock.Stock.Industry,
        }).ToListAsync();
    }
}