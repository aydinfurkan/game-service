namespace GameService.Domain.ValueObject
{
    public class Position
    {
        public int X;
        public int Y;
        public int Z;

        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Set(int x, int y, int z)
        {
            (X,Y,Z) = (x,y,z);
        }
        
        public (int, int, int) Get()
        {
            return (X,Y,Z);
        }
    }
}