using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabirintPathFinder
{
    abstract class Element //абстрактный класс элемента лабиринта
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
        public override void draw() //перегружаю абстрактную функцию отрисовки
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("#");
        }
    }
    class Void : Element //проход
    {
        public bool passed = false; //был ли здесь бот. По умолчанию его здесь не было
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
    struct Coord //структура, для того чтобы постоянно не передавать по два инта
    {
        public int x, y;
        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void draw() //функция отрисовки координаты в своих позициях
        {
            Console.SetCursorPosition(x, y);
            Console.Write("P");
        }
        public static bool operator==(Coord a, Coord b) //перегрузил операторы для удобного использования и потому что могу
        {
            return (a.x == b.x && a.y == b.y);
        }
        public static bool operator !=(Coord a, Coord b)
        {
            return (a.x != b.x && a.y != b.y);
        }
    }

    class Way //класс путь. Для сохранения пути от начал до выхода. У каждой контрольной точки свой путь до следующей, при успешном завершении если их вывести, то получится путь от начала до финиша
    {
        List<Coord> way = new List<Coord>();
        public Way()
        {
            
        }
        public int getSteps() //получение общего количества шагов в этом пути
        {
            return way.Count();
        }
        public void push(Coord coord) //добавления позиции в путь
        {
            way.Add(coord);
        }        
        public void draw() //отрисовка пути
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
        public Checkpoint prev = null; //поскольку контрольные точки будут храниться в виде "списка", то каждая конрольная точка знает о прошлой. 
        public Way way = new Way(); //у каждой контрольной точки свой путь до следующей. Если вывести все подпути, то получается финальный путь от начала до финиша
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
        public override void draw() //перегружаю абстрактную функцию, но вообще она не используется 
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
            labirint.draw(); //рисую лабиринт
            if(labirint.isPassable(new Coord(1, 1), new Coord(6, 5)))
            {
                Console.SetCursorPosition(0, 15);
                Console.WriteLine("Passable!");
            }
            else
            {
                Console.SetCursorPosition(0, 15);
                Console.WriteLine("Not passable :C");
            }
            Console.WriteLine("Бондаренко М.Д. Пишу это, потому что вы говорили писать свою фамилию на скриншотах");
        }
    }
}
