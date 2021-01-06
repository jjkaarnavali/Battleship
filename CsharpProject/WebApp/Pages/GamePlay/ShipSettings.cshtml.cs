using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GamePlay
{
    public class ShipSettings : PageModel
    {
        
        [BindProperty]
        public int Carrier { get; set; }
        [BindProperty]
        public int Battleshipp { get; set; }
        [BindProperty]
        public int Submarine { get; set; }
        [BindProperty]
        public int Cruiser { get; set; }
        [BindProperty]
        public int Patrol { get; set; }
        [BindProperty]
        public string CanTouch { get; set; } = null!;
        
        [BindProperty]
        public int Id { get; set; }
        
        
        
        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("./P1PlaceShips", new { id = Id, carriers = Carrier, battleships = Battleshipp, submarines = Submarine, cruisers = Cruiser, patrols = Patrol, canTouch = CanTouch});
            
        }
    }
}