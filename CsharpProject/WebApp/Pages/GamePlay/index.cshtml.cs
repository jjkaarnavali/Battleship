using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.GamePlay
{
    public class Index : PageModel
    {
        
        private readonly ILogger<IndexModel> _logger;
        private readonly DAL.ApplicationDbContext _context;

        public Index(DAL.ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public Game Game { get; set; } = null!;
        
        
        
        
        public Battleships Battleships { get; set; } = new Battleships();

        public string Winner { get; set; } = null!;

        public bool SwitchPlayer { get; set; } = false;
       


        public async Task OnGetAsync(int id, int? x, int? y)
        {
            Game = await _context.Games
                .Where(x => x.GameId == id)
                .FirstOrDefaultAsync();
            
            Game.PlayerA = _context.Players.Find(Game.PlayerAId);
            Game.PlayerB = _context.Players.Find(Game.PlayerBId);

            if (Game != null && Game.BoardState.Length > 0)
            {
                
                Battleships.SetGameStateFromJsonString(Game.BoardState);
            }

            

            if (x != null && y != null)
            {
                bool checkIfHit = Battleships.NextMoveByP1;
                Battleships.TakeAShot(x.Value, y.Value, Battleships.NextMoveByP1);

                if (checkIfHit != Battleships.NextMoveByP1)
                {
                    SwitchPlayer = true;
                }
                

                Game!.BoardState = Battleships.GetSerializedGameState();

                
            }

            _context.Games.Update(Game);
            await _context.SaveChangesAsync();
            
            bool isOver = Battleships.IsGameOver();
            bool P1Won = false;
            if (isOver)
            {
                if (Battleships.NextMoveByP1)
                {
                    P1Won = true;
                }
                

                if (P1Won)
                {
                    Winner = Game!.PlayerA.Name;
                }
                else
                {
                    Winner = Game!.PlayerB.Name;
                }
            }
        }
        
    }
}