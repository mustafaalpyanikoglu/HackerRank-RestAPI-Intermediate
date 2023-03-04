using Newtonsoft.Json;
using System.Net;
using System.Runtime.CompilerServices;

string competition = Console.ReadLine();

int year = Convert.ToInt32(Console.ReadLine().Trim());

int result = Result.getWinnerTotalGoals(competition, year);

Console.WriteLine(result);
class Result
{
    public static int getWinnerTotalGoals(string competition, int year)
    {
        string team = "";
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage res = client.GetAsync($"https://jsonmock.hackerrank.com/api/football_competitions?name={competition}&year={year}").Result)
            {
                using (HttpContent content = res.Content)
                {
                    string data = content.ReadAsStringAsync().Result;
                    WinnerTeam a = JsonConvert.DeserializeObject<WinnerTeam>(data);
                    team = a.data[0].winner;
                }
            }
        }
        Pageble pageble = new Pageble();
        int totalGoal = 0;
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage res = client.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?competition={competition}&year={year}&team2={team}&page=1").Result)
            {
                using (HttpContent content = res.Content)
                {
                    string data = content.ReadAsStringAsync().Result;
                    pageble = JsonConvert.DeserializeObject<Pageble>(data);
                }
            }
        }
        for (int i = 0; i < pageble.total_pages; i++)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res =  client.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?competition={competition}&year={year}&team2={team}&page={i}").Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = content.ReadAsStringAsync().Result;
                        pageble = JsonConvert.DeserializeObject<Pageble>(data);
                        totalGoal += pageble.data.Sum(c => c.team2goals);
                    }
                }
            }
        }
        pageble = new Pageble();
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage res =  client.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?competition={competition}&year={year}&team1={team}&page=1").Result)
            {
                using (HttpContent content = res.Content)
                {
                    string data = content.ReadAsStringAsync().Result;
                    pageble = JsonConvert.DeserializeObject<Pageble>(data);
                }
            }
        }
        for (int i = 0; i < pageble.total_pages; i++)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = client.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?competition={competition}&year={year}&team1={team}&page={i}").Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = content.ReadAsStringAsync().Result;
                        pageble = JsonConvert.DeserializeObject<Pageble>(data);
                        totalGoal += pageble.data.Sum(c => c.team1goals);
                    }
                }
            }
        }
        return totalGoal;

    }
    public static int getTotalGoals(string team, int year)
    {
        Pageble pageble = new Pageble();
        int totalGoal = 0;
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage res = client.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page=1").Result)
            {
                using (HttpContent content = res.Content)
                {
                    string data = content.ReadAsStringAsync().Result;
                    pageble = JsonConvert.DeserializeObject<Pageble>(data);
                }
            }
        }
        for (int i = 0; i < pageble.total_pages; i++)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = client.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={i}").Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = content.ReadAsStringAsync().Result;
                        pageble = JsonConvert.DeserializeObject<Pageble>(data);
                        totalGoal += pageble.data.Sum(c => c.team2goals);
                    }
                }
            }
        }
        pageble = new Pageble();
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage res = client.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page=1").Result)
            {
                using (HttpContent content = res.Content)
                {
                    string data = content.ReadAsStringAsync().Result;
                    pageble = JsonConvert.DeserializeObject<Pageble>(data);
                }
            }
        }
        for (int i = 0; i < pageble.total_pages; i++)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = client.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={i}").Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = content.ReadAsStringAsync().Result;
                        pageble = JsonConvert.DeserializeObject<Pageble>(data);
                        totalGoal += pageble.data.Sum(c => c.team1goals);
                    }
                }
            }
        }
        return totalGoal;
    }
}

public class Pageble
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<FootbalMatch> data { get; set; }
}
public class WinnerTeam
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<Team> data { get; set; }
}
public class Team
{
    public string winner { get; set; }
}

public class FootbalMatch
{
    public string competition { get; set; }
    public int year { get; set; }
    public string round { get; set; }
    public string team1 { get; set; }
    public string team2 { get; set; }
    public int team1goals { get; set; }
    public int team2goals { get; set; }
}
