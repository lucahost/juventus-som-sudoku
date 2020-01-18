using System;
using System.Collections.Generic;
using System.Linq;
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
        List<Field> InitFields();
        Task<List<Field>> GetSudokuGameFromWebApi(GameDifficulty difficulty);
        List<Field> ReplacePreDefinedFields(List<Field> baseList, List<Field> preFields);
    }

    public class SudokuService : ISudokuService
    {
        public List<Field> InitFields()
        {
            var res = new List<Field>(81);
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    res.Add(new Field(i, j));
                }
            }
            return res;
        }

        public List<Field> ReplacePreDefinedFields(List<Field> baseList, List<Field> preDefinedFields)
        {
            foreach (Field preDefined in preDefinedFields)
            {
                var itemToReplaceValue = baseList.FirstOrDefault(field => field.X == preDefined.X && field.Y == preDefined.Y);
                if (itemToReplaceValue != null)
                {
                    itemToReplaceValue.Value = preDefined.Value;
                    itemToReplaceValue.Predefined = true;
                }
            }

            return baseList;
        }

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
