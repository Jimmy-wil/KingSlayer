namespace DefaultNamespace
{
    public class Player
    {
        public int Health { get; set; }
        public int Mana { get; set; }


        public void layer(int health, int mana)
        {
            Health = health;
            Mana = mana;
        }
        
        

    }
}