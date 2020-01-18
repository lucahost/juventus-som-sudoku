using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Juventus.SOM.Sudoku.Models;
using Newtonsoft.Json;

namespace Juventus.SOM.Sudoku.Services
{
    public enum GameDifficulty
    {
        Easy = 1,
        Medium = 2,
        Difficult = 3
    }

    public interface ISudokuService
    {
        Task<List<Field>> GetSudokuGameFromWebApi(GameDifficulty difficulty);
    }

    public class SudokuService : ISudokuService
    {
        public async Task<List<Field>> GetSudokuGameFromWebApi(GameDifficulty difficulty)
        {
            string endpoint = $"http://www.cs.utep.edu/cheon/ws/sudoku/new/?level={(int)difficulty}&size=9";
            using (var client = new HttpClient())
            {
                var resp = await client.GetAsync(endpoint);

                if (resp.IsSuccessStatusCode)
                {
                    string response = await resp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<SudokuApiResponse>(response);
                    return new List<Field>(result.Squares);
                }

            }
            return null;
        }
    }
}
