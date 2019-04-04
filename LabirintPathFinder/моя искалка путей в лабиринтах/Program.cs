using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabirintPathFinder
{
    abstract class Element
    {
        protected int x, y;
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public abstract void draw();

    }
    class Wall : Element //стена
    {
        public Wall()
        { }
        public Wall(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public override void draw()
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("#");
        }
    }
    class Void : Element //проход
    {
        public bool passed = false;
        public Void()
        { }
        public Void(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public override void draw()
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(" ");
        }
    }
    struct Coord
    {
        public int x, y;
        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void draw()
        {
            Console.SetCursorPosition(x, y);
            Console.Write("P");
        }
        public static bool operator==(Coord a, Coord b)
        {
            return (a.x == b.x && a.y == b.y);
        }
        public static bool operator !=(Coord a, Coord b)
        {
            return (a.x != b.x && a.y != b.y);
        }
    }

    class Way
    {
        List<Coord> way = new List<Coord>();
        public Way()
        {
            
        }
        public void push(Coord coord)
        {
            way.Add(coord);
        }        
        public List<Coord> getWay()
        {
            return way;
        }
        public void draw()
        {
            for(int i = 0; i < way.Count(); i++)
            {
                Console.SetCursorPosition(way[i].x, way[i].y);
                Console.Write("*");
            }
        }
    }
    class Checkpoint : Element //контрольная точка. в принципе, ему не обязательно наследовать Element, нет необходимости хранить его в массиве этого типа, но мне было лень писать теже самые свойства что и в Element
    {
        public Checkpoint prev = null;
        public Way way = new Way();
        public Checkpoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Checkpoint(Coord coord)
        {
            x = coord.x;
            y = coord.y;
        }
        public override void draw()
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("C");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Labirint labirint = new Labirint();
            labirint.draw();
            labirint.isPassable(new Coord(1, 1), new Coord(4, 1));
            Console.SetCursorPosition(20, 20);
        }
    }
}
