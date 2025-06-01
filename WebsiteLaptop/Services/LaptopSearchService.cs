using FuzzySharp;
using WebsiteLaptop.Models;
using WebsiteLaptop.Data;

namespace WebsiteLaptop.Services
{
    public class LaptopSearchService
    {
        private readonly WebsiteLaptopContext _context;

        public LaptopSearchService(WebsiteLaptopContext context)
        {
            _context = context;
        }

        public List<Laptop> FuzzySearch(string keyword, int threshold = 70)
        {
            keyword = keyword?.ToLower().Trim() ?? "";

            var laptops = _context.Laptop
                .Where(l => !l.DaXoa)
                .ToList();

            var matched = laptops
                .Select(l => new
                {
                    Laptop = l,
                    Score = Math.Max(
                        Fuzz.Ratio(keyword, l.TenLaptop?.ToLower() ?? ""),
                        Fuzz.Ratio(keyword, l.HangSanXuat?.ToLower() ?? "")
                    )
                })
                .Where(x => x.Score >= threshold)
                .OrderByDescending(x => x.Score)
                .Select(x => x.Laptop)
                .ToList();

            return matched;
        }

    }
}
