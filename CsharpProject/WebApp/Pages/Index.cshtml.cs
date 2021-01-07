﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
 using Newtonsoft.Json.Linq;

 namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DAL.ApplicationDbContext _context;

        public IndexModel(DAL.ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public IList<Game> Game { get;set; } = null!;
        
        [BindProperty]
        public string PlayerA { get; set; } = null!;
        [BindProperty]
        public string PlayerB { get; set; } = null!;
        
        [BindProperty]
        public int BoardSize { get; set; }
        [BindProperty]
        public string NewGame { get; set; } = null!;

        [BindProperty]
        public int Id { get; set; }
        

        public async Task OnGetAsync()
        {
            Game = await _context.Games
                .OrderBy(x => x.GameId)
                .Include(g => g.PlayerA)
                .Include(g => g.PlayerB).ToListAsync();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (Id != null && NewGame == null)
            {
                var loadGame = _context.Games.Find(Id);
                
                dynamic d = JObject.Parse(loadGame.BoardState);
                if (d.Height != null)
                {
                    GameBrain.Battleships.s = d.Height;
                }

                
            
                
                return RedirectToPage("./GamePlay/Index", new { id = loadGame.GameId});
                
            }
            if(PlayerA == null)
            {
                PlayerA = "PlayerA";
            }
            if(PlayerB == null)
            {
                PlayerB = "PlayerB";
            }
            if(BoardSize == null)
            {
                BoardSize = 10;
            }

            if (BoardSize < 5)
            {
                BoardSize = 5;
            }
            if (BoardSize > 30)
            {
                BoardSize = 30;
            }
            
            var playerA = new Player()
            {
                
                Name = PlayerA
            };
            
            var playerB = new Player()
            {
                Name = PlayerB
            };
            
           
            var gameOption = new GameOption()
            {
                Name = "Standard 10x10",
                BoardWidth = BoardSize,
                BoardHeight = BoardSize
            };
            
            var game = new Game()
            {
                PlayerA = playerA,
                PlayerB = playerB,
                GameOption = gameOption,
                BoardState = "",
                Description = DateTime.Now.ToLongDateString()
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            GameBrain.Battleships.s = BoardSize;
            
            return RedirectToPage("./GamePlay/ShipSettings", new { id = game.GameId});
        }
        
    }
}