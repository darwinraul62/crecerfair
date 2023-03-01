using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Web.Models;
using Crecer.Fair.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Web.ViewComponents
{
    public class FairNavigationMenuViewComponent : ViewComponent
    {
        private readonly IFairService fairService;
        public FairNavigationMenuViewComponent(IFairService fairService)
        {
            this.fairService = fairService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        //int maxPriority, bool isDone)//example params
        {
            var fair = await fairService.GetMainAsync();
            var stands = await fairService.GetStandsAsync(fair.FairId);

            FairNavigationMenuViewModel model = new FairNavigationMenuViewModel();
            model.Categories = new List<StandProviderCategory>();

            if (stands != null && stands.Any())
            {
                model.Categories = stands.Where(p => p.ProviderCategoryId.HasValue).
                    Select(p => new StandProviderCategory { CategoryId = p.ProviderCategoryId.Value, Name = p.ProviderCategoryName })
                    .Distinct().ToList();

                var uniqueStands = stands.GroupBy(p=>p.ProviderName).
                    Select(p => new
                    {
                        PositionRef = p.Min(p=>p.PositionRef)                        
                    });

                var standsNav = stands.Where(p=> uniqueStands.Any(q=>q.PositionRef ==  p.PositionRef) ).
                    Select(p=>new StandProviderNav()
                    {
                        Name = p.ProviderName,
                        PositionRef = p.PositionRef,
                        StandId = p.StandId,
                        CategoryId = p.ProviderCategoryId,
                        Url =  Url.Action("Stand", "Fair", new { id = p.StandId })
                    });

                foreach (var c in model.Categories)
                {
                    c.Stands = standsNav.Where(p => p.CategoryId == c.CategoryId).ToList();
                }
            }

            return View(model);
        }
    }
}
