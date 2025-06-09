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
            Comments = stock.Stock.Comments
        }).ToListAsync();
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolioModel)
    {
        await _context.Portfolios.AddAsync(portfolioModel);
        await _context.SaveChangesAsync();
        return portfolioModel;
    }

    public async Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol)
    {
        var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(portfolio =>
            portfolio.AppUserId == appUser.Id && portfolio.Stock.Symbol.ToLower() == symbol.ToLower());

        if (portfolioModel == null)
        {
            return null;
        }

        _context.Portfolios.Remove(portfolioModel);
        await _context.SaveChangesAsync();
        return portfolioModel;
    }
}