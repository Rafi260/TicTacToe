public class User
{
    public string username;
    public string email;
    public int totalMatch;
    public int win;
    public int lose;

    public User()
    {
    }

    public User(string username = "Guest", string email = "a", int totalMatch = 0, int win = 0, int lose = 0)
    {
        this.username = username;
        this.email = email;
        this.totalMatch = totalMatch;
        this.win = win;
        this.lose = lose;
    }
    /*public string getUsername() { return username; }
    public int getCoin() { return coin; }
    public int getGoals() { return goals; }
    public string getEmail() { return email; }

    public void setUsername(string username) { this.username = username; }
    public void setCoin(int coin) { this.coin = coin; }
    public void setGoals(int goals) { this.goals = goals; }*/

}
