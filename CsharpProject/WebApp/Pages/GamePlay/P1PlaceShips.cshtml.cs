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
    public class P1PlaceShips : PageModel
    {
        
        private readonly ILogger<IndexModel> _logger;
        private readonly DAL.ApplicationDbContext _context;

        public P1PlaceShips(DAL.ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public Game Game { get; set; } = null!;
        
        
        
        
        public Battleships Battleships { get; set; } = new Battleships();
        
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

        [BindProperty(SupportsGet = true)]
        public int PosX { get; set; } = 0;
        [BindProperty(SupportsGet = true)]
        public int PosY { get; set; } = 0;
        [BindProperty(SupportsGet = true)]
        public bool Horizontal { get; set; }
       


        public async Task OnGetAsync(int id, int? carriers, int? battleships, int? submarines, int? cruisers, int? patrols, string? canTouch, int? x, int? y, string? dir)
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
            
            if (carriers != null)
            {
                Carrier = carriers.Value;
                Battleshipp = battleships.Value;
                Submarine = submarines.Value;
                Cruiser = cruisers.Value;
                Patrol = patrols.Value;
            }
            if (canTouch == "yes")
            {
                CanTouch = "yes";
            }
            else
            {
                CanTouch = "no";
            }
            
            
            switch (dir)
            {
                case "up-left":
                    PosY--;
                    PosX--;
                    break;
                case "up":
                    PosY--;
                    break;
                case "up-right":
                    PosY--;
                    PosX++;
                    if (PosX == Battleships.s)
                    {
                        PosX--;
                    }
                    break;
                case "left":
                    PosX--;
                    break;
                case "right":
                    PosX++;
                    if (PosX == Battleships.s)
                    {
                        PosX--;
                    }
                    break;
                case "down-left":
                    PosY++;
                    PosX--;
                    break;
                case "down":
                    PosY++;
                    break;
                case "down-right":
                    PosY++;
                    PosX++;
                    if (PosX == Battleships.s)
                    {
                        PosX--;
                    }
                    break;
                case "rotate":
                    Horizontal = !Horizontal;
                    break;
            }

            if (PosX < 0)
            {
                PosX = 0;
            }
            if (PosY < 0)
            {
                PosY = 0;
            }
            if (PosX + 5 > Battleships.s && Horizontal)
            {
                PosX = Battleships.s - 5;
            }else if (PosX > Battleships.s && !Horizontal)
            {
                PosX = Battleships.s - 1;
            }
            if (PosY + 5 > Battleships.s && !Horizontal)
            {
                PosY = Battleships.s - 5;
            }else if (PosY > Battleships.s && Horizontal)
            {
                PosY = Battleships.s;
            }
            
            

            
            
            

            if (x != null && y != null)
            {
                bool touch = false;
                if (CanTouch == "yes")
                {
                    touch = true;
                }
                
                
                Battleships.PlaceShipP1(Horizontal, 5,x.Value, y.Value, touch);
                

                Game!.BoardState = Battleships.GetSerializedGameState();
                
                
            }
            _context.Games.Update(Game);
            await _context.SaveChangesAsync();
            
            
        }
    }
}