namespace Models
{

    public class Coin : Model
    {
        public Coin(int v)
        {
            body.Width = body.Height = 40;
            value = v;
            body.Tag = "coin";

        }
        public int value { get; set; }
        
    }
}
