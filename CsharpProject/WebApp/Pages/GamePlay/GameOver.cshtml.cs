using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApp.Pages.GamePlay
{
    public class GameOver : PageModel
    {
        
        public Game Game { get; set; } = null!;

        [BindProperty]
        public string Winner { get; set; } = null!;
        
        
        public Battleships Battleships { get; set; } = new Battleships();
        
        public async Task OnGetAsync(string winner)
        {
            Winner = winner;
        }
    }
}
  