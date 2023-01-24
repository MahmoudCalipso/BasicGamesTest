using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BasicGames.Data;
using BasicGamesEntities.Models;
using System.Data.SqlTypes;
using NuGet.Protocol;
using System.Linq;
using NuGet.Versioning;
using Microsoft.CodeAnalysis;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BasicGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
 
        private readonly BasicGamesContext _context;

        public GamesController(BasicGamesContext _context)
        {
            this._context = _context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Games>>> GetGames()
        {
            var getAllGames = await _context.Games.ToListAsync();
            return Ok(getAllGames);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Games>> GetGames(int id)
        {
            var getGameById = await _context.Games.FindAsync(id);
            if(getGameById == null)
            {
                return BadRequest();
            }
            return Ok(getGameById);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGames(int id, Games games)
        {
            var verifGame = await _context.Games.FindAsync(id);
            if (verifGame == null)
                return NotFound();
            
            verifGame.Genre= games.Genre;
            verifGame.PlayTime = games.PlayTime;
            verifGame.Game = games.Game;
            verifGame.UserId = games.UserId;
            verifGame.Platforms = games.Platforms;
            _context.Entry(verifGame).State = EntityState.Modified;
            await _context.SaveChangesAsync();
      
            return Ok(games);
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostGames([FromBody] IList<Games> games)
        {
            foreach (var game in games)
            {
                await _context.Games.AddAsync(game);
            }
            return Ok(games);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGames(int id)
        {
            var checkGame = await _context.Games.FindAsync(id);
            if (checkGame == null)
                return NotFound();
             _context.Games.Remove(checkGame) ;
            await  _context.SaveChangesAsync();
            return Ok("Your Game is removed ");
        }

        private async Task<bool> GamesExists(int id)
        {
            var checkGame = await _context.Games.FindAsync(id);
            if (checkGame != null) return true;
            return false;
        }
     
        [HttpGet("selectTopByPlaytime")]
        public async Task<IActionResult> SelectTopByPlayTime([BindRequired] string genre, [BindRequired] string platform)
        {
            var games = await _context.Games.ToListAsync();
            var gamesfiltred = games.Where(x => (x.Genre.ToLower() == genre.ToLower()) && x.Platforms.Contains(platform));
            var gameGroupByGameName = gamesfiltred.GroupBy(i => i.Game);
            var gameReduce = gameGroupByGameName.Select(s => new
            {
                game = s.Key,
                totalPlayed = s.Sum(w => w.PlayTime)
            });

            if (gameReduce == null)
            {
                return NotFound();
            }

            return Ok(gameReduce.MaxBy(g => g.totalPlayed));
        }
        [HttpGet("selectTopByPlayers")]
        public async Task<IActionResult> SelectTopByPlayers([BindRequired] string genre, [BindRequired] string platform)
        {
            var games = await _context.Games.ToListAsync();
            var gamesFiltred = games.Where(x => x.Genre.ToLower() == genre.ToLower() 
                                            && x.Platforms.Contains(platform))
                                    .GroupBy(i => i.Game.ToLower())
                                    .Select(g => new { name = g.Key, count = g.Count() }); 
            
            var maxUsers = gamesFiltred.MaxBy(u => u.count);
            var mostPlayedGames = gamesFiltred.Where(s => s.count == maxUsers?.count).ToList();

            if (mostPlayedGames == null)
            {
                return NotFound();
            }

            return Ok(mostPlayedGames);
        }
    }
}
